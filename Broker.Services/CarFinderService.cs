
using System.Threading.Tasks;
using Broker.Domain;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Domain.Queries;
using Broker.Service.Contracts;
using CarFinder.Api.Contracts;

namespace Broker.Services
{
    public class CarFinderService : ICarFinderService
    {
        private readonly static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IVehicleReader _vehicleReader;
        private readonly IRestFactory _restFactory;
        private readonly IVehicleWriter _vehicleWriter;

        public CarFinderService(IVehicleReader vehicleReader,
                                IVehicleWriter vehicleWriter,
                                IRestFactory restFactory)
        {
            _vehicleWriter = vehicleWriter;
            _restFactory = restFactory;
            _vehicleReader = vehicleReader;
        }

        public async Task<VehicleDetailsDto> FindVehicleByRegistrationNo(string regNo)
        {
            // check if we have a local reference to the vehicle (it has already been queried from the 3rd party paid service
            var vehicle = await _vehicleReader.GetVehicleByRegNo(regNo);

            if (vehicle != null)
                return vehicle;

            // didn't find anything. query the 3rd party service
            _logger.Trace("Querying Car Finder Service for reg {0}", regNo);

            var gateway = _restFactory.CreateGateway<VehicleMetaData>(EndPoint.CarFinder);
            var uri = string.Format("api/car/{0}", regNo);
            
            var response = await gateway.Get(uri);

            if (response == null)
                return null;

            var transformVehicle = new VehicleDetailsDto
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

            // add it to the local db to avoid further service querying.
            int vehicleId = await _vehicleWriter.AddVehicle(transformVehicle);
            transformVehicle.VehicleId = vehicleId;

            return transformVehicle;

        }
    }
}
