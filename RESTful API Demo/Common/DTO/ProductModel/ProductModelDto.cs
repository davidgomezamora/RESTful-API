using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.ProductModel
{
    public class ProductModelDto
    {
        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public string Instructions { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
