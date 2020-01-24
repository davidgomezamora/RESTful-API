using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Store
{
    public class StoreDto
    {
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public string Demographics { get; set; }
        public string ConcatenatedData {get; set; }
    }
}
