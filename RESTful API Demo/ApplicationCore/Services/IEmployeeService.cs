using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;
using Common.DTO.Employee;

namespace ApplicationCore.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetEmployees(EmployeeResourceParameters employeeResourceParameters);
        EmployeeDto GetEmployee(Guid guid);
        // List<CustomerDto> GetCustomers(Guid rowguid);
        EmployeeDto AddEmployee(EmployeeForAdditionDto employeeForAdditionDto);
    }
}
