using System;
using Broker.Service.Contracts;

namespace Broker.web.Configuration
{
    public class ProductionConfiguration : ISystemConfiguration
    {
        public Uri ServicesBaseUri 
        {
            get { return new Uri("http://findmycar.cloudapp.net"); }
        }

        public Uri CarFinderBaseUri
        {
            get { return new Uri("http://insureme.cloudapp.net"); }
        }
    }
}