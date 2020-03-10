using Common.DTO.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.ValidationAttributes
{
    public class EmployeeForManipulationValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            EmployeeForManipulationDto employeeForManipulationDto = (EmployeeForManipulationDto)validationContext.ObjectInstance;

            if (employeeForManipulationDto.FirstName.Equals(employeeForManipulationDto.LastName))
            {
                return new ValidationResult(
                    "The provided first name should be different from the last name",
                    new[] { this.GetType().Name });
            }

            return ValidationResult.Success;
        }
    }
}
