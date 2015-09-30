
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Broker.Domain.Models;
using Newtonsoft.Json;

namespace Broker.web.Models
{
    public class CarInsuranceViewModel
    {
        public IEnumerable<SelectListItem> Counties { get; set; }

        public IEnumerable<SelectListItem> NoClaimsBonusList { get; set; }

        public int CountyId { get; set; }

        public string JsonVehicle { get; set; }

        public VehicleDetailsDto Vehicle
        {
            get { return JsonVehicle != string.Empty ? 
                JsonConvert.DeserializeObject<VehicleDetailsDto>(JsonVehicle) : 
                new VehicleDetailsDto(); }
        }

        [Required]
        public int NoClaimsDiscount { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required]
        public string CarValue { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
    }
}