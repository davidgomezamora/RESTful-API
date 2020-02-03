using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Employee
{
    public class EmployeeForAdditionDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public List<OrderForAdditionDto> Orders { get; set; } = new List<OrderForAdditionDto>();
    }
}
