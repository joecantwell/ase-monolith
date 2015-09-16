// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using Broker.Domain;
using Broker.Service.Contracts;

namespace Broker.Services
{
    public class RestFactory : IRestFactory
    {
        private readonly ISystemConfiguration _systemConfiguration;

        public RestFactory(ISystemConfiguration systemConfiguration)
        {
            _systemConfiguration = systemConfiguration;
        }

        public IRestGateway<T> CreateGateway<T>(EndPoint endpoint) where T : class
        {
            switch (endpoint)
            {
                case EndPoint.InsuranceService:
                    return new RestGateway<T>(_systemConfiguration.ServicesBaseUri);
                case EndPoint.CarFinder:
                    return new RestGateway<T>(_systemConfiguration.CarFinderBaseUri);
                default:
                    return null;
            }   
        }
    }
}
