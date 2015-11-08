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
using System.ComponentModel.DataAnnotations;

namespace Broker.Domain.Models
{
    public class CarQuoteRequestDto
    {
        public int CarQuoteId { get; set; }
        public int VehicleId { get; set; }
        public Guid VehicleRef { get; set; }
        public int? CountyId { get; set; }
        public int? NoClaimsDiscountYears { get; set; }
        public decimal? VehicleValue { get; set; }
        public int? DriverAge { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        public DateTime? UTCDateAdded { get; set; }
    }
}
