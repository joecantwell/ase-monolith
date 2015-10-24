// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Runtime.Serialization;

namespace Thirdparty.Api.Contracts
{
    [DataContract]
    public class ServiceCarInsuranceQuoteRequest
    {
        [DataMember]
        public int QuoteRequestId { get; set; }

        [DataMember]
        public string County { get; set; }

        [DataMember]
        public int NoClaimsDiscountYears { get; set; }

        [DataMember]
        public decimal VehicleValue { get; set; }

        [DataMember]
        public int DriverAge { get; set; }

        [DataMember]
        public string ModelDesc { get; set; }

        [DataMember]
        public int ManufYear { get; set; }

        [DataMember]
        public string CurrentRegistration { get; set; }

        [DataMember]
        public bool? IsImport { get; set; }

        [DataMember]
        public Insurer Insurer { get;set;}
    }
}
