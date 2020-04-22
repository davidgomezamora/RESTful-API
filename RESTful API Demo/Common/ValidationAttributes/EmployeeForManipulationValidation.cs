using Common.DTO.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

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

            if (!String.IsNullOrEmpty(employeeForManipulationDto.HomePhone) && !Regex.IsMatch(employeeForManipulationDto.HomePhone, @"^\([0-9]{3}\)\s[0-9]{3}\-[0-9]{3,15}$"))
            {
                return new ValidationResult(
                    "The home phone provided must be in the format (000) 000-000 ...",
                    new[] { this.GetType().Name });
            }

            return ValidationResult.Success;
        }
    }
}
