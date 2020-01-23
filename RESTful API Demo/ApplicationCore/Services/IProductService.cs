using Common.DTO.Product;
using Common.DTO.ProductModel;
using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IProductService
    {
        List<ProductDto> GetProducts(ProductResourceParameters productResourceParameters);
        ProductDto GetProduct<T>(T productId);
        ProductModelDto GetProductModel<T>(T productId);
    }
}
