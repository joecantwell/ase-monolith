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
using Broker.Domain.Models;

namespace Broker.Tests.TestFixtures
{
    public class QuoteRequestBuilder
    {
        public CarQuoteRequestDto Build()
        {
            return new CarQuoteRequestDto
            {
                CountyId = 1,
                DriverAge = 43,
                EmailAddress = "joe@test.io",
                NoClaimsDiscountYears = 5,
                Telephone = "0879562324",
                VehicleValue = Convert.ToDecimal(7000)
            };
        }

        public static implicit operator CarQuoteRequestDto(QuoteRequestBuilder instance)
        {
            return instance.Build();
        }
    }
}
