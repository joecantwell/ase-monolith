
using System;

namespace Broker.Service.Contracts
{
    public interface ISystemConfiguration
    {
        Uri ServicesBaseUri { get; }

        Uri CarFinderBaseUri { get; }
    }
}
