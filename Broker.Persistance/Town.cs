//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Broker.Persistance
{
    using System;
    using System.Collections.Generic;
    
    public partial class Town
    {
        public int TownId { get; set; }
        public string TownName { get; set; }
        public int CountyId { get; set; }
        public Nullable<System.DateTime> UTCDateAdded { get; set; }
    }
}
