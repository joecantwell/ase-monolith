
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.Service.Contracts;

namespace Broker.Services
{
    public sealed class RestGateway<T> : IRestGateway<T> where T : class
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Uri _baseUri;
        public RestGateway(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> Put(T objectToPut, string uri)
        {
            _logger.Trace("Put {0}{1}", _baseUri, uri);
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.PutAsJsonAsync(uri, objectToPut).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<HttpResponseMessage> Post(T objectToPost, string uri)
        {
            _logger.Trace("Post {0}{1}", _baseUri, uri);
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.PostAsJsonAsync(uri, objectToPost).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<T> Get(string uri)
        {
            _logger.Trace("Get {0}{1}", _baseUri, uri);
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.GetAsync(uri).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsAsync<T>().Result;
                return result;
            }
        }

        public async Task<HttpResponseMessage> Delete(string uri)
        {
            _logger.Trace("Delete {0}{1}", _baseUri, uri);
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.DeleteAsync(uri).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }
    }
}
