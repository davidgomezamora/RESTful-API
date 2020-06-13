using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApplicationCore.DTO.Employee;
using ApplicationCore.ResourceParameters;
using ApplicationCore.DTO.Order;
using CommonWebAPI.DataService;
using CommonWebAPI.Helpers;

namespace ApplicationCore.Services
{
    public interface IEmployeeService : IServiceBase
    {
        Task<PagedList<EmployeeDto>> GetEmployeesAsync(EmployeeResourceParameters employeeResourceParameters);
        //Task<EmployeeDto> GetEmployeeAsync(string employeeId);
        Task<List<OrderDto>> GetOrdersAsync(string employeeId);
        /*Task<EmployeeDto> AddEmployeeAsync(EmployeeForAdditionDto employeeForAdditionDto);
        Task<List<EmployeeDto>> AddEmployeesAsync(List<EmployeeForAdditionDto> employeesForAdditionDto);
        Task<Boolean> ExistsAsync(string employeeId);
        Task<Boolean> UpdateEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto);
        Task<ModelStateDictionary> PartiallyUpdateEmployeeAsync(string employeeId, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument);
        Task<EmployeeDto> UpsertingEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto);
        Task<EmployeeForUpsertingDto> UpsertingEmployeeAsync(string employeeId, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument);*/
    }
}
