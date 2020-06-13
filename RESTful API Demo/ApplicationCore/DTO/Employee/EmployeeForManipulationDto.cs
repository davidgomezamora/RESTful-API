using ApplicationCore.DTO.Order;
using ApplicationCore.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.DTO.Employee
{
    [EmployeeForManipulationValidation]
    public abstract class EmployeeForManipulationDto
    {
        [Required]
        [MaxLength(10)]
        public virtual string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public virtual string LastName { get; set; }
        [MaxLength(30)]
        public virtual string Title { get; set; }
        [MaxLength(25)]
        public virtual string TitleOfCourtesy { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? HireDate { get; set; }
        [MaxLength(60)]
        public virtual string Address { get; set; }
        [MaxLength(15)]
        public virtual string City { get; set; }
        [MaxLength(15)]
        public virtual string Region { get; set; }
        [MaxLength(10)]
        public virtual string PostalCode { get; set; }
        [MaxLength(15)]
        public virtual string Country { get; set; }
        [MaxLength(24)]
        public virtual string HomePhone { get; set; }
        [MaxLength(4)]
        public virtual string Extension { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string Notes { get; set; }
        public virtual int? ReportsTo { get; set; }
        [MaxLength(255)]
        public virtual string PhotoPath { get; set; }

        public List<OrderForAdditionDto> Orders { get; set; } = new List<OrderForAdditionDto>();
    }
}
