﻿
using System;
using System.Collections.Generic;
using Broker.Domain.Models;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors.Messages
{
    /// <summary>
    /// Message for initial call to the start the quote process
    /// </summary>
    public class RequestQuotes
    {
        public RequestQuotes(CarQuoteRequestDto request, VehicleDetailsDto vehicle, Uri serviceLocation)
        {
            QuoteRequest = request;
            VehicleDetails = vehicle;
            ServiceLocation = serviceLocation;
        }

        public CarQuoteRequestDto QuoteRequest { get; private set; }
        public VehicleDetailsDto VehicleDetails { get; private set; }
        public Uri ServiceLocation { get; private set; }

    }

    /// <summary>
    /// passed to Quote Coordinators workers
    /// </summary>
    public class GetQuotesFromService
    {
        public GetQuotesFromService(Uri serviceLocation, ServiceCarInsuranceQuoteRequest insuranceRequest)
        {
            ServiceLocation = serviceLocation;
            InsuranceRequest = insuranceRequest;
        }

         public Uri ServiceLocation { get; private set; }
         public ServiceCarInsuranceQuoteRequest InsuranceRequest { get; private set; }
    }

    /// <summary>
    /// collection passed back from worker services
    /// </summary>
    public class QuotesReturnedFromService
    {
        public QuotesReturnedFromService(List<CarQuoteResponseDto> quotesFromService)
        {
            QuotesFromService = quotesFromService;
        }

        public IReadOnlyList<CarQuoteResponseDto> QuotesFromService;
    }

    /// <summary>
    /// flag to determine if the quotes are returned and saved
    /// </summary>
    public class IsLoadComplete { }

    /// <summary>
    /// bool to trigger a db write rather than wait for all services to return
    /// </summary>
    public class TimedOutOrComplete
    {
        public TimedOutOrComplete(bool isTimedOut = false, bool isComplete = false)
        {
            IsTimedOut = isTimedOut;
            IsComplete = isComplete;
        }

        public bool IsTimedOut { get; private set; }
        public bool IsComplete{ get; private set; }
    }
   
    /// <summary>
    /// message to query the database for all quotes for the quoteId
    /// </summary>
    public class ListQuotes
    {
        public ListQuotes(int quoteId)
        {
            QuoteId = quoteId;
        }

        public int QuoteId { get; private set; }
    }
}
