// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ActorUI.Actors.Messages;
using Akka.Actor;
using Akka.Event;
using AutoMapper;
using Broker.Domain.Models;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors
{
    /// <summary>
    /// THis Actor will be spawned from the InsuranceQuoteActor when it wants to collate 
    /// quotes from all available insurance companies
    /// </summary>
    public class QuoteServiceActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public QuoteServiceActor()
        {
            ReceiptListener();
        }

        private void ReceiptListener()
        {
            Receive<GetQuotesFromService>(req =>
            {
                var senderClosure = this.Sender;
                var client = new HttpClient { BaseAddress = req.ServiceLocation };

                client.PostAsJsonAsync("api/carinsurancequote", req.InsuranceRequest).ContinueWith(httpRequest =>
                {
                    var response = httpRequest.Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var quotes = response.Content.ReadAsAsync<IEnumerable<ServiceCarInsuranceQuoteResponse>>();

                        _log.Debug("result returned from quote service");

                        return new QuotesReturnedFromService(Mapper.Map<IEnumerable<CarQuoteResponseDto>>(quotes.Result).ToList());
                    }

                    return null;

                }).PipeTo(senderClosure);    
            });
        }
    }
}
