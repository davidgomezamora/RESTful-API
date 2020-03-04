using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Common.DTO.Employee;
using Common.DTO.Order;
using Common.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_API_Demo.Controllers
{
    // Control de versiones soportadas por el endpoint de este controlador
    [ApiVersion("1.0")]
    // Control de versiones no soportadas por el endpoint de este controlador
    [ApiVersion("0.5", Deprecated = true)]
    // Definición del endpoint de este controlador ../api/employees/
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        // Inyección del servicio de la capa ApplicationCore
        public EmployeesController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService  ??
                throw new ArgumentNullException(nameof(employeeService));
        }

        // [GET]: .../api/employees/
        // [GET]: .../api/employees?modifiedDate={value}
        // [GET]: .../api/employees?searchQuery={value}
        // [GET]: .../api/employees?modifiedDate={value}&searchQuery={value}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync([FromQuery] EmployeeResourceParameters employeeResourceParameters)
        {
            List<EmployeeDto> employeeDtos = await this._employeeService.GetEmployeesAsync(employeeResourceParameters);

            if(employeeDtos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(employeeDtos);
        }

        // [GET]: .../api/employees/{employeeId}/
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeAsync(string employeeId)
        {
            EmployeeDto employeeDTO = await this._employeeService.GetEmployeeAsync(employeeId);

            if(employeeDTO == null)
            {
                return NotFound();
            }

            return Ok(employeeDTO);
        }

        // [GET] .../api/employees/{employeeId}/orders/
        [HttpGet("{employeeId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForEmployeeAsync(string employeeId)
        {
            List<OrderDto> orderDtos = await this._employeeService.GetOrdersForEmployeeAsync(employeeId);

            if (orderDtos == null)
            {
                return NotFound();
            }

            return Ok(orderDtos);
        }

        // [POST] .../api/employees/
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> AddEmployeeAsync(EmployeeForAdditionDto employeeForAdditionDto)
        {
            EmployeeDto employeeDto = await this._employeeService.AddEmployeeAsync(employeeForAdditionDto);

            if(employeeDto == null)
            {
                return NotFound();
            }

            return Ok(employeeDto);
        }

        // [GET]: .../api/employees/error/
        [HttpGet("error")]
        public ActionResult<EmployeeDto> GetError()
        {
            throw new Exception("This is a test exception.");
        }

        // [OPTIONS]: .../api/employees/
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, PATHC, DELETE");

            return Ok();
        }

        // [PUT]: .../api/employees/{employeeId}/
        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto)
        {
            EmployeeDto employeeDto = await this._employeeService.UpdateEmployeeAsync(employeeId, employeeForUpdateDto);

            if (employeeDto == null)
            {
                return NotFound();
            }

            return Ok(employeeDto);
        }

        // [PUT]: .../api/employees/{employeeId}/orders/{orderId}
        [HttpPut("{employeeId}/orders/{orderId}")]
        public ActionResult UpdateOrderForEmployeeAsync(string employeeId, string orderId, EmployeeForUpdateDto employeeForUpdateDto)
        {
            return Ok();
        }
    }
}