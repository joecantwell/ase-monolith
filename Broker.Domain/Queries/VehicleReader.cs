// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Linq;
using AutoMapper;
using Broker.Domain.Models;
using Broker.Persistance;

namespace Broker.Domain.Queries
{
    public interface IVehicleReader
    {
        VehicleDetailsDto GetVehicleByRegNo(string regNo);
    }

    public class VehicleReader : IVehicleReader
    {
        private readonly Entities _context;

        public VehicleReader(Entities context)
        {
            _context = context;
        }

        public VehicleDetailsDto GetVehicleByRegNo(string regNo)
        {
            return Mapper.Map<VehicleDetailsDto>(_context.VehicleDetails.Where(x => x.CurrentRegistration.Equals(regNo)));
        }
    }
}
