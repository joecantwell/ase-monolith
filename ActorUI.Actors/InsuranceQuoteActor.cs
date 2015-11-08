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
using ActorUI.Actors.Messages;
using Akka.Actor;
using Akka.Routing;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Domain.Queries;
using Broker.Persistance;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors
{
    public class InsuranceQuoteActor : ReceiveActor
    {
        private readonly Entities _context;
        private readonly IActorRef insuranceServices;

        public InsuranceQuoteActor()
        {
            _context = new Entities();

            int noInsurers = Enum.GetNames(typeof(Insurer)).Length; // get num of insurance services
            // create a child actor for each insurance service
            insuranceServices = Context.ActorOf(Props.Create(typeof(InsuranceServiceActor)).WithRouter(new RoundRobinPool(noInsurers)), "ServiceInterrogator");

            Ready();
        }

        private void Ready()
        {
            Receive<SaveQuoteRequest>(req =>
            {
                var senderClosure = Sender;
                ICarQuoteRequestWriter carQuoteRequestWriter = new CarQuoteRequestWriter(_context);

                carQuoteRequestWriter.AddQuote(req.QuoteRequest).ContinueWith(s => s.Result).PipeTo(senderClosure);
            });

            Receive<CollateQuotes>(req =>
            {
                var senderClosure = Sender;
                var self = Self;

                // build the object to post
                var serviceRequest = new ServiceCarInsuranceQuoteRequest
                {
                    QuoteRequestId = req.QuoteRequest.CarQuoteId,
                    NoClaimsDiscountYears = req.QuoteRequest.NoClaimsDiscountYears.HasValue ? req.QuoteRequest.NoClaimsDiscountYears.Value : 0,
                    VehicleValue = req.QuoteRequest.VehicleValue.HasValue ? req.QuoteRequest.VehicleValue.Value : 0,
                    CurrentRegistration = req.VehicleDetails.CurrentRegistration,
                    DriverAge = req.QuoteRequest.DriverAge.HasValue ? req.QuoteRequest.DriverAge.Value : 0,
                    ModelDesc = req.VehicleDetails.ModelDesc,
                    IsImport = req.VehicleDetails.IsImport,
                    ManufYear = req.VehicleDetails.ManufYear.HasValue ? req.VehicleDetails.ManufYear.Value : 0,
                };

                // push the service request to the children
                IEnumerable<CarQuoteResponseDto> allQuotes = new List<CarQuoteResponseDto>();
                foreach (var insurer in Enum.GetValues(typeof(Insurer)))
                {
                    serviceRequest.Insurer = (Insurer)insurer;
                    var quotes = insuranceServices.Ask<IEnumerable<CarQuoteResponseDto>>(new GetQuotesFromService(req.ServiceLocation, serviceRequest)).Result;
                    allQuotes = allQuotes.Concat(quotes); 
                }

            });

            Receive<ListQuotes>(req =>
            {
                var senderClosure = Sender;
                ICarQuoteResponseReader carQuoteResponseReader = new CarQuoteResponseReader(_context);

                carQuoteResponseReader.GetQuoteResponses(req.QuoteId).ContinueWith(s => s.Result).PipeTo(senderClosure);
            });
        }
    }
}
