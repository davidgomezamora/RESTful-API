using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.Employee
{
    public class EmployeeForAdditionDto : IValidatableObject
    {
        [Required]
        [MaxLength(10)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        public List<OrderForAdditionDto> Orders { get; set; } = new List<OrderForAdditionDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.FirstName.Equals(this.LastName))
            {
                yield return new ValidationResult(
                    "The provided first name should be different from the last name",
                    new[] { this.GetType().Name });
            }
        }
    }
}
