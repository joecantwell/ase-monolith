
using Broker.Domain;

namespace Broker.Service.Contracts
{
    public interface IRestFactory
    {
        IRestGateway<T> CreateGateway<T>(EndPoint endpoint) where T : class;
    }
}
