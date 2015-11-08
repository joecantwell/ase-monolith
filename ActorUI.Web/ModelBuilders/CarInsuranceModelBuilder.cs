using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActorUI.web.Models;
using Broker.Domain.Models;
using Broker.Domain.Queries;

namespace ActorUI.web.ModelBuilders
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
            var ncbDict = new Dictionary<int, string>
            {
                {0, "No Discount"},
                {1, "1 Year"},
                {2, "2 Years"},
                {3, "3 Years"},
                {4, "4 Years"},
                {5, "5+ Years"}
            };

            return new CarInsuranceViewModel
            {
                Counties = _countyReader.ListCounties().Select(x => new SelectListItem
                {
                    Text = x.CountyName,
                    Value = x.CountyId.ToString()
                }),
                NoClaimsBonusList = ncbDict.Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString()
                }),
                CarQuoteRequest = new CarQuoteRequestDto()
            };
        }
    }
}