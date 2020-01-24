using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Customer
{
    public class CustomerDto
    {
        public string AccountNumber { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
