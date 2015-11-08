using System;

namespace ActorUI.web.Configuration
{
    public interface ISystemConfiguration
    {
        Uri ServicesBaseUri { get; }
        Uri CarFinderBaseUri { get; }
    }

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