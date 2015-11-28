using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
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
            return CalculateQuote(request)
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
        /// <param name="request"></param>
        /// <returns></returns>
        private Dictionary<QuoteType, decimal> CalculateQuote(ServiceCarInsuranceQuoteRequest request)
        {
            Thread.Sleep(2000); // add a 2 second delay

            var quotePrices = new Dictionary<QuoteType, decimal>();
            switch (request.Insurer)
            {
                case Insurer.AllianceDirect:
                    quotePrices.Add(QuoteType.Comprehensive, BuildQuote((decimal)400, request));
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, BuildQuote((decimal)370, request));
                    quotePrices.Add(QuoteType.ThirdParty, BuildQuote((decimal)310, request));
                    break;
                case Insurer.AxaCar:
                    // Add a further 6 second delay for the Axa Service (Test)
                    Thread.Sleep(6000);
                    quotePrices.Add(QuoteType.Comprehensive, BuildQuote((decimal)420, request));
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, BuildQuote((decimal)350, request));
                    quotePrices.Add(QuoteType.ThirdParty, BuildQuote((decimal)300, request));
                    break;
                case Insurer.ZurichCar:
                    quotePrices.Add(QuoteType.Comprehensive, BuildQuote((decimal)380, request));
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, BuildQuote((decimal)360, request));
                    quotePrices.Add(QuoteType.ThirdParty, BuildQuote((decimal)320, request));
                    break;
                case Insurer.OneTwoThree:
                    quotePrices.Add(QuoteType.Comprehensive, BuildQuote((decimal)500, request));
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, BuildQuote((decimal)407, request));
                    quotePrices.Add(QuoteType.ThirdParty, BuildQuote((decimal)245, request));
                    break;
                case Insurer.Fbd:
                    quotePrices.Add(QuoteType.Comprehensive, BuildQuote((decimal)430, request));
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, BuildQuote((decimal)390, request));
                    quotePrices.Add(QuoteType.ThirdParty, BuildQuote((decimal)350, request));
                    break;
                default:
                    quotePrices.Add(QuoteType.Comprehensive, (decimal)0);
                    quotePrices.Add(QuoteType.ThirdPartyFireAndTheft, (decimal)0);
                    quotePrices.Add(QuoteType.ThirdParty, (decimal)0);
                    break;
            }

            return quotePrices;

        }

        private decimal BuildQuote(decimal baseQuote, ServiceCarInsuranceQuoteRequest request)
        {
            var riskPremium = baseQuote + VehicleAgePremium(request.ManufYear) + DriverHighRiskPremium(request.DriverAge) + VehicleValuePremium(request.ManufYear);
            
            return HighRiskOfTheftPremium(riskPremium, request.County);

        }

        private decimal VehicleAgePremium(int manufYear)
        {
            return manufYear < DateTime.Now.AddYears(-10).Year ? 200 : 0;
        }

        private decimal HighRiskOfTheftPremium(decimal premium, string county)
        {
            var highRiskAreas = new[] {"Dublin", "Cork", "Limerick", "Galway", "Meath", "Kildare"};

            // increase by 10% if in high risk area
            return highRiskAreas.Contains(county) ? (premium/(decimal)10) + premium : premium;
        }

        private decimal DriverHighRiskPremium(int driverAge)
        {
            if (driverAge <= 23)
            {
                // v high risk
                return 1000;
            }
            else if (driverAge >= 70)
            {
                // moderate risk
                return 350;
            }
           
            // low risk
            return 0;
            
        }

        private decimal VehicleValuePremium(decimal vehicleValue)
        {
            if (vehicleValue > 50000)
            {
                return 150;
            } 

            if (vehicleValue > 40000)
            {
                return 100;
            } 
            
            if (vehicleValue > 10000)
            {
                return 80;
            }
            
            return vehicleValue > 50000 ? 50 : 100; // cars less than 5k are more prone to accident/claims
        }
    }
}