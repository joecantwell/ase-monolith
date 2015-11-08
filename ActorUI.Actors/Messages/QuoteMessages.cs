
using System;
using Broker.Domain.Models;
using Thirdparty.Api.Contracts;

namespace ActorUI.Actors.Messages
{
    public class SaveQuoteRequest
    {
        public SaveQuoteRequest(CarQuoteRequestDto quoteRequest)
        {
            QuoteRequest = quoteRequest;
        }

        public CarQuoteRequestDto QuoteRequest { get; private set; }
    }

    public class CollateQuotes
    {
        public CollateQuotes(CarQuoteRequestDto request, VehicleDetailsDto vehicle, Uri serviceLocation)
        {
            QuoteRequest = request;
            VehicleDetails = vehicle;
            ServiceLocation = serviceLocation;
        }

        public CarQuoteRequestDto QuoteRequest { get; private set; }
        public VehicleDetailsDto VehicleDetails { get; private set; }
        public Uri ServiceLocation { get; private set; }

    }

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

   

    public class ListQuotes
    {
        public ListQuotes(int quoteId)
        {
            QuoteId = quoteId;
        }

        public int QuoteId { get; private set; }
    }
}
