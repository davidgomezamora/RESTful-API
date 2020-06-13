using CommonWebAPI;
using CommonWebAPI.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ResourceParameters
{
    public class EmployeeResourceParameters : ParametersBase
    {
        public DateTime? HireDate { get; set; }
        public string City { get; set; }
    }
}
