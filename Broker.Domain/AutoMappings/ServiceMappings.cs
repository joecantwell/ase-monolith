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
using AutoMapper;
using Broker.Domain.Models;
using Thirdparty.Api.Contracts;

namespace Broker.Domain.AutoMappings
{
    public class ServiceMappings : Profile
    {
        protected override void Configure()
        {
            ServiceToDomainMappings();
        }

        public void ServiceToDomainMappings()
        {
            CreateMap<ServiceCarInsuranceQuoteResponse, CarQuoteResponseDto>()
                .ForMember(dest => dest.ResponseId, opts => opts.Ignore())
                .ForMember(dest => dest.CarQuoteId, opts => opts.MapFrom(x => x.QuoteId))
                .ForMember(dest => dest.Insurer, opts => opts.MapFrom(x => x.InsuranceCompany))
                .ForMember(dest => dest.QuoteType, opts => opts.MapFrom(x => x.QuoteType.GetDisplayName()))
                .ForMember(dest => dest.UTCDateAdded, opts => opts.MapFrom(x => DateTime.UtcNow))
                .ForMember(dest => dest.IsCheapest, opts => opts.Ignore())
                ;
        }
    }
}
