
using Broker.Domain;

namespace ActorUI.Web.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapperDomainConfig.Configure();
        }
    }
}