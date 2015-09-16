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
    public class VehicleDetailsDto
    {
        public int VehicleId { get; set; }
        public int? ManufYear { get; set; }
        public int? ModelId { get; set; }
        public string CurrentRegistration { get; set; }
        public string PreviousRegistrations { get; set; }
        public string Colour { get; set; }
        public bool? IsImport { get; set; }
        public DateTime? UTCDateAdded { get; set; }
    }
}
