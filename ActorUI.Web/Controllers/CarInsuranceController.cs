using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ActorUI.Actors;
using ActorUI.Actors.Messages;
using ActorUI.web.Configuration;
using ActorUI.web.ModelBuilders;
using ActorUI.web.Models;
using Akka.Actor;
using Broker.Domain.Models;

namespace ActorUI.Web.Controllers
{
    public class CarInsuranceController : Controller
    {

        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICarInsuranceModelBuilder _carInsuranceModelBuilder;
        private readonly ISystemConfiguration _systemConfiguration;


        public CarInsuranceController(ICarInsuranceModelBuilder carInsuranceModelBuilder, 
                                        ISystemConfiguration systemConfiguration)
        {
            _systemConfiguration = systemConfiguration;
            _carInsuranceModelBuilder = carInsuranceModelBuilder;
        }

        // GET: /CarInsurance/
        [HttpGet]
        public ActionResult Index()
        {
            var model = _carInsuranceModelBuilder.BuildCarQuoteView();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CarInsuranceViewModel model)
        {
            try
            {
                decimal value;
                decimal.TryParse(model.CarValue, out value);

                model.CarQuoteRequest.VehicleValue = value;
                model.CarQuoteRequest.VehicleRef = model.Vehicle.VehicleRef;

                model.CarQuoteRequest.CarQuoteId = await SystemActors.QuoteActor.Ask<int>(new SaveQuoteRequest(model.CarQuoteRequest));

                // populate the database
                SystemActors.QuoteActor.Tell(new CollateQuotes(model.CarQuoteRequest, model.Vehicle, _systemConfiguration.ServicesBaseUri));

            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }

            return RedirectToAction("Details", new { id = model.CarQuoteRequest.CarQuoteId });
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            // get the quotes from the db
            var quotes = await SystemActors.QuoteActor.Ask<IEnumerable<CarQuoteResponseDto>>(new ListQuotes(id));
          
            return View(new QuotesReturnedViewModel{Quotes = quotes});
        }

        [HttpGet]
        public async Task<JsonResult> CheckCar(string regNo)
        {
            try
            {
                _logger.Trace("Find Car Registration {0}", regNo);
                VehicleDetailsDto car = null;

                // check local storage
                car = await SystemActors.VehicleActor.Ask<VehicleDetailsDto>(new FindCarFromLocalStorage(regNo));
                if (car == null)
                {
                    car = await SystemActors.VehicleActor.Ask<VehicleDetailsDto>(new FindCarFromService(regNo, _systemConfiguration.CarFinderBaseUri));
                    // async write to the Db fire & froget
                    SystemActors.VehicleActor.Tell(new SaveVehicleDetails(car)); 
                }
                   
                return Json(car, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { id = -1, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}