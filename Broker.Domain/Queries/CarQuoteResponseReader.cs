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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Queries
{
    public interface ICarQuoteResponseReader
    {
        Task<IEnumerable<CarQuoteResponseDto>> GetQuoteResponses(int quoteId);
    }

    public class CarQuoteResponseReader : ICarQuoteResponseReader
    {
        private readonly Entities _context;

        public CarQuoteResponseReader(Entities context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarQuoteResponseDto>> GetQuoteResponses(int quoteId)
        {
            var quoteResponses = await _context.CarInsuranceQuoteResponses.Where(x => x.CarQuoteId == quoteId).ToListAsync();

            return Mapper.Map<IEnumerable<CarQuoteResponseDto>>(quoteResponses);
        }
    }
}
