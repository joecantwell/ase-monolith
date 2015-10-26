using System;
using Broker.Service.Contracts;

namespace BrokerUI.web.Configuration
{
    public class ProductionConfiguration : ISystemConfiguration
    {
        public Uri ServicesBaseUri 
        {
            get { return new Uri("http://insureme.cloudapp.net"); }
        }

        public Uri CarFinderBaseUri
        {
            get { return new Uri("http://findmycar.cloudapp.net"); }
        }
    }
}