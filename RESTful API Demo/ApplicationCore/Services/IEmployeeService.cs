﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApplicationCore.DTO.Employee;
using ApplicationCore.ResourceParameters;
using ApplicationCore.DTO.Order;
using Infrastructure.Entities;
using System.Dynamic;
using Common.DataService;

namespace ApplicationCore.Services
{
    public interface IEmployeeService : IBaseService<EmployeeDto, EmployeeForAdditionDto, EmployeeForUpdateDto, EmployeeForSortingDto, EmployeeResourceParameters> { }
}
