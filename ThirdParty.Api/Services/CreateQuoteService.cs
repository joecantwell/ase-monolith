using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thirdparty.Api.Contracts;

namespace ThirdParty.Api.Services
{
    public interface ICreateQuoteService
    {
        List<ServiceCarInsuranceQuoteResponse> CreateQuotes(ServiceCarInsuranceQuoteRequest request);
    }

    public class CreateQuoteService : ICreateQuoteService
    {
       
        public List<ServiceCarInsuranceQuoteResponse> CreateQuotes(ServiceCarInsuranceQuoteRequest request)
        {
            return CalculateQuote(request.Insurer)
                 .Select(x => new ServiceCarInsuranceQuoteResponse
                 {
                     QuoteId = request.QuoteRequestId,
                     QuoteType = x.Key,
                     InsuranceCompany = request.Insurer.GetDisplayName(),
                     QuoteDescription = x.Key.GetDisplayName(),
                     QuoteValue = x.Value
                 }).ToList();     
        }

        
        /// <summary>
        /// todo: build in some logic to determine a more random set of prices determined by the quote factors
        /// out of scope for now
        /// </summary>
        /// <param name="insurer"></param>
        /// <returns></returns>
        private Dictionary<QuoteType, decimal> CalculateQuote(Insurer insurer)
        {
            Thread.Sleep(2000); // add a 3 second delay

            var quotePrices = new Dictionary<QuoteType, decimal>();
            switch (insurer)
            {
                case Insurer.AllianceDirect:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)500);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)470);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)410);
                    break;
                case Insurer.AxaCar:
                     quotePrices.Add(QuoteType.Comprehensive, (decimal)520);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)450);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)400);
                    break;
                case Insurer.ZurichCar:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)480);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)460);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)420);
                    break;
                case Insurer.OneTwoThree:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)600);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)507);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)345);
                    break;
                case Insurer.Fbd:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)530);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)490);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)450);
                    break;
                default:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)0);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)0);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)0);
                    break;
            }

            return quotePrices;

        }
    }
}