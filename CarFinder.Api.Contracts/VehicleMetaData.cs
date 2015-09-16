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
using System.Runtime.Serialization;

namespace CarFinder.Api.Contracts
{
    [DataContract]
    public class VehicleMetaData
    {
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
        public string NoDoors { get; set; }

        [DataMember]
        public int EngineSizeCC { get; set; }

        [DataMember]
        public string CurrentRegistration { get; set; }

        [DataMember]
        public string Colour { get; set; }

        [DataMember]
        public bool? IsImport { get; set; }

    }
}
