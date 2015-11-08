
using System;
using Broker.Domain.Models;

namespace ActorUI.Actors.Messages
{
    public class FindCarFromService
    {
        public FindCarFromService(string carRegNo, Uri serviceLocation)
        {
            CarRegNo = carRegNo;
            ServiceLocation = serviceLocation;
        }

        public string CarRegNo { get; private set; }
        public Uri ServiceLocation { get; private set; }
    }

    public class FindCarFromLocalStorage
    {
        public FindCarFromLocalStorage(string carRegNo)
        {
            CarRegNo = carRegNo;
        }

        public string CarRegNo { get; private set; }
    }

    public class SaveVehicleDetails
    {
        public SaveVehicleDetails(VehicleDetailsDto vehicle)
        {
            this.Vehicle = vehicle;
        }

        public VehicleDetailsDto Vehicle { get; private set; }
    }
}
