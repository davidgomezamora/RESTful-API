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
using Common.APIController;
using Common.DataService;
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
    public class EmployeesController : APIControllerBase<IEmployeeService, int, EmployeeDto, EmployeeForAdditionDto, EmployeeForUpdateDto, EmployeeForSortingDto, EmployeeResourceParameters>
    {
        private readonly IOrderService _orderService;
        public EmployeesController(IEmployeeService employeeService, IOrderService orderService) : base(employeeService, "Employee")
        {
            this._orderService = orderService ??
                throw new ArgumentNullException(nameof(orderService));
        }

        [HttpPost(Name = "AddEmployee")]
        public override Task<ActionResult<EmployeeDto>> AddAsync(EmployeeForAdditionDto additionDto)
        {
            return base.AddAsync(additionDto);
        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public override Task<ActionResult<ExpandoObject>> GetAsync(int id, string fields)
        {
            return base.GetAsync(id, fields);
        }

        [HttpGet(Name = "GetEmployees")]
        public override Task<ActionResult<PagedList<ExpandoObject>>> GetPagedListAsync([FromQuery] EmployeeResourceParameters resourceParameters)
        {
            return base.GetPagedListAsync(resourceParameters);
        }

        [HttpGet("list", Name = "GetEmployeesList")]
        public override Task<ActionResult<List<ExpandoObject>>> GetListAsync([FromQuery] EmployeeResourceParameters resourceParameters)
        {
            return base.GetListAsync(resourceParameters);
        }

        [HttpPut("{id}", Name = "UpdateEmployee")]
        public override Task<ActionResult<EmployeeDto>> UpdateAsync(int id, EmployeeForUpdateDto updateDto)
        {
            return base.UpdateAsync(id, updateDto);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateEmployee")]
        public override Task<ActionResult> PartiallyUpdateAsync(int id, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument)
        {
            return base.PartiallyUpdateAsync(id, jsonPatchDocument);
        }

        [HttpDelete("{id}", Name = "RemoveEmployee")]
        public override Task<ActionResult> RemoveAsync(int id)
        {
            return base.RemoveAsync(id);
        }

        [HttpGet("{employeeId}/orders")]
        public async Task<ActionResult<PagedList<ExpandoObject>>> GetOrdersListAsync(int employeeId, [FromQuery] OrderResourceParameters resourceParameters)
        {
            // Agregar id de filtro
            resourceParameters.EmployeeId = employeeId;

            if (!this._orderService.ValidateOrderByString(resourceParameters.OrderBy) || !this._orderService.ValidateFields(resourceParameters.Fields))
            {
                return BadRequest();
            }

            List<ExpandoObject> expandoObjects = await this._orderService.GetListAsync(resourceParameters);

            if (!expandoObjects.Any())
            {
                return NoContent();
            }

            return Ok(expandoObjects);
        }

        // Test
        [HttpGet("error")]
        public ActionResult<EmployeeDto> GetError()
        {
            throw new Exception("This is a test exception.");
        }
    }
}