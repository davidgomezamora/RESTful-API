using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTO.Order
{
    public class OrderForManipulationDto
    {
        public virtual string CustomerId { get; set; }
        public virtual int? EmployeeId { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }
        public virtual int? ShipVia { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }
    }
}
