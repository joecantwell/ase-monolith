using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarFinder.Api.Controllers
{
    public class CarController : ApiController
    {
        public CarController()
        {
            
        }

        public HttpResponseMessage Get(string id)
        {
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
