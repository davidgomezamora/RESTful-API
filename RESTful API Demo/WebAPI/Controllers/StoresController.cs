using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Common.DTO.Customer;
using Common.DTO.Store;
using Common.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_API_Demo.Controllers
{
    // Control de versiones soportadas por el endpoint de este controlador
    [ApiVersion("1.0")]
    // Control de versiones no soportadas por el endpoint de este controlador
    [ApiVersion("0.5", Deprecated = true)]
    // Definición del endpoint de este controlador ../api/stores/
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        // Inyección del servicio de la capa ApplicationCore
        public StoresController(IStoreService storeService)
        {
            this._storeService = storeService  ??
                throw new ArgumentNullException(nameof(storeService));
        }

        // [GET]: .../api/stores/
        // [GET]: .../api/stores?modifiedDate={value}
        // [GET]: .../api/stores?searchQuery={value}
        // [GET]: .../api/stores?modifiedDate={value}&searchQuery={value}
        [HttpGet]
        public ActionResult<IEnumerable<StoreDto>> GetStores([FromQuery] StoreResourceParameters storeResourceParameters)
        {
            List<StoreDto> storeDtos = this._storeService.GetStores(storeResourceParameters);

            if(storeDtos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(storeDtos);
        }

        // [GET]: .../api/stores/{rowguid}/
        [HttpGet("{rowguid}")]
        public ActionResult<StoreDto> GetStore(Guid rowguid)
        {
            StoreDto storeDTO = this._storeService.GetStore(rowguid);

            if(storeDTO == null)
            {
                return NotFound();
            }

            return Ok(storeDTO);
        }

        // [GET] .../api/stores/{rowguid}/customers/
        [HttpGet("{rowguid}/customers")]
        public ActionResult<IEnumerable<CustomerDto>> GetCustomersForStore(Guid rowguid)
        {
            List<CustomerDto> customerDtos = this._storeService.GetCustomers(rowguid);

            if (customerDtos == null)
            {
                return NotFound();
            }

            return Ok(customerDtos);
        }

        // [GET]: .../api/stores/error/
        [HttpGet("error")]
        public ActionResult<StoreDto> GetError()
        {
            throw new Exception("This is a test exception.");
        }
    }
}