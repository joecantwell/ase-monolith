

using System;
using System.Threading.Tasks;
using Broker.Domain;
using Broker.Domain.Models;
using Broker.Domain.Queries;
using Broker.Service.Contracts;
using CarFinder.Api.Contracts;

namespace Broker.Services
{
    public class CarFinderService : ICarFinderService
    {
        private readonly IVehicleReader _vehicleReader;
        private readonly IRestFactory _restFactory;

        public CarFinderService(IVehicleReader vehicleReader,
                                IRestFactory restFactory)
        {
            _restFactory = restFactory;
            _vehicleReader = vehicleReader;
        }

        public async Task<VehicleDetailsDto> FindVehicleByRegistrationNo(string regNo)
        {
            // check if we have a local reference to the vehicle (it has already been queried from the 3rd party paid service
            var vehicle = _vehicleReader.GetVehicleByRegNo(regNo);

            if (vehicle != null)
                return vehicle;

            // didn't find anything. query the 3rd party service
            var gateway = _restFactory.CreateGateway<VehicleMetaData>(EndPoint.CarFinder);
            var uri = string.Format("/api/car/{0}", regNo);
            
            var response = await gateway.Get(uri);

            if (response == null)
                return null;

            return new VehicleDetailsDto
            {
                CurrentRegistration = response.CurrentRegistration,
                Colour = response.Colour,
                IsImport = response.IsImport,
                ManufYear = response.ManufYear,
                BodyType = response.BodyType,
                FuelType = response.FuelType,
                ModelDesc = response.VehicleDesc,
                ModelName = string.Format("{0} {1}", response.Make, response.Model),
                Transmission = response.Transmission
            };

        }
    }
}
