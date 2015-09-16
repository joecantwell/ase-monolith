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
using Broker.Domain.AutoMappings;

namespace Broker.Domain
{
    public static class AutoMapperDomainConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => x.AddProfile(new InsuranceMappings()));
        }
    }
}
