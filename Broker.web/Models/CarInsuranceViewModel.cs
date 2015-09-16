using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Broker.Domain.Models;

namespace Broker.web.Models
{
    public class CarInsuranceViewModel
    {
        public IEnumerable<SelectListItem> Counties { get; set; }

        public int CountyId { get; set; }

        public int NoClaimsDiscount { get; set; }

        public int Age { get; set; }

        public string EmailAddress { get; set; }

        public string Telephone { get; set; }
    }
}