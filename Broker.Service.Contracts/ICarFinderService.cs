// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System.Threading.Tasks;
using Broker.Domain.Models;

namespace Broker.Service.Contracts
{
    public interface ICarFinderService
    {
        Task<VehicleDetailsDto> FindVehicleByRegistrationNo(string regNo);
    }
}
