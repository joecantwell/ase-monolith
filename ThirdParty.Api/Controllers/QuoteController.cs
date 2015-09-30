using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ThirdParty.Api.Controllers
{
    public class QuoteController : ApiController
    {
        public QuoteController()
        {
            
        }

        public HttpResponseMessage Post()
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
