
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Broker.Service.Contracts;
using Broker.web.ModelBuilders;
using Broker.web.Models;

namespace Broker.web.Controllers
{
    public class CarInsuranceController : Controller
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICarInsuranceModelBuilder _carInsuranceModelBuilder;
        private readonly ICarFinderService _carFinderService;

        public CarInsuranceController(ICarInsuranceModelBuilder carInsuranceModelBuilder, 
                                        ICarFinderService carFinderService)
        {
            _carFinderService = carFinderService;
            _carInsuranceModelBuilder = carInsuranceModelBuilder;
        }

        // GET: /CarInsurance/
        public ActionResult Index()
        {
            var model = _carInsuranceModelBuilder.BuildCarQuoteView();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CarInsuranceViewModel model)
        {
            try
            {
               
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> CheckCar(string id)
        {
            try
            {
                _logger.Trace("Query Service for Reg No {0}", id);

                var car = await _carFinderService.FindVehicleByRegistrationNo(id);

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