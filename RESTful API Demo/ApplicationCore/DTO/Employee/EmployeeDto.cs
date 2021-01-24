using Common.DataService;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApplicationCore.DTO.Employee
{
    // [DataContract]
    public class EmployeeDto : IDto
    {
        public virtual int EmployeeId { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Title { get; set; }
        public virtual string TitleOfCourtesy { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? HireDate { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string Extension { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string Notes { get; set; }
        public virtual int? ReportsTo { get; set; }
        public virtual string PhotoPath { get; set; }

        public object GetId()
        {
            return this.EmployeeId;
        }
    }
}
