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
    public class VehicleDetailsBuilder
    {
        public VehicleDetailsDto Build()
        {
            return new VehicleDetailsDto
            {
                BodyType = "Estate",
                VehicleRef = Guid.NewGuid(),
                Colour = "Black",
                CurrentRegistration = "161T3453",
                FuelType = "Diesel",
                IsImport = false,
                ManufYear = 2016,
                ModelDesc = "2016 Audi A4 2.0 TDI Avant 173 BHP 5 DR",
                ModelName = "Audi A4 Avant",
                Transmission = "Manual"
            };
        }

        public static implicit operator VehicleDetailsDto(VehicleDetailsBuilder instance)
        {
            return instance.Build();
        }
    }
}
