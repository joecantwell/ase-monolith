// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using Broker.Domain.Models;
using BrokerUI.web.Models;
using Newtonsoft.Json;

namespace Broker.Tests.TestFixtures
{
    public class CarInsuranceViewModelBuilder
    {
        private VehicleDetailsDto _vehicle;
        private CarQuoteRequestDto _carQuoteRequest;

        public CarInsuranceViewModel Build()
        {
            return new CarInsuranceViewModel
            {
                JsonVehicle = JsonConvert.SerializeObject(_vehicle),
                CarQuoteRequest = _carQuoteRequest
            };
        }

        public CarInsuranceViewModelBuilder WithVehicle(VehicleDetailsDto vehicle)
        {
            this._vehicle = vehicle;
            return this;
        }

        public CarInsuranceViewModelBuilder WithRequest(CarQuoteRequestDto request)
        {
            this._carQuoteRequest = request;
            return this;
        }

        public static implicit operator CarInsuranceViewModel(CarInsuranceViewModelBuilder instance)
        {
            return instance.Build();
        }
    }
}
