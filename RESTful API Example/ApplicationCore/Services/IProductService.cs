using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IProductService
    {
        List<ProductDTO> GetProducts();
    }
}
