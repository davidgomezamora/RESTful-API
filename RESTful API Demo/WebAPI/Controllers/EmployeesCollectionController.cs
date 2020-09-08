using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.DTO.Employee;
using ApplicationCore.Services;
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
    public class EmployeesCollectionController : APIControllerBase
    {
        private readonly IEmployeeService _employeeService;

        // Inyección del servicio de la capa ApplicationCore
        public EmployeesCollectionController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService ??
                throw new ArgumentNullException(nameof(employeeService));
        }

        // [GET] .../api/employeesCollection/(id,id,id...)
        [HttpGet("({ids})", Name = "GetEmployeesCollectionAsync")]
        public async Task<ActionResult<IEnumerable<ExpandoObject>>> GetEmployeesCollectionAsync([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> ids, string fields)
        {
            if (ids == null || !this._employeeService.ValidateFields<EmployeeDto>(fields))
            {
                return BadRequest();
            }

            List<ExpandoObject> expandoObjects = await this._employeeService.GetAsync<EmployeeDto>(ids.ToList<object>(), fields);

            if (!ids.Count().Equals(expandoObjects.Count()))
            {
                return NotFound();
            }

            return Ok(expandoObjects);
        }

        // [POST] .../api/employeesCollection/
        [HttpPost]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> AddEmployeesCollectionAsync(List<EmployeeForAdditionDto> employeesForAdditionDto)
        {
            List<EmployeeDto> employeesDto = await this._employeeService.AddAsync<EmployeeDto, EmployeeForAdditionDto>(employeesForAdditionDto);

            if (employeesDto == null)
            {
                return NotFound();
            }

            string idsEmployees = String.Join(",", employeesDto.Select(x => x.EmployeeId));

            return CreatedAtRoute("GetEmployeesCollectionAsync", new { ids = idsEmployees }, employeesDto);
        }

        // [DELETE] .../api/employeesCollection/(id,id,id...)
        [HttpDelete("({ids})")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> RemoveEmployeesCollectionAsync(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            if (ids.Count() == (await this._employeeService.RemoveAsync(ids.ToList<object>())).Count())
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}