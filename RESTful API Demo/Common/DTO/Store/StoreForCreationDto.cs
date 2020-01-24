using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Store
{
    public class StoreForCreationDto
    {
        public string Name { get; set; }
        public int? SalesPersonId { get; set; }
        public string Demographics { get; set; }
        // public Guid Rowguid { get; set; }
        // public DateTime ModifiedDate { get; set; }
    }
}
