using Common.DTO.Order;
using Common.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.Employee
{
    [EmployeeForAdditionValidation]
    public class EmployeeForAdditionDto
    {
        [Required]
        [MaxLength(10)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        public List<OrderForAdditionDto> Orders { get; set; } = new List<OrderForAdditionDto>();
    }
}
