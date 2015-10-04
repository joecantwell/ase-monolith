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
    public class ServiceCarInsuranceQuoteResponse
    {
        [DataMember]
        public int QuoteId { get; set; }

        [DataMember]
        public string InsuranceCompany { get; set; }

        [DataMember]
        public decimal QuoteValue { get; set; }

        [DataMember]
        public string QuoteDescription { get; set; }

        [DataMember]
        public QuoteType QuoteType { get; set; }
    }

    public enum QuoteType
    {
        Comprehensive,
        ThirdPartyFireAndTheft,
        ThirdParty
    }
}
