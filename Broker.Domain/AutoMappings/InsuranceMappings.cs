// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Diagnostics.PerformanceData;
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
        }

        public void ContractToDomainMaps()
        {
            CreateMap<CountyDto, County>()
                .ForMember(dest => dest.CarInsuranceQuoteRequests, opts => opts.Ignore())
                .ForMember(dest => dest.HomeInsuranceQuoteRequests, opts => opts.Ignore())
                ;

            CreateMap<VehicleDetailsDto, VehicleDetail>()
                ;
        }
    }
}
