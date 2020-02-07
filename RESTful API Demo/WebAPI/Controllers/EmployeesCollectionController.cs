using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Common.DTO.Employee;
using Common.Helpers;
using Infraestructure.Entities;
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

        // [GET] .../api/employeesCollection/(id,id,id...)
        [HttpGet("({ids})")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesCollectionAsync(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();

            foreach (string id in ids)
            {
                employeeDtos.Add(await this._employeeService.GetEmployeeAsync(id));
            }

            if (!ids.Count().Equals(employeeDtos.Count()))
            {
                return NotFound();
            }

            return Ok(employeeDtos);
        }

        // [POST] .../api/employeesCollection/
        [HttpPost]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> AddEmployeesCollectionAsync(List<EmployeeForAdditionDto> employeesForAdditionDto)
        {
            List<EmployeeDto> employeesDto = await this._employeeService.AddEmployeesAsync(employeesForAdditionDto);

            if (employeesDto == null)
            {
                return NotFound();
            }

            return Ok(employeesDto);
        }
    }
}