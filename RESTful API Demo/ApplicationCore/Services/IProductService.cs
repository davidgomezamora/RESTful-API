using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IProductService
    {
        List<ProductDto> GetProducts(string color = "", string searchName = "");
        ProductDto GetProduct<T>(T productId);
        ProductModelDto GetProductModel<T>(T productId);
    }
}
