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
        public ActionResult<IEnumerable<EmployeeDto>> GetEmployees([FromQuery] EmployeeResourceParameters employeeResourceParameters)
        {
            List<EmployeeDto> employeeDtos = this._employeeService.GetEmployees(employeeResourceParameters);

            if(employeeDtos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(employeeDtos);
        }

        // [GET]: .../api/employees/{employeeId}/
        [HttpGet("{employeeId}")]
        public ActionResult<EmployeeDto> GetEmployee(string employeeId)
        {
            EmployeeDto employeeDTO = this._employeeService.GetEmployee(employeeId);

            if(employeeDTO == null)
            {
                return NotFound();
            }

            return Ok(employeeDTO);
        }

        // [GET] .../api/employees/{employeeId}/orders/
        [HttpGet("{employeeId}/orders")]
        public ActionResult<IEnumerable<OrderDto>> GetOrdersForEmployee(string employeeId)
        {
            List<OrderDto> orderDtos = this._employeeService.GetOrders(employeeId);

            if (orderDtos == null)
            {
                return NotFound();
            }

            return Ok(orderDtos);
        }

        [HttpPost]
        public ActionResult<EmployeeDto> AddEmployee(EmployeeForAdditionDto employeeForAdditionDto)
        {
            EmployeeDto employeeDto = this._employeeService.AddEmployee(employeeForAdditionDto);

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
    }
}