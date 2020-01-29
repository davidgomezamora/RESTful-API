using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;
using Common.DTO.Employee;
using System.Linq.Expressions;

namespace ApplicationCore.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employees> _repository;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios: Repository y Mapper
        public EmployeeService(IRepository<Employees> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public List<EmployeeDto> GetEmployees(EmployeeResourceParameters employeeResourceParameters)
        {
            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(employeeResourceParameters.HireDate.ToString()) && string.IsNullOrWhiteSpace(employeeResourceParameters.SearchQuery))
            {
                return this._mapper.Map<List<EmployeeDto>>(this._repository.GetList());
            }

            // QueryParameters para fitlrado y/o busqueda de resultados
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100);

            // Resuelve el filtro
            if (!string.IsNullOrWhiteSpace(employeeResourceParameters.HireDate.ToString()))
            {
                queryParameters.WhereList.Add(x => x.HireDate.Value.Date.Equals(employeeResourceParameters.HireDate.Value.Date));
            }

            // Resuelve la busqueda
            if (!string.IsNullOrWhiteSpace(employeeResourceParameters.SearchQuery))
            {
                employeeResourceParameters.SearchQuery = employeeResourceParameters.SearchQuery.Trim();

                queryParameters.WhereList.Add(x => (x.FirstName + "" + x.LastName).Contains(employeeResourceParameters.SearchQuery));
            }

            // Retorna el resultado con filtro y/o busqueda
            return this._mapper.Map<List<EmployeeDto>>(this._repository.FindBy(queryParameters));
        }

        public EmployeeDto GetEmployee(Guid guid)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100);

            queryParameters.Where = x => Guid.Parse(x.EmployeeId.ToString()) == guid;

            return this._mapper.Map<EmployeeDto>(this._repository.FindBy(queryParameters).FirstOrDefault());
        }

        public EmployeeDto AddEmployee(EmployeeForAdditionDto employeeForAdditionDto)
        {
            throw new NotImplementedException();
        }

        /*public List<CustomerDto> GetCustomers(Guid guid)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>(1, 100);

            queryParameters.Where = x => x.Rowguid == guid;
            queryParameters.PathRelatedEntities = new List<string>() { "Customer" };

            return this._mapper.Map<List<CustomerDto>>(this._repository.FindBy(queryParameters).SelectMany(x => x.Customer).ToList());
        }*/

                    /*public EmployeeDto AddEmployee(EmployeeForAdditionDto employeeForAdditionDto)
                    {
                        BusinessEntityDto bussinessEntity = this._businessEntityService.GetBusinessEntity(employeeForAdditionDto.BusinessEntity);

                        if(bussinessEntity == null)
                        {
                            return null;
                        }

                        Employees employee = this._mapper.Map<Employees>(employeeForAdditionDto);

                        // employee.BusinessEntityId = bussinessEntity.BusinessEntityId;

                        bool result = this._repository.Add(employee);

                        EmployeeDto employeeToReturn = this._mapper.Map<EmployeeDto>(employee);

                        return GetEmployee(Guid.Parse(employeeToReturn.EmployeeId.ToString());
                    }*/
                }
            }
