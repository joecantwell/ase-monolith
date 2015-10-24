using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Thirdparty.Api.Contracts;
using ThirdParty.Api.Services;

namespace ThirdParty.Api.Controllers
{
    public class CarInsuranceQuoteController : ApiController
    {
       
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Controller is Accessible");
        }

        public HttpResponseMessage Post(ServiceCarInsuranceQuoteRequest request)
        {
            try
            {
                ICreateQuoteService quoteService = new CreateQuoteService();

                var quotesReturned = quoteService.CreateQuotes(request);

                return Request.CreateResponse(HttpStatusCode.OK, quotesReturned);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
