using System;
using Broker.Service.Contracts;

namespace BrokerUI.web.Configuration
{
    public class DebugConfiguration : ISystemConfiguration
    {
        public Uri ServicesBaseUri
        {
            get { return new Uri("http://localhost:8088"); }
        }

        public Uri CarFinderBaseUri
        {
            get { return new Uri("http://localhost:8089"); }
        }
    }
}