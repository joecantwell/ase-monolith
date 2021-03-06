﻿// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Queries
{
    public interface IVehicleReader
    {
        Task<VehicleDetailsDto> GetVehicleByRegNo(string regNo);
    }

    public class VehicleReader : IVehicleReader
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Entities _context;

        public VehicleReader(Entities context)
        {
            _context = context;
        }

        public async Task<VehicleDetailsDto> GetVehicleByRegNo(string regNo)
        {
            var vehicle = await _context.VehicleDetails.FirstOrDefaultAsync(x => x.CurrentRegistration == regNo);

            return Mapper.Map<VehicleDetailsDto>(vehicle);
        }
    }
}
