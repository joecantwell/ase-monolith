using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Broker.Domain.Queries;
using Broker.web.Models;

namespace Broker.web.ModelBuilders
{
    public interface ICarInsuranceModelBuilder
    {
        CarInsuranceViewModel BuildCarQuoteView();
    }

    public class CarInsuranceModelBuilder : ICarInsuranceModelBuilder
    {
        private readonly ICountyReader _countyReader;

        public CarInsuranceModelBuilder(ICountyReader countyReader)
        {
            _countyReader = countyReader;
        }

        public CarInsuranceViewModel BuildCarQuoteView()
        {
            return new CarInsuranceViewModel
            {
                Counties = _countyReader.ListCounties().Select(x => new SelectListItem
                {
                    Text = x.CountyName,
                    Value = x.CountyId.ToString()
                })
            };
        }
    }
}