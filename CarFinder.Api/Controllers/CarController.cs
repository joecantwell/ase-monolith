using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarFinder.Api.Services;

namespace CarFinder.Api.Controllers
{
    public class CarController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            try
            {
                var vehicle = CarRegistrationService.GetVehicleByRegistration(id);
                
                return Request.CreateResponse(HttpStatusCode.Created, vehicle);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            
        }
    }
}
