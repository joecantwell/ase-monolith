// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.Service.Contracts;

namespace Broker.Services
{
    public sealed class RestGateway<T> : IRestGateway<T> where T : class
    {
        private readonly Uri _baseUri;
        public RestGateway(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> Put(T objectToPut, string uri)
        {
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.PutAsJsonAsync(uri, objectToPut).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<HttpResponseMessage> Post(T objectToPost, string uri)
        {
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.PostAsJsonAsync(uri, objectToPost).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<T> Get(string uri)
        {
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
            using (var client = new HttpClient { BaseAddress = _baseUri })
            {
                var response = await client.DeleteAsync(uri).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }
    }
}
