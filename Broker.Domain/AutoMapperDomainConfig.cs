
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
