using Common.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ResourceParameters
{
    public class OrderResourceParameters : ServiceParameters
    {
        public int? EmployeeId { get; set; }
    }
}
