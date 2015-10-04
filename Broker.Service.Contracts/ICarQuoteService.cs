// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Broker.Domain.Models;

namespace Broker.Service.Contracts
{
    public interface ICarQuoteService
    {
        Task<int> AddQuotes(CarQuoteRequestDto request, VehicleDetailsDto vehicle);

        Task<IEnumerable<CarQuoteResponseDto>> ListQuotes(int quoteId);
    }
}
