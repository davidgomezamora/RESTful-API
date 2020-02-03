using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Common.DTO.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_API_Demo.Controllers
{
    // Control de versiones soportadas por el endpoint de este controlador
    [ApiVersion("1.0")]
    // Control de versiones no soportadas por el endpoint de este controlador
    [ApiVersion("0.5", Deprecated = true)]
    // Definición del endpoint de este controlador ../api/employeesCollection/
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesCollectionController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        // Inyección del servicio de la capa ApplicationCore
        public EmployeesCollectionController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService ??
                throw new ArgumentNullException(nameof(employeeService));
        }

        // [POST] .../api/employeesCollection/
        [HttpPost]
        public ActionResult<IEnumerable<EmployeeDto>> AddEmployees(List<EmployeeForAdditionDto> employeesForAdditionDto)
        {
            List<EmployeeDto> employeesDto = this._employeeService.AddEmployees(employeesForAdditionDto);

            if (employeesDto == null)
            {
                return NotFound();
            }

            return Ok(employeesDto);
        }
    }
}