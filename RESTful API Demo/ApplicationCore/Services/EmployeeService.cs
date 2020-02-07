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
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employees> _repository;
        private readonly IDataSecurity _dataSecurity;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
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

        public async Task<List<EmployeeDto>> GetEmployeesAsync(EmployeeResourceParameters employeeResourceParameters)
        {
            List<Employees> employees;

            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(employeeResourceParameters.HireDate.ToString()) && string.IsNullOrWhiteSpace(employeeResourceParameters.City) && string.IsNullOrWhiteSpace(employeeResourceParameters.SearchQuery))
            {
                employees = await Task.Run(() =>
                {
                    return this._repository.GetList();
                });

                return this._mapper.Map<List<EmployeeDto>>(employees);
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

            employees = await Task.Run(() =>
            {
                return this._repository.FindBy(queryParameters);
            });

            // Retorna el resultado con filtro y/o busqueda
            return this._mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100)
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId)
            };

            Employees employees = await Task.Run(() =>
            {
                return this._repository.FindBy(queryParameters).FirstOrDefault();
            });


            return this._mapper.Map<EmployeeDto>(employees);
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100)
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId),
                PathRelatedEntities = new List<string>() { "Orders" }
            };

            List<Orders> orders = await Task.Run(() =>
            {
                return this._repository.FindBy(queryParameters).SelectMany(x => x.Orders).ToList();
            });

            return this._mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeForAdditionDto employeeForAdditionDto)
        {
            Employees employee = this._mapper.Map<Employees>(employeeForAdditionDto);

            bool result = await Task.Run(() =>
            {
                return this._repository.Add(employee);
            });

            if (result)
            {
                return this._mapper.Map<EmployeeDto>(employee);
            }

            return null;
        }

        public async Task<List<EmployeeDto>> AddEmployeesAsync(List<EmployeeForAdditionDto> employeesForAdditionDto)
        {
            List<EmployeeDto> employeesDtos = new List<EmployeeDto>();

            foreach (EmployeeForAdditionDto employeeForAdditionDto in employeesForAdditionDto)
            {
                employeesDtos.Add(await AddEmployeeAsync(employeeForAdditionDto));
            }

            return employeesDtos;
        }
    }
}
