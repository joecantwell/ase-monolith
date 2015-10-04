// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Commands
{
    public interface ICarQuoteRequestWriter
    {
        Task<int> AddQuote(CarQuoteRequestDto request);
    }

    public class CarQuoteRequestWriter : ICarQuoteRequestWriter
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Entities _context;

        public CarQuoteRequestWriter(Entities context)
        {
            _context = context;
        }

        public async Task<int> AddQuote(CarQuoteRequestDto request)
        {
            _logger.Trace("Persisting Quote prior to external query");

            var mappedQuote = Mapper.Map<CarInsuranceQuoteRequest>(request);
            mappedQuote.UTCDateAdded = DateTime.UtcNow;

            _context.CarInsuranceQuoteRequests.Add(mappedQuote);
            await _context.SaveChangesAsync();

            return mappedQuote.CarQuoteId;
        }
    }
}
