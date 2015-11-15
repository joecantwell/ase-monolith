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
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Commands
{
    public interface ICarQuoteResponseWriter
    {
        Task<bool> AddResponse(IEnumerable<CarQuoteResponseDto> response);
    }

    public class CarQuoteResponseWriter : ICarQuoteResponseWriter
    {
        private readonly Entities _context;

        public CarQuoteResponseWriter(Entities context)
        {
            _context = context;
        }


        public async Task<bool> AddResponse(IEnumerable<CarQuoteResponseDto> response)
        {
            var mapppedObjects = Mapper.Map<IEnumerable<CarInsuranceQuoteResponse>>(response);
           
            _context.CarInsuranceQuoteResponses.AddRange(mapppedObjects);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
