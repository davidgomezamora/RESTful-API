using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;
using Common.DTO.Employee;
using Common.DTO.Order;

namespace ApplicationCore.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetEmployees(EmployeeResourceParameters employeeResourceParameters);
        EmployeeDto GetEmployee(string employeeId);
        List<OrderDto> GetOrders(string employeeId);
        EmployeeDto AddEmployee(EmployeeForAdditionDto employeeForAdditionDto);
    }
}
