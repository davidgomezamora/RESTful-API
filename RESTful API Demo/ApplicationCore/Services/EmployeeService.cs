using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;
using Common.DTO.Employee;
using System.Linq.Expressions;
using Security;
using Common.DTO.Order;
using Repository;

namespace ApplicationCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employees> _repository;
        private readonly IDataSecurity _dataSecurity;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios: Repository y Mapper
        public EmployeeService(IRepository<Employees> repository,
            IDataSecurity dataSecurity,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public List<EmployeeDto> GetEmployees(EmployeeResourceParameters employeeResourceParameters)
        {
            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(employeeResourceParameters.HireDate.ToString()) && string.IsNullOrWhiteSpace(employeeResourceParameters.City) && string.IsNullOrWhiteSpace(employeeResourceParameters.SearchQuery))
            {
                return this._mapper.Map<List<EmployeeDto>>(this._repository.GetList());
            }

            // QueryParameters para fitlrado y/o busqueda de resultados
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100);

            // Resuelve el filtro: HireDate
            if (!string.IsNullOrWhiteSpace(employeeResourceParameters.HireDate.ToString()))
            {
                queryParameters.WhereList.Add(x => x.HireDate.Value.Date.Equals(employeeResourceParameters.HireDate.Value.Date));
            }

            // Resuelve el filtro: City
            if (!string.IsNullOrWhiteSpace(employeeResourceParameters.City))
            {
                queryParameters.WhereList.Add(x => x.City.Equals(employeeResourceParameters.City));
            }

            // Resuelve la busqueda: FullName => (FirstName + LastName)
            if (!string.IsNullOrWhiteSpace(employeeResourceParameters.SearchQuery))
            {
                employeeResourceParameters.SearchQuery = employeeResourceParameters.SearchQuery.Trim();

                queryParameters.WhereList.Add(x => (x.FirstName + "" + x.LastName).Contains(employeeResourceParameters.SearchQuery));
            }

            // Retorna el resultado con filtro y/o busqueda
            return this._mapper.Map<List<EmployeeDto>>(this._repository.FindBy(queryParameters));
        }

        public EmployeeDto GetEmployee(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100)
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId)
            };

            return this._mapper.Map<EmployeeDto>(this._repository.FindBy(queryParameters).FirstOrDefault());
        }

        public List<OrderDto> GetOrders(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100)
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId),
                PathRelatedEntities = new List<string>() { "Orders" }
            };

            return this._mapper.Map<List<OrderDto>>(this._repository.FindBy(queryParameters).SelectMany(x => x.Orders).ToList());
        }

        public EmployeeDto AddEmployee(EmployeeForAdditionDto employeeForAdditionDto)
        {
            Employees employee = this._mapper.Map<Employees>(employeeForAdditionDto);

            if (this._repository.Add(employee))
            {
                return this._mapper.Map<EmployeeDto>(employee);
            }

            return null;
        }
    }
}
