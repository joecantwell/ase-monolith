
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Broker.Domain.Models;
using Newtonsoft.Json;

namespace BrokerUI.web.Models
{
    public class CarInsuranceViewModel
    {
        public IEnumerable<SelectListItem> Counties { get; set; }

        public IEnumerable<SelectListItem> NoClaimsBonusList { get; set; }

        public CarQuoteRequestDto CarQuoteRequest { get; set; }

        public string JsonVehicle { get; set; }

        public VehicleDetailsDto Vehicle
        {
            get { return JsonVehicle != string.Empty ? 
                JsonConvert.DeserializeObject<VehicleDetailsDto>(JsonVehicle) : 
                new VehicleDetailsDto(); }
        }     

        [Required]
        public string CarValue { get; set; }
    }
}