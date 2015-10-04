// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Collections.Generic;
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
            int quoteId = await _carQuoteRequestWriter.AddQuote(request);

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
            };

            var gateway = _restFactory.CreateGateway<ServiceCarInsuranceQuoteRequest>(EndPoint.InsuranceService);

            var response = await gateway.Post(serviceRequest, "/api/carinsurancequote");

            if (response != null)
            {
                var quotes = await response.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();

                await _carQuoteResponseWriter.AddResponse(Mapper.Map<IEnumerable<CarQuoteResponseDto>>(quotes));
            }

            return quoteId;

        }

        public async Task<IEnumerable<CarQuoteResponseDto>> ListQuotes(int quoteId)
        {
            return await _carQuoteResponseReader.GetQuoteResponses(quoteId);
        }
    }
}
