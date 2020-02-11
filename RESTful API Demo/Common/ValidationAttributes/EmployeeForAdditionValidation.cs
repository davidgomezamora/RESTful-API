using Common.DTO.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.ValidationAttributes
{
    public class EmployeeForAdditionValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            EmployeeForAdditionDto employeeForAdditionDto = (EmployeeForAdditionDto)validationContext.ObjectInstance;

            if (employeeForAdditionDto.FirstName.Equals(employeeForAdditionDto.LastName))
            {
                return new ValidationResult(
                    "The provided first name should be different from the last name",
                    new[] { this.GetType().Name });
            }

            return ValidationResult.Success;
        }
    }
}
