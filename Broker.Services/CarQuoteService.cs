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
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
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

        private readonly ICarQuoteRequestWriter _carQuoteRequestWriter;
        private readonly IRestFactory _restFactory;
        private readonly ICarQuoteResponseWriter _carQuoteResponseWriter;
        private readonly ICarQuoteResponseReader _carQuoteResponseReader;
        private readonly Timer _serviceTimer;
        private Boolean _isTimedOut, _isLoadComplete;

        public CarQuoteService(ICarQuoteRequestWriter carQuoteRequestWriter,
                                ICarQuoteResponseWriter carQuoteResponseWriter,
                                ICarQuoteResponseReader carQuoteResponseReader,
                                IRestFactory restFactory)
        {
            _carQuoteResponseReader = carQuoteResponseReader;
            _carQuoteResponseWriter = carQuoteResponseWriter;
            _restFactory = restFactory;
            _carQuoteRequestWriter = carQuoteRequestWriter;

            _serviceTimer = new Timer(); // timer with 5 secoond interval
            _serviceTimer.Elapsed += ServiceTimerElapsed;
            _serviceTimer.Interval = 5000; // 5 second interval
        }

        void ServiceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // 5 seconds are up
            _serviceTimer.Stop();
            _isTimedOut = true;

            _logger.Debug("Timeout Fired!");
        }

        public async Task<int> AddQuotes(CarQuoteRequestDto request, VehicleDetailsDto vehicle)
        {
            int quoteId = await _carQuoteRequestWriter.AddQuote(request);

            var gateway = _restFactory.CreateGateway<ServiceCarInsuranceQuoteRequest>(EndPoint.InsuranceService);

            // create a collection to hold all parallel responses
            IEnumerable<ServiceCarInsuranceQuoteResponse> allQuotes = new List<ServiceCarInsuranceQuoteResponse>();

  
            // build the initial object to post
            var serviceRequest = new ServiceCarInsuranceQuoteRequest
            {
                QuoteRequestId = quoteId,
                NoClaimsDiscountYears = request.NoClaimsDiscountYears.HasValue ? request.NoClaimsDiscountYears.Value : 0,
                VehicleValue = request.VehicleValue.HasValue ? request.VehicleValue.Value : 0,
                CurrentRegistration = vehicle.CurrentRegistration,
                DriverAge = request.DriverAge.HasValue ? request.DriverAge.Value : 0,
                ModelDesc = vehicle.ModelDesc,
                IsImport = vehicle.IsImport,
                ManufYear = vehicle.ManufYear.HasValue ? vehicle.ManufYear.Value : 0
            };

            _isTimedOut = false;
            _serviceTimer.Start(); // start the timer.
            _isLoadComplete = false;

            serviceRequest.Insurer = Insurer.AllianceDirect; // define insurer prior to post
            Task<HttpResponseMessage> allianceResponse = gateway.Post(serviceRequest, "api/carinsurancequote");

            serviceRequest.Insurer = Insurer.AxaCar;
            Task<HttpResponseMessage> axaResponse = gateway.Post(serviceRequest, "api/carinsurancequote");

            serviceRequest.Insurer = Insurer.Fbd;
            Task<HttpResponseMessage> fbdResponse = gateway.Post(serviceRequest, "api/carinsurancequote");

            serviceRequest.Insurer = Insurer.OneTwoThree;
            Task<HttpResponseMessage> oneTwoThreeResponse = gateway.Post(serviceRequest, "api/carinsurancequote");

            serviceRequest.Insurer = Insurer.ZurichCar;
            Task<HttpResponseMessage> zurichResponse = gateway.Post(serviceRequest, "api/carinsurancequote");

            await Task.WhenAll(allianceResponse, axaResponse, fbdResponse, oneTwoThreeResponse, zurichResponse);

            if (allianceResponse != null)
            {
                _logger.Trace("Response from {0}", Insurer.AllianceDirect.ToString());
                var quotes = await allianceResponse.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }

            if (axaResponse != null)
            {
                _logger.Trace("Response from {0}", Insurer.AxaCar.ToString());
                var quotes = await axaResponse.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }

            if (fbdResponse != null)
            {
                _logger.Trace("Response from {0}", Insurer.Fbd.ToString());
                var quotes = await fbdResponse.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }

            if (oneTwoThreeResponse != null)
            {
                _logger.Trace("Response from {0}", Insurer.OneTwoThree.ToString());
                var quotes = await oneTwoThreeResponse.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }

            if (zurichResponse != null)
            {
                _logger.Trace("Response from {0}", Insurer.ZurichCar.ToString());
                var quotes = await zurichResponse.Result.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();
                allQuotes = allQuotes.Concat(quotes);
            }
            

            // Valid but slower response (await after each call == sync approach)
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

        public async Task<IEnumerable<CarQuoteResponseDto>> ListQuotes(int quoteId)
        {
            return await _carQuoteResponseReader.GetQuoteResponses(quoteId);
        }
    }
}
