using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTO.Employee
{
    public class EmployeeForUpsertingDto
    {
        public EmployeeDto EmployeeDto;
        public ModelStateDictionary ModelStateDictionary;
    }
}
