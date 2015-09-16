using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Domain;

namespace Broker.web.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapperDomainConfig.Configure();
        }
    }
}