// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.AutoMappings
{
    public class InsuranceMappings : Profile
    {
        protected override void Configure()
        {
            DomainToContractMaps();
            ContractToDomainMaps();
        }

        public void DomainToContractMaps()
        {
            CreateMap<County, CountyDto>()
                ;

            CreateMap<VehicleDetail, VehicleDetailsDto>()
                ;

            CreateMap<CarInsuranceQuoteRequest, CarQuoteRequestDto>()
                .ForMember(dest => dest.VehicleRef, opts => opts.Ignore())
                ;

            CreateMap<CarInsuranceQuoteResponse, CarQuoteResponseDto>()
                ;
        }

        public void ContractToDomainMaps()
        {
            CreateMap<CountyDto, County>()
                .ForMember(dest => dest.CarInsuranceQuoteRequests, opts => opts.Ignore())
                ;

            CreateMap<VehicleDetailsDto, VehicleDetail>()
                .ForMember(dest => dest.CarInsuranceQuoteRequests, opts => opts.Ignore())
                ;

            CreateMap<CarQuoteRequestDto, CarInsuranceQuoteRequest>()
                .ForMember(dest => dest.County, opts => opts.Ignore())
                .ForMember(dest => dest.VehicleDetail, opts => opts.Ignore())
                .ForMember(dest => dest.CarInsuranceQuoteResponses, opts => opts.Ignore())
                ;

            CreateMap<CarQuoteResponseDto, CarInsuranceQuoteResponse>()
                .ForMember(dest => dest.CarInsuranceQuoteRequest, opts => opts.Ignore());
        }
    }
}
