
using System;

namespace Broker.Domain.Models
{
    public class VehicleDetailsDto
    {
        public int VehicleId { get; set; }

        public string ModelName { get; set; }

        public string ModelDesc { get; set; }

        public int? ManufYear { get; set; }

        public string CurrentRegistration { get; set; }

        public string Colour { get; set; }

        public string BodyType { get; set; }

        public string FuelType { get; set; }

        public string Transmission { get; set; }

        public bool? IsImport { get; set; }

        public DateTime? UTCDateAdded { get; set; }
    }
}
