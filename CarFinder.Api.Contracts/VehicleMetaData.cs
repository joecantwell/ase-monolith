

using System;
using System.Runtime.Serialization;

namespace CarFinder.Api.Contracts
{
    [DataContract]
    public class VehicleMetaData
    {
        [DataMember]
        public Guid VehicleRef { get; set; }

        [DataMember]
        public string VehicleDesc { get; set; }

        [DataMember]
        public int ManufYear { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public string BodyType { get; set; }

        [DataMember]
        public string FuelType { get; set; }

        [DataMember]
        public string Transmission { get; set; }

        [DataMember]
        public int NoDoors { get; set; }

        [DataMember]
        public int EngineSizeCC { get; set; }

        [DataMember]
        public string CurrentRegistration { get; set; }

        [DataMember]
        public string Colour { get; set; }

        [DataMember]
        public bool? IsImport { get; set; }

        [DataMember]
        public int BreakHorsePower { get; set; }

    }
}
