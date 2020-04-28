using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;
using Common.DTO.Employee;
using Security;
using Common.DTO.Order;
using Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using ServicesBase;

namespace ApplicationCore.Services
{
    public class EmployeeService : ServiceBase<Employees>, IEmployeeService
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
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }  else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.ExistsAsync(id);
        }

        public override Task<EntityDto> GetAsync<EntityDto>(object id)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.GetAsync<EntityDto>(id);
        }

        public override Task<List<EntityDto>> GetAsync<EntityDto>(List<object> ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                int j = 0;
                if (int.TryParse(ids[i].ToString(), out j))
                {
                    ids[i] = j;
                }
                else
                {
                    ids[i] = int.Parse(this._dataSecurity.AESDescrypt(ids[i].ToString()));
                }
            }

            return base.GetAsync<EntityDto>(ids);
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
            List<string> pathRelatedEntities = new List<string>()
            {
                { "Orders" }
            };

            return this._mapper.Map<List<OrderDto>>((await this._repository.GetByIdAsync(int.Parse(this._dataSecurity.AESDescrypt(employeeId)), pathRelatedEntities)).Orders.ToList());
        }
        
        public override Task<ModelStateDictionary> PartiallyUpdateAsync<EntityForUpdateDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.PartiallyUpdateAsync(id, jsonPatchDocument);
        }

        public override Task<bool> RemoveAsync(object id)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.RemoveAsync(id);
        }

        public override Task<int> RemoveAsync(List<object> ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                int j = 0;
                if (int.TryParse(ids[i].ToString(), out j))
                {
                    ids[i] = j;
                }
                else
                {
                    ids[i] = int.Parse(this._dataSecurity.AESDescrypt(ids[i].ToString()));
                }
            }

            return base.RemoveAsync(ids);
        }

        public override Task<bool> UpdateAsync(object id, object update)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpdateAsync(id, update);
        }

        public override Task<EntityDto> UpsertingAsync<EntityDto, EntityForAdditionDto>(object id, object update)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpsertingAsync<EntityDto, EntityForAdditionDto>(id, update);
        }

        public override Task<EntityDto> UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument, ModelStateDictionary modelStateDictionary)
        {
            int i = 0;
            if (int.TryParse(id.ToString(), out i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(id, jsonPatchDocument, modelStateDictionary);
        }
    }
}
