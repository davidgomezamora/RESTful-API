using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_API_Example.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("0.5", Deprecated = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService  ??
                throw new ArgumentNullException(nameof(productService));
        }

        // [GET]: .../api/products/
        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetProduct()
        {
            return this._productService.GetProducts();
        }

        // [GET]: .../api/products/{id}
        /*[HttpGet()]
        public ActionResult<IEnumerable<ProductDTO>> GetProduct(int id)
        {
            return this._productService.GetProducts();
        }*/
    }
}