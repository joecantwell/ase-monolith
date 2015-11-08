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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ActorUI.Actors.Messages;
using Akka.Actor;
using AutoMapper;
using Broker.Domain.Models;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors
{
    /// <summary>
    /// THis Actor will be spawned from the InsuranceQuoteActor when it wants to collate 
    /// quotes from all available insurance companies
    /// </summary>
    public class InsuranceServiceActor : ReceiveActor
    {
        public InsuranceServiceActor()
        {
            Ready();
        }

        private void Ready()
        {
            Receive<GetQuotesFromService>(req =>
            {
                var senderClosure = Sender;
                var client = new HttpClient { BaseAddress = req.ServiceLocation };
                
                client.PostAsJsonAsync("api/carinsurancequote", req.InsuranceRequest).ContinueWith(httpRequest =>
                {
                    var response = httpRequest.Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var quotes = response.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();

                        return Mapper.Map<IEnumerable<CarQuoteResponseDto>>(quotes);
                    }

                    return null;

                }, TaskContinuationOptions.AttachedToParent &
                    TaskContinuationOptions.ExecuteSynchronously)
                    .PipeTo(senderClosure);    
            });
        }
    }
}
