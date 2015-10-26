
using System.Collections.Generic;
using Broker.Domain.Models;

namespace BrokerUI.web.Models
{
    public class QuotesReturnedViewModel
    {
        public IEnumerable<CarQuoteResponseDto> Quotes { get; set; } 
    }
}