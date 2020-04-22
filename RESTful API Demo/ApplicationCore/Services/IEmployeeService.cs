using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;
using Common.DTO.Employee;
using Common.DTO.Order;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetEmployeesAsync(EmployeeResourceParameters employeeResourceParameters);
        Task<EmployeeDto> GetEmployeeAsync(string employeeId);
        Task<List<OrderDto>> GetOrdersAsync(string employeeId);
        Task<EmployeeDto> AddEmployeeAsync(EmployeeForAdditionDto employeeForAdditionDto);
        Task<List<EmployeeDto>> AddEmployeesAsync(List<EmployeeForAdditionDto> employeesForAdditionDto);
        Task<Boolean> ExistsAsync(string employeeId);
        Task<Boolean> UpdateEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto);
        // Task<Boolean> UpdateOrderAsync(string employeeId, string orderId);
    }
}
