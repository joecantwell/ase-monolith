
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Broker.Service.Contracts;
using BrokerUI.web.ModelBuilders;
using BrokerUI.web.Models;

namespace BrokerUI.web.Controllers
{
    public class CarInsuranceController : Controller
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICarInsuranceModelBuilder _carInsuranceModelBuilder;
        private readonly ICarFinderService _carFinderService;
        private readonly ICarQuoteService _carQuoteService;

        public CarInsuranceController(ICarInsuranceModelBuilder carInsuranceModelBuilder, 
                                        ICarFinderService carFinderService,
                                        ICarQuoteService carQuoteService)
        {
            _carQuoteService = carQuoteService;
            _carFinderService = carFinderService;
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
            int quoteId= 0;
            try
            {
                decimal value;
                decimal.TryParse(model.CarValue, out value);

                model.CarQuoteRequest.VehicleValue = value;
                model.CarQuoteRequest.VehicleId = model.Vehicle.VehicleId;
                quoteId = await _carQuoteService.AddQuotes(model.CarQuoteRequest, model.Vehicle);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }

            return RedirectToAction("Details", new {id = quoteId});
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            // get the quotes from the db
            var quotes = await _carQuoteService.ListQuotes(id);

            return View(new QuotesReturnedViewModel{Quotes = quotes});
        }

        [HttpGet]
        public async Task<JsonResult> CheckCar(string regNo)
        {
            try
            {
                _logger.Trace("Query Service for Reg No {0}", regNo);

                var car = await _carFinderService.FindVehicleByRegistrationNo(regNo);

                return Json(car, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { id = -1, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}