using Common.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ResourceParameters
{
    public class EmployeeResourceParameters : ServiceParameters
    {
        public DateTime? HireDate { get; set; }
        public string? City { get; set; }
    }
}
