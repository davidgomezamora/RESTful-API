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
    public class EmployeeService : DatabaseService<Employees>, IEmployeeService
    {
        //private readonly IRepository<Employees> _repository;
        private readonly IDataSecurity _dataSecurity;
        //private readonly IMapper _mapper;*/
        // private readonly IDatabaseService<Employees> _databaseService;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public EmployeeService(IRepository<Employees> repository,
            IDataSecurity dataSecurity,
            IMapper mapper/*,
            IDatabaseService<Employees> databaseService*/)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            /*this._databaseService = databaseService ??
                throw new ArgumentNullException(nameof(databaseService));*/
        }

        public override Task<bool> ExistsAsync(object id)
        {
            return base.ExistsAsync(this._dataSecurity.AESDescrypt(id.ToString()));
        }

        public override Task<bool> UpdateAsync(object id, object update)
        {
            return base.UpdateAsync(this._dataSecurity.AESDescrypt(id.ToString()), update);
        }

        public override Task<ModelStateDictionary> PartiallyUpdateAsync<EntityForUpdateDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument)
        {
            return base.PartiallyUpdateAsync(this._dataSecurity.AESDescrypt(id.ToString()), jsonPatchDocument);
        }

        public override Task<EntityDto> UpsertingAsync<EntityDto, EntityForAdditionDto>(object id, object update)
        {
            return base.UpsertingAsync<EntityDto, EntityForAdditionDto>(this._dataSecurity.AESDescrypt(id.ToString()), update);
        }

        public override Task<EntityDto> UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument, ModelStateDictionary modelStateDictionary)
        {
            return base.UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(this._dataSecurity.AESDescrypt(id.ToString()), jsonPatchDocument, modelStateDictionary);
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

        public async Task<List<OrderDto>> GetOrdersAsync(string employeeId)
        {
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>()
            {
                Where = x => x.EmployeeId.ToString() == this._dataSecurity.AESDescrypt(employeeId),
                PathRelatedEntities = new List<string>() { "Orders" }
            };

            return this._mapper.Map<List<OrderDto>>((await this._repository.FindByAsync(queryParameters)).SelectMany(x => x.Orders).ToList());
        }
    }
}
