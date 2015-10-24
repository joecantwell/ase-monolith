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
    public class CarQuoteResponseDto
    {
        public int ResponseId { get; set; }
        public int CarQuoteId { get; set; }
        public string Insurer { get; set; }
        public string QuoteType { get; set; }
        public decimal? QuoteValue { get; set; }
        public bool? IsCheapest { get; set; }
        public DateTime? UTCDateAdded { get; set; }
    }
}
