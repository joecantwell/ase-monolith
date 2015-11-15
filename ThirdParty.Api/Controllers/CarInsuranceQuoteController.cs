using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Thirdparty.Api.Contracts;
using ThirdParty.Api.Services;

namespace ThirdParty.Api.Controllers
{
    public class CarInsuranceQuoteController : ApiController
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Controller is Accessible");
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(ServiceCarInsuranceQuoteRequest request)
        {
            try
            {
                ICreateQuoteService quoteService = new CreateQuoteService();

                _logger.Debug(JsonConvert.SerializeObject(request));

                var quotesReturned = await Task.FromResult<List<ServiceCarInsuranceQuoteResponse>>(quoteService.CreateQuotes(request));

                _logger.Debug(JsonConvert.SerializeObject(quotesReturned));

                return Request.CreateResponse(HttpStatusCode.OK, quotesReturned);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
