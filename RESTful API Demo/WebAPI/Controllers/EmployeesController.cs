using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.DTO.Employee;
using ApplicationCore.DTO.Order;
using ApplicationCore.ResourceParameters;
using ApplicationCore.Services;
using Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RESTful_API_Demo.Controllers
{
    // Control de versiones soportadas por el endpoint de este controlador
    [ApiVersion("1.0")]
    // Control de versiones no soportadas por el endpoint de este controlador
    [ApiVersion("0.5", Deprecated = true)]
    // Definición del endpoint de este controlador ../api/employees/
    [Route("api/[controller]")]
    public class EmployeesController : APIControllerBase
    {
        private readonly IEmployeeService _employeeService;

        // Inyección del servicio de la capa ApplicationCore
        public EmployeesController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService ??
                throw new ArgumentNullException(nameof(employeeService));
        }

        // [GET]: .../api/employees/
        [HttpGet(Name = "GetEmployees")]
        public async Task<ActionResult<PagedList<ExpandoObject>>> GetEmployeesAsync([FromQuery] EmployeeResourceParameters employeeResourceParameters)
        {
            if (!this._employeeService.ValidateOrderBy(employeeResourceParameters.OrderBy) || !this._employeeService.ValidateFields<EmployeeDto>(employeeResourceParameters.Fields))
            {
                return BadRequest();
            }

            PagedList<ExpandoObject> pagedList = await this._employeeService.GetEmployeesAsync(employeeResourceParameters);

            if (pagedList.Results.Count() == 0)
            {
                return NoContent();
            }

            if(pagedList.HasPrevious)
            {
                pagedList.PreviousPageLink = CreateResourceUri(employeeResourceParameters, ResourceUriType.PreviousPage, "GetEmployees");
            }

            if (pagedList.HasNext)
            {
                pagedList.NextPageLink = CreateResourceUri(employeeResourceParameters, ResourceUriType.NextPage, "GetEmployees");
            }

            return Ok(pagedList);
        }

        // [GET]: .../api/employees/{employeeId}/
        [HttpGet("{employeeId}", Name = "GetEmployeeAsync")]
        public async Task<ActionResult<ExpandoObject>> GetEmployeeAsync(string employeeId, string fields)
        {
            if (!this._employeeService.ValidateFields<EmployeeDto>(fields))
            {
                return BadRequest();
            }

            ExpandoObject expandoObject = await this._employeeService.GetAsync<EmployeeDto>(fields, employeeId);

            if (expandoObject == null)
            {
                return NotFound();
            }

            return Ok(expandoObject);
        }

        // [GET] .../api/employees/{employeeId}/orders/
        [HttpGet("{employeeId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForEmployeeAsync(string employeeId)
        {
            List<OrderDto> orderDtos = await this._employeeService.GetOrdersAsync(employeeId);

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
            EmployeeDto employeeDto = await this._employeeService.AddAsync<EmployeeDto>(employeeForAdditionDto);

            if (employeeDto == null)
            {
                return NotFound();
            }

            return CreatedAtRoute("GetEmployeeAsync", new { employeeId = employeeDto.EmployeeId }, employeeDto);
        }

        // [PUT]: .../api/employees/{employeeId}/
        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto)
        {
            if (await this._employeeService.ExistsAsync(employeeId))
            {
                if (await this._employeeService.UpdateAsync(employeeId, employeeForUpdateDto))
                {
                    return NoContent();
                }

                // 304 (Not Modified)
                return StatusCode(304);
            }

            // Upserting
            EmployeeDto employeeDto = await this._employeeService.UpsertingAsync<EmployeeDto, EmployeeForAdditionDto>(employeeId, employeeForUpdateDto);

            if (employeeDto == null)
            {
                return NotFound();
            }

            return CreatedAtRoute("GetEmployeeAsync", new { employeeId = employeeDto.EmployeeId }, employeeDto);
        }

        // [PATCH]: .../api/employees/{employeeId}/
        [HttpPatch("{employeeId}")]
        public async Task<ActionResult> PartiallyUpdateEmployeeAsync(string employeeId, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument)
        {
            if (await this._employeeService.ExistsAsync(employeeId))
            {
                ModelStateDictionary modelStateDictionary = await this._employeeService.PartiallyUpdateAsync<EmployeeForUpdateDto>(employeeId, jsonPatchDocument);

                if (modelStateDictionary != null)
                {
                    ModelState.Merge(modelStateDictionary);

                    if (!ModelState.IsValid)
                    {
                        return ValidationProblem(ModelState);
                    }

                    return NoContent();
                }

                // 304 (Not Modified)
                return StatusCode(304);
            }

            // Upserting
            EmployeeDto employeeDto = await this._employeeService.UpsertingAsync<EmployeeDto, EmployeeForUpdateDto, EmployeeForAdditionDto>(employeeId, jsonPatchDocument, ModelState);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (employeeDto == null)
            {
                return NotFound();
            }

            return CreatedAtRoute("GetEmployeeAsync", new { employeeId = employeeDto.EmployeeId }, employeeDto);
        }

        // [DELETE]: .../api/employees/{employeeId}/
        [HttpDelete("{employeeId}")]
        public async Task<ActionResult> RemoveEmployeeAsync(string employeeId)
        {
            if (await this._employeeService.ExistsAsync(employeeId))
            {
                if (await this._employeeService.RemoveAsync(employeeId))
                {
                    return NoContent();
                }

                // 304 (Not Modified)
                return StatusCode(304);
            }

            return NotFound();
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
            Response.Headers.Add("Allow", "GET, POST, DELETE, PUT, PATCH");

            return Ok();
        }
    }
}