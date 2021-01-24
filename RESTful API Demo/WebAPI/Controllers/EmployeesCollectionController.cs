using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.DTO.Employee;
using ApplicationCore.Services;
using Common.APIController;
using Common.DataService;
using Infrastructure.Entities;
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
        [HttpGet("({ids})", Name = "GetEmployeesCollectionAsync")]
        public async Task<ActionResult<IEnumerable<ExpandoObject>>> GetEmployeesCollectionAsync([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids, string fields)
        {
            if (ids is null || !this._employeeService.ValidateFields(fields))
            {
                return BadRequest();
            }

            List<ExpandoObject> expandoObjects = await this._employeeService.GetAsync(ids.Cast<object>().ToList(), fields);

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
            List<EmployeeDto> employeesDto = await this._employeeService.AddAsync(employeesForAdditionDto);

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
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids is null)
            {
                return BadRequest();
            }

            if (ids.Count().Equals((await this._employeeService.RemoveAsync(ids.Cast<object>().ToList())).Count()))
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}