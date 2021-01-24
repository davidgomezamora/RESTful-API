using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using ApplicationCore.DTO.Employee;
using ApplicationCore.DTO.Order;
using ApplicationCore.ResourceParameters;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions.Impl;
using System.Dynamic;
using Common.DataService;
using Common.DataRepository;

namespace ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employee, EmployeeDto, EmployeeForAdditionDto, EmployeeForUpdateDto, EmployeeForSortingDto, EmployeeResourceParameters>, IEmployeeService
    {
        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public EmployeeService(IRepository<Employee> repository, IMapper mapper) : base(repository, mapper) { }

        public override void BuildSearchQueryFilter(EmployeeResourceParameters parameters, out QueryParameters<Employee> queryParameters)
        {
            queryParameters = new QueryParameters<Employee>();

            // Resuelve el filtro: HireDate
            if (!(parameters.HireDate is null))
            {
                queryParameters.WhereList.Add(x => x.HireDate.Value.Date.Equals(parameters.HireDate.Value.Date));
            }

            // Resuelve el filtro: City
            if (!(parameters.City is null))
            {
                queryParameters.WhereList.Add(x => x.City.Equals(parameters.City));
            }

            // Resuelve la busqueda: FullName => (FirstName + LastName)
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                parameters.SearchQuery = parameters.SearchQuery.Trim();

                queryParameters.WhereList.Add(x => (x.FirstName + "" + x.LastName).Contains(parameters.SearchQuery));
            }
        }
    }
}
