// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

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
