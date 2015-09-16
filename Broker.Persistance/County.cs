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
    
    public partial class County
    {
        public County()
        {
            this.CarInsuranceQuoteRequests = new HashSet<CarInsuranceQuoteRequest>();
            this.HomeInsuranceQuoteRequests = new HashSet<HomeInsuranceQuoteRequest>();
        }
    
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public Nullable<System.DateTime> UTCDateAdded { get; set; }
    
        public virtual ICollection<CarInsuranceQuoteRequest> CarInsuranceQuoteRequests { get; set; }
        public virtual ICollection<HomeInsuranceQuoteRequest> HomeInsuranceQuoteRequests { get; set; }
    }
}
