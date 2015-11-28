
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Domain.Queries;
using Broker.Service.Contracts;
using Thirdparty.Api.Contracts;

namespace Broker.Services
{
    public class CarQuoteService : ICarQuoteService
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private const double Timeout = 10.0; //in seconds

        private readonly ICarQuoteRequestWriter _carQuoteRequestWriter;
        private readonly IRestFactory _restFactory;
        private readonly ICarQuoteResponseWriter _carQuoteResponseWriter;
        private readonly ICarQuoteResponseReader _carQuoteResponseReader;

        public CarQuoteService(ICarQuoteRequestWriter carQuoteRequestWriter,
                                ICarQuoteResponseWriter carQuoteResponseWriter,
                                ICarQuoteResponseReader carQuoteResponseReader,
                                IRestFactory restFactory)
        {
            _carQuoteResponseReader = carQuoteResponseReader;
            _carQuoteResponseWriter = carQuoteResponseWriter;
            _restFactory = restFactory;
            _carQuoteRequestWriter = carQuoteRequestWriter;
        }


        public async Task<int> AddQuotes(CarQuoteRequestDto request, VehicleDetailsDto vehicle)
        {
            int quoteId = await _carQuoteRequestWriter.AddQuote(request); // add it to the database

            var gateway = _restFactory.CreateGateway<ServiceCarInsuranceQuoteRequest>(EndPoint.InsuranceService);

            var tasksToCallService = new List<Task<HttpResponseMessage>>(); // task collection

            // create a task for each external service call
            foreach (var insurer in Enum.GetValues(typeof (Insurer)))
            {                
                _logger.Trace("Sending Request for {0}", insurer);

                var serviceRequest = BuildServiceRequest((Insurer)insurer, quoteId, request, vehicle);
                Task<HttpResponseMessage> response = gateway.Post(serviceRequest, "api/carinsurancequote");

                tasksToCallService.Add(response);
            }
 
            // add timeout handling for each service call
            TimeSpan timeOut = TimeSpan.FromSeconds(Timeout);
            var tasksToComplete = tasksToCallService.Select(serviceCall => Task.WhenAny(serviceCall, Task.Delay(timeOut))).ToList();

            // wait for the tasks to complete or timeout
            await Task.WhenAll(tasksToComplete);

            // create a collection to hold all async responses
            IEnumerable<ServiceCarInsuranceQuoteResponse> allQuotes = new List<ServiceCarInsuranceQuoteResponse>();

            // read and merge the results from successful tasks
            foreach (var response in tasksToCallService.Where(resp => resp.Status == TaskStatus.RanToCompletion && resp.Result.IsSuccessStatusCode))
            {
                var quotes = await response.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }
           
            // Valid but slower alternative (await after each call == sync approach)
            /*
            foreach (var insurer in Enum.GetValues(typeof(Insurer)))
            {
                // build the object to post
                var serviceRequest = new ServiceCarInsuranceQuoteRequest
                {
                    QuoteRequestId = quoteId,
                    NoClaimsDiscountYears = request.NoClaimsDiscountYears.HasValue ? request.NoClaimsDiscountYears.Value : 0,
                    VehicleValue = request.VehicleValue.HasValue ? request.VehicleValue.Value : 0,
                    CurrentRegistration = vehicle.CurrentRegistration,
                    DriverAge = request.DriverAge.HasValue ? request.DriverAge.Value : 0,
                    ModelDesc = vehicle.ModelDesc,
                    IsImport = vehicle.IsImport,
                    ManufYear = vehicle.ManufYear.HasValue ? vehicle.ManufYear.Value : 0,
                    Insurer = (Insurer)insurer
                };

                var response = await gateway.Post(serviceRequest, "api/carinsurancequote");

                if (response != null)
                {
                    _logger.Trace("Response from {0}", insurer.ToString());
                    var quotes = await response.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                    allQuotes = allQuotes.Concat(quotes);                   
                }
            }
            */

            // determine which is the cheapest by quote type
            var responsesToSave = Mapper.Map<IEnumerable<CarQuoteResponseDto>>(allQuotes);

            var carQuoteResponseDtos = responsesToSave as CarQuoteResponseDto[] ?? responsesToSave.ToArray();
            var cheapestQuotes = carQuoteResponseDtos
                                            .GroupBy(x => x.QuoteType)
                                            .SelectMany(y => y.OrderBy(x => x.QuoteValue)
                                            .Take(1));

            // set ischeapest flag in the database
            foreach (var quote in carQuoteResponseDtos)
            {
                if (cheapestQuotes.Contains(quote))
                {
                    quote.IsCheapest = true;
                }
            }

            await _carQuoteResponseWriter.AddResponse(carQuoteResponseDtos);

            return quoteId;

        }

        private ServiceCarInsuranceQuoteRequest BuildServiceRequest(Insurer insurer, int quoteId, CarQuoteRequestDto request, VehicleDetailsDto vehicle)
        {
            return new ServiceCarInsuranceQuoteRequest
            {
                QuoteRequestId = quoteId,
                NoClaimsDiscountYears = request.NoClaimsDiscountYears.HasValue ? request.NoClaimsDiscountYears.Value : 0,
                VehicleValue = request.VehicleValue.HasValue ? request.VehicleValue.Value : 0,
                CurrentRegistration = vehicle.CurrentRegistration,
                DriverAge = request.DriverAge.HasValue ? request.DriverAge.Value : 0,
                ModelDesc = vehicle.ModelDesc,
                IsImport = vehicle.IsImport,
                ManufYear = vehicle.ManufYear.HasValue ? vehicle.ManufYear.Value : 0,
                Insurer = insurer
            };
        }


        public async Task<IEnumerable<CarQuoteResponseDto>> ListQuotes(int quoteId)
        {
            return await _carQuoteResponseReader.GetQuoteResponses(quoteId);
        }
    }
}
