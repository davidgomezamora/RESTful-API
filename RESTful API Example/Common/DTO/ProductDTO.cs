using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public bool? MakeFlag { get; set; }
        public string NameNumber { get; set; }

        public int? ProductModelId { get; set; }
    }
}
