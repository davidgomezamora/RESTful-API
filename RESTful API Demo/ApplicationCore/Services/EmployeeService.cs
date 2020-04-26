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
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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
            // QueryParameters para fitlrado y/o busqueda de resultados
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>();

            queryParameters.PageSize = employeeResourceParameters.PageSize;
            queryParameters.PageNumber = employeeResourceParameters.PageNumber;

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
            return this._mapper.Map<List<EmployeeDto>>(await this._repository.FindByAsync(queryParameters));
        }

        public async Task<EmployeeDto> GetEmployeeAsync(string employeeId)
        {
            return this._mapper.Map<EmployeeDto>(await this._repository.GetByIdAsync(int.Parse(this._dataSecurity.AESDescrypt(employeeId))));
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>()
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId),
                PathRelatedEntities = new List<string>() { "Orders" }
            };

            return this._mapper.Map<List<OrderDto>>((await this._repository.FindByAsync(queryParameters)).SelectMany(x => x.Orders).ToList());
        }

        public async Task<EmployeeDto> AddEmployeeAsync(string employeeId, EmployeeForAdditionDto employeeForAdditionDto)
        {
            Employees employee = this._mapper.Map<Employees>(employeeForAdditionDto);
            employee.EmployeeId = int.Parse(this._dataSecurity.AESDescrypt(employeeId));

            if (await this._repository.AddAsync(employee))
            {
                return this._mapper.Map<EmployeeDto>(employee);
            }

            return null;
        }

        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeForAdditionDto employeeForAdditionDto)
        {
            Employees employee = this._mapper.Map<Employees>(employeeForAdditionDto);

            if (await this._repository.AddAsync(employee))
            {
                return this._mapper.Map<EmployeeDto>(employee);
            }

            return null;
        }

        public async Task<List<EmployeeDto>> AddEmployeesAsync(List<EmployeeForAdditionDto> employeesForAdditionDto)
        {
            // List<EmployeeDto> employeesDtos = await this._repository.AddAllAsync(this._mapper.Map<List<Employees>>(employeesDtos));
            List<EmployeeDto> employeesDtos = new List<EmployeeDto>();

            foreach (EmployeeForAdditionDto employeeForAdditionDto in employeesForAdditionDto)
            {
                employeesDtos.Add(await AddEmployeeAsync(employeeForAdditionDto));
            }

            return employeesDtos;
        }

        public Task<Boolean> ExistsAsync(string employeeId)
        {
            return this._repository.ExistsAsync(int.Parse(this._dataSecurity.AESDescrypt(employeeId)));
        }

        public async Task<Boolean> UpdateEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto)
        {
            Employees employee = this._mapper.Map<Employees>(employeeForUpdateDto);
            employee.EmployeeId = int.Parse(this._dataSecurity.AESDescrypt(employeeId));

            return await this._repository.UpdateAsync(employee);
        }

        public async Task<ModelStateDictionary> PartiallyUpdateEmployeeAsync(string employeeId, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument)
        {
            Employees employee = await this._repository.GetByIdAsync(int.Parse(this._dataSecurity.AESDescrypt(employeeId)));

            EmployeeForUpdateDto employeeForUpdateDto = this._mapper.Map<EmployeeForUpdateDto>(employee);

            this._repository.DetachedEntity(employee);

            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
            jsonPatchDocument.ApplyTo(employeeForUpdateDto, modelStateDictionary);

            if (modelStateDictionary.IsValid)
            {
                ValidationContext validationContext = new ValidationContext(employeeForUpdateDto);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (Validator.TryValidateObject(employeeForUpdateDto, validationContext, validationResults))
                {
                    if (!await this.UpdateEmployeeAsync(employeeId, employeeForUpdateDto))
                    {
                        return null;
                    }
                }

                foreach (ValidationResult validationResult in validationResults)
                {
                    modelStateDictionary.AddModelError(validationResult.MemberNames.FirstOrDefault(), validationResult.ErrorMessage);
                }
            }

            return modelStateDictionary;
        }

        public async Task<EmployeeDto> UpsertingEmployeeAsync(string employeeId, EmployeeForUpdateDto employeeForUpdateDto)
        {
            EmployeeForAdditionDto employeeForAdditionDto = this._mapper.Map<EmployeeForAdditionDto>(this._mapper.Map<Employees>(employeeForUpdateDto));

            return await this.AddEmployeeAsync(employeeId, employeeForAdditionDto);
        }

        public async Task<EmployeeForUpsertingDto> UpsertingEmployeeAsync(string employeeId, JsonPatchDocument<EmployeeForUpdateDto> jsonPatchDocument)
        {
            EmployeeForUpsertingDto employeeForUpsertingDto = new EmployeeForUpsertingDto();
            employeeForUpsertingDto.ModelStateDictionary = new ModelStateDictionary();

            EmployeeForUpdateDto employeeForUpdateDto = new EmployeeForUpdateDto();
            jsonPatchDocument.ApplyTo(employeeForUpdateDto, employeeForUpsertingDto.ModelStateDictionary);

            if (employeeForUpsertingDto.ModelStateDictionary.IsValid)
            {
                ValidationContext validationContext = new ValidationContext(employeeForUpdateDto);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (Validator.TryValidateObject(employeeForUpdateDto, validationContext, validationResults))
                {
                    EmployeeForAdditionDto employeeForAdditionDto = this._mapper.Map<EmployeeForAdditionDto>(this._mapper.Map<Employees>(employeeForUpdateDto));

                    employeeForUpsertingDto.EmployeeDto = await this.AddEmployeeAsync(employeeId, employeeForAdditionDto);
                }

                foreach (ValidationResult validationResult in validationResults)
                {
                    employeeForUpsertingDto.ModelStateDictionary.AddModelError(validationResult.MemberNames.FirstOrDefault(), validationResult.ErrorMessage);
                }
            }

            return employeeForUpsertingDto;
        }
    }
}
