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
using System.Linq;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Queries
{
    public interface ICountyReader
    {
        IEnumerable<CountyDto> ListCounties();
    }

    public class CountyReader : ICountyReader
    {
        private readonly Entities _context;

        public CountyReader(Entities context)
        {
            _context = context;
        }

        public IEnumerable<CountyDto> ListCounties()
        {
            return Mapper.Map<IEnumerable<CountyDto>>(_context.Counties.AsEnumerable());
        }
    }
}
