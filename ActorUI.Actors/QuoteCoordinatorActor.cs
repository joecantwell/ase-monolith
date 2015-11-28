
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ActorUI.Actors.Messages;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Persistance;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors
{
    /// <summary>
    /// http://getakka.net/docs/Props
    /// </summary>
    public class QuoteCoordinatorActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private readonly IActorRef _quoteServicesPool;
        private readonly ICarQuoteResponseWriter _carQuoteResponseWriter;
        private readonly ICarQuoteRequestWriter _carQuoteRequestWriter;

        private readonly List<CarQuoteResponseDto> _quoteResults = new List<CarQuoteResponseDto>();
        private readonly int _numInsurers = Enum.GetNames(typeof (Insurer)).Length; // no of insurers configured
        private readonly Timer _serviceTimer;
        private Boolean _isLoadComplete;

        public QuoteCoordinatorActor(Entities context)
        {
            _carQuoteResponseWriter = new CarQuoteResponseWriter(context);
            _carQuoteRequestWriter = new CarQuoteRequestWriter(context);

            _serviceTimer = new Timer(); // timer with 5 secoond interval
            _serviceTimer.Elapsed += ServiceTimerElapsed;
            _serviceTimer.Interval = 5000; // 5 second interval

            // create a child actor for each insurance service          
            _quoteServicesPool =
                Context.ActorOf(
                    Props.Create(() => new QuoteServiceActor())
                         .WithRouter(new RoundRobinPool(_numInsurers)), "ServiceInterrogator");

            MessageReceiver();
        }

        void ServiceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _serviceTimer.Stop();
            _log.Debug("Timeout Fired!");

            // tell this actors receiver that the timeout has expired
            SystemActors.QuoteActor.Tell(new TimedOutOrComplete(isTimedOut: true));
        }

        private void MessageReceiver()
        {
            Receive<RequestQuotes>(req =>
            {               
                // write the initial request to the db.
                Task<int> quoteId = _carQuoteRequestWriter.AddQuote(req.QuoteRequest).ContinueWith(s => s.Result);

                _quoteResults.Clear(); // clean down any previous results
                _serviceTimer.Start(); // start the timer.
                _isLoadComplete = false;

                // push the service request message to the service workers            
                foreach (var insurer in Enum.GetValues(typeof (Insurer)))
                {
                    // build the object to post
                    var serviceRequest = new ServiceCarInsuranceQuoteRequest
                    {
                        QuoteRequestId = quoteId.Result, // created quoteId
                        NoClaimsDiscountYears =
                            req.QuoteRequest.NoClaimsDiscountYears.HasValue
                                ? req.QuoteRequest.NoClaimsDiscountYears.Value
                                : 0,
                        VehicleValue = req.QuoteRequest.VehicleValue.HasValue ? req.QuoteRequest.VehicleValue.Value : 0,
                        CurrentRegistration = req.VehicleDetails.CurrentRegistration,
                        DriverAge = req.QuoteRequest.DriverAge.HasValue ? req.QuoteRequest.DriverAge.Value : 0,
                        ModelDesc = req.VehicleDetails.ModelDesc,
                        IsImport = req.VehicleDetails.IsImport,
                        ManufYear = req.VehicleDetails.ManufYear.HasValue ? req.VehicleDetails.ManufYear.Value : 0,
                        Insurer = (Insurer) insurer
                    };

                    _quoteServicesPool.Tell(new GetQuotesFromService(req.ServiceLocation, serviceRequest));

                    _log.Debug("Fired Request for {0}", insurer);
                }

                this.Sender.Tell(quoteId.Result); // pass the quoteId back to the controller
            });

            // Listener for QuoteServices Results. Append to Results Collection and sort as required
            Receive<QuotesReturnedFromService>(req =>
            {
                if (req == null)
                    return;

                _log.Debug("Appending {0} Logs", req.QuotesFromService.Count);

                _quoteResults.AddRange(req.QuotesFromService);  

                int insurersReturned = _quoteResults.GroupBy(x => x.Insurer).Count();
                if (_numInsurers.Equals(insurersReturned))
                    this.Sender.Tell(new TimedOutOrComplete(isComplete: true));
            });


            Receive<TimedOutOrComplete>(req =>
            {
                if (req.IsTimedOut && req.IsComplete) // not interested in a response post timeout
                    return;
                
                // sort the collated results only if all insurers have returned or if
                // the default 5 second time out has been reached.
                if (req.IsTimedOut || req.IsComplete)
                {
                    var cheapestQuotes = _quoteResults
                        .GroupBy(x => x.QuoteType)
                        .SelectMany(y => y.OrderBy(x => x.QuoteValue)
                            .Take(1));

                    // set ischeapest flag for UI
                    foreach (var quote in _quoteResults)
                    {
                        if (cheapestQuotes.Contains(quote))
                        {
                            quote.IsCheapest = true;
                        }
                    }

                    // persist results and pass the bool result back to this Actor which will set the IsLoadComplete flag
                    _carQuoteResponseWriter.AddResponse(_quoteResults).ContinueWith(s => s.Result).PipeTo(Self);
                }
            });

            Receive<bool>(req => { _isLoadComplete = req; }); // success bool piped from write to Db query above

            Receive<IsLoadComplete>(req => this.Sender.Tell(_isLoadComplete)); // is Complete Flag set Request

            Receive<ListQuotes>(req => // pass back the complete result set once everything has been persisted.
            {
                if (_isLoadComplete)
                    Sender.Tell(_quoteResults);
                
                Sender.Tell(null);
            });
        }
    }
}
