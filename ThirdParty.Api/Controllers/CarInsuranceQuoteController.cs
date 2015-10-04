using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Thirdparty.Api.Contracts;

namespace ThirdParty.Api.Controllers
{
    public class CarInsuranceQuoteController : ApiController
    {
        public CarInsuranceQuoteController()
        {
            
        }


        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Controller is Accessible");
        }

        public HttpResponseMessage Post(ServiceCarInsuranceQuoteRequest request)
        {
            try
            {
                var returnedQuotes = new List<ServiceCarInsuranceQuoteResponse>
                {
                    new ServiceCarInsuranceQuoteResponse {
                        QuoteId = request.QuoteRequestId,
                        QuoteType = QuoteType.Comprehensive,
                        InsuranceCompany = "Alliance Direct",
                        QuoteDescription = "Comprehensive Insurance",
                        QuoteValue = 500
                    },
                    new ServiceCarInsuranceQuoteResponse {
                        QuoteId = request.QuoteRequestId,
                        QuoteType = QuoteType.ThirdPartyFireAndTheft,
                        InsuranceCompany = "Zurich Car Insurance",
                        QuoteDescription = "Third Party Fire and Theft Insurance",
                        QuoteValue = 450
                    },
                    new ServiceCarInsuranceQuoteResponse {
                        QuoteId = request.QuoteRequestId,
                        QuoteType = QuoteType.ThirdPartyFireAndTheft,
                        InsuranceCompany = "AXA Car Insurance",
                        QuoteDescription = "Third Party Insurance",
                        QuoteValue = 475
                    }
                };

                return Request.CreateResponse(HttpStatusCode.OK, returnedQuotes);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
