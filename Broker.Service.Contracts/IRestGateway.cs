

using System.Net.Http;
using System.Threading.Tasks;

namespace Broker.Service.Contracts
{
    public interface IRestGateway<T>
    {
        Task<HttpResponseMessage> Put(T objectToPut, string uri);

        Task<HttpResponseMessage> Post(T objectToPost, string uri);

        Task<T> Get(string uri);

        Task<HttpResponseMessage> Delete(string uri);
    }
}
