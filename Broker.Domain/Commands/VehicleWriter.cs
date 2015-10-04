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
    public interface IVehicleWriter
    {
        Task<int> AddVehicle(VehicleDetailsDto vehicle);
    }

    public class VehicleWriter : IVehicleWriter
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Entities _context;

        public VehicleWriter(Entities context)
        {
            _context = context;
        }

        public async Task<int> AddVehicle(VehicleDetailsDto vehicle)
        {
            _logger.Trace("Adding Vehicle to local Cache - {0}", vehicle.CurrentRegistration);

            var mappedVehicle = Mapper.Map<VehicleDetail>(vehicle);
            mappedVehicle.UTCDateAdded = DateTime.UtcNow;

            _context.VehicleDetails.Add(mappedVehicle);
            await _context.SaveChangesAsync();

            return mappedVehicle.VehicleId;
        }
    }
}
