using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ResourceParameters
{
    public class EmployeeResourceParameters : ResourceParameters
    {
        public DateTime? HireDate{ get; set; }
        public string City { get; set; }
    }
}
