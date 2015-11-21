using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CarFinder.Api.Contracts;
using CarFinder.Api.Services;

namespace CarFinder.Api.Controllers
{
    public class CarController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Get()
        {         
            return Request.CreateResponse(HttpStatusCode.OK, "Controller is Accessible");
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Entry!");

                var vehicle = await Task.FromResult<VehicleMetaData>(CarRegistrationService.GetVehicleByRegistration(id));

                return Request.CreateResponse<VehicleMetaData>(HttpStatusCode.OK, vehicle);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            } 
        }
    }
}
