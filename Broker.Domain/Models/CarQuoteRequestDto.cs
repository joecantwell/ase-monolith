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

namespace Broker.Domain.Models
{
    public class CarQuoteRequestDto
    {
        public int CarQuoteId { get; set; }
        public string CarRegistration { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string CarDesc { get; set; }
        public int? CarYear { get; set; }
        public int? CountyId { get; set; }
        public int? NoClaimsDiscountYears { get; set; }
        public decimal? VehicleValue { get; set; }
        public int? DriverAge { get; set; }
        public string EmailAddress { get; set; }
        public string Telephone { get; set; }
        public DateTime? UTCDateAdded { get; set; }
    }
}
