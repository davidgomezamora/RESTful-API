using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Common.DTO.Employee
{
    // [DataContract]
    public class EmployeeDto
    {
        /*private readonly IDataSecurity _dataSecurity;
        public EmployeeDto(IDataSecurity dataSecurity)
        {
            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));
        }*/
        public string EmployeeId { get; set; }

        /*[JsonIgnore]
        [XmlIgnore]
        public string EmployeeId {
            get { return this.Id; }
            set { this.Id = value; }
        }

        public string Id { get; set; }*/
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public byte[] Photo { get; set; }
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string PhotoPath { get; set; }
    }
}
