// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using ActorUI.Actors.Messages;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Domain.Queries;
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

        private readonly Entities _context;
        private readonly IActorRef _quoteServices;
        private IEnumerable<CarQuoteResponseDto> _allQuotes;
      

        public QuoteCoordinatorActor()
        {
            _context = new Entities();
            _allQuotes = new List<CarQuoteResponseDto>();

            // create a child actor for each insurance service
            int numInsurers = Enum.GetNames(typeof(Insurer)).Length;
            _quoteServices = Context.ActorOf(Props.Create(typeof(QuoteServiceActor)).WithRouter(new RoundRobinPool(numInsurers)), "ServiceInterrogator");

            Ready();
        }

        private void Ready()
        {
            Receive<SaveQuoteRequest>(req =>
            {
                var senderClosure = Sender;
                ICarQuoteRequestWriter carQuoteRequestWriter = new CarQuoteRequestWriter(_context);

                carQuoteRequestWriter.AddQuote(req.QuoteRequest).ContinueWith(s => s.Result).PipeTo(senderClosure);
            });

            Receive<RequestQuotes>(req =>
            {
                // build the object to post
                var serviceRequest = new ServiceCarInsuranceQuoteRequest
                {
                    QuoteRequestId = req.QuoteRequest.CarQuoteId,
                    NoClaimsDiscountYears = req.QuoteRequest.NoClaimsDiscountYears.HasValue ? req.QuoteRequest.NoClaimsDiscountYears.Value : 0,
                    VehicleValue = req.QuoteRequest.VehicleValue.HasValue ? req.QuoteRequest.VehicleValue.Value : 0,
                    CurrentRegistration = req.VehicleDetails.CurrentRegistration,
                    DriverAge = req.QuoteRequest.DriverAge.HasValue ? req.QuoteRequest.DriverAge.Value : 0,
                    ModelDesc = req.VehicleDetails.ModelDesc,
                    IsImport = req.VehicleDetails.IsImport,
                    ManufYear = req.VehicleDetails.ManufYear.HasValue ? req.VehicleDetails.ManufYear.Value : 0,
                };

                // push the service request to the service workers            
                foreach (var insurer in Enum.GetValues(typeof(Insurer)))
                {
                    serviceRequest.Insurer = (Insurer)insurer;
                    _quoteServices.Tell(new GetQuotesFromService(req.ServiceLocation, serviceRequest));

                    _log.Debug("Fired Request for {0}", insurer);
                }

            });

            Receive<QuotesReturnedFromService>(req =>
            {
                 _log.Debug("Appending {0} Logs", req.QuotesFromService.Count);
               
                 ICarQuoteResponseWriter carQuoteResponseWriter = new CarQuoteResponseWriter(_context);
                 carQuoteResponseWriter.AddResponse(req.QuotesFromService);
            });

            Receive<ListQuotes>(req =>
            {
                var senderClosure = Sender;
                ICarQuoteResponseReader carQuoteResponseReader = new CarQuoteResponseReader(_context);

                carQuoteResponseReader.GetQuoteResponses(req.QuoteId).ContinueWith(s =>
                {
                    var quotes = s.Result; // records returned from db.
                    var cheapestQuotes = quotes
                                           .GroupBy(x => x.QuoteType)
                                           .SelectMany(y => y.OrderBy(x => x.QuoteValue)
                                           .Take(1));

                    // set ischeapest flag for UI
                    foreach (var quote in quotes)
                    {
                        if (cheapestQuotes.Contains(quote))
                        {
                            quote.IsCheapest = true;
                        }
                    }

                    return quotes;

                }).PipeTo(senderClosure);
            });
        }
    }
}
