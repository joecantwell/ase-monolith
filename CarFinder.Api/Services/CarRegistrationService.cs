using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using CarFinder.Api.Contracts;

namespace CarFinder.Api.Services
{
    public static class CarRegistrationService
    {
        /// <summary>
        /// bit of scaffolding here. it will wait 5 seconds and return back a default regardless of which reg no is passed in.
        /// </summary>
        /// <param name="carReg"></param>
        /// <returns></returns>
        public static VehicleMetaData GetVehicleByRegistration(string carReg)
        {
            Thread.Sleep(new TimeSpan(0,0,0,2)); // 2 second sleep

            return new VehicleMetaData
            {
                VehicleRef = Guid.NewGuid(),
                CurrentRegistration = carReg,
                BodyType = "Estate",
                Colour = "Blue",
                FuelType = "Diesel",
                EngineSizeCC = 1980,
                IsImport = true,
                Make = "VolksWagen",
                Model = "Passat",
                BreakHorsePower = 103,
                ManufYear = 2008,
                NoDoors = 5,
                Transmission = "Manual",
                VehicleDesc = "2008 Volkswagen Passat 1.9 TDI Bluemotion 103 BHP 5 DR"
            };
        }
    }
}