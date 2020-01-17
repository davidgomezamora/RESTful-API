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
    // Control de versiones soportadas por el endpoint de este controlador
    [ApiVersion("1.0")]
    // Control de versiones no soportadas por el endpoint de este controlador
    [ApiVersion("0.5", Deprecated = true)]
    // Definición del endpoint de este controlador ../api/products/
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Inyección del servicio de la capa ApplicationCore
        public ProductsController(IProductService productService)
        {
            this._productService = productService  ??
                throw new ArgumentNullException(nameof(productService));
        }

        // [GET]: .../api/products/
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProduct()
        {
            List<ProductDto> productDtos = this._productService.GetProducts();

            if(productDtos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(productDtos);
        }

        // [GET]: .../api/products/{id}
        [HttpGet("{productId}")]
        public ActionResult<ProductDto> GetProduct(int productId)
        {
            ProductDto productDTO = this._productService.GetProduct(productId);

            if(productDTO == null)
            {
                return NotFound();
            }

            return Ok(this._productService.GetProduct(productId));
        }
    }
}