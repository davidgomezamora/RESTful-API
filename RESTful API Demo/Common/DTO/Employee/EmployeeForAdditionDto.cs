using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Employee
{
    public class EmployeeForAdditionDto
    {
        public Guid BusinessEntity { get; set; }
        public string Name { get; set; }
        // public int? SalesPersonId { get; set; }
        public string Demographics { get; set; }
        // public Guid Rowguid { get; set; }
        // public DateTime ModifiedDate { get; set; }
    }
}
