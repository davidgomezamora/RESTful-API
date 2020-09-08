using Infraestructure.Entities;
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
using Common.BaseService;
using Common.Security;
using Common.DataRepository;
using Common.Helpers;
using System.Dynamic;

namespace ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employees, EmployeeForSortingDto>, IEmployeeService
    {
        private readonly IDataSecurity _dataSecurity;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public EmployeeService(IRepository<Employees> repository,
            IDataSecurity dataSecurity,
            IMapper mapper) : base(repository, mapper)
        {

            this.Repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));

            this.Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public override Task<bool> ExistsAsync(params object[] id)
        {
            if (int.TryParse(id[0].ToString(), out int i))
            {
                id[0] = i;
            }  else
            {
                id[0] = int.Parse(this._dataSecurity.AESDescrypt(id[0].ToString()));
            }

            return base.ExistsAsync(id);
        }

        public override Task<ExpandoObject> GetAsync<TEntityDto>(string fields, params object[] id)
        {
            if (int.TryParse(id[0].ToString(), out int i))
            {
                id[0] = i;
            }
            else
            {
                id[0] = int.Parse(this._dataSecurity.AESDescrypt(id[0].ToString()));
            }

            return base.GetAsync<TEntityDto>(fields, id);
        }

        public override Task<List<ExpandoObject>> GetAsync<TEntityDto>(List<object> ids, string fields)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                if (int.TryParse(ids[i].ToString(), out int j))
                {
                    ids[i] = j;
                }
                else
                {
                    ids[i] = int.Parse(this._dataSecurity.AESDescrypt(ids[i].ToString()));
                }
            }

            return base.GetAsync<TEntityDto>(ids, fields);
        }
        
        public override Task<ModelStateDictionary> PartiallyUpdateAsync<TUpdateDto>(object id, JsonPatchDocument<TUpdateDto> jsonPatchDocument)
        {
            if (int.TryParse(id.ToString(), out int i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.PartiallyUpdateAsync(id, jsonPatchDocument);
        }

        public override Task<bool> RemoveAsync(params object[] id)
        {
            if (int.TryParse(id[0].ToString(), out int i))
            {
                id[0] = i;
            }
            else
            {
                id[0] = int.Parse(this._dataSecurity.AESDescrypt(id[0].ToString()));
            }

            return base.RemoveAsync(id);
        }

        public override Task<List<object>> RemoveAsync(List<object> ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                if (int.TryParse(ids[i].ToString(), out int j))
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
            if (int.TryParse(id.ToString(), out int i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpdateAsync(id, update);
        }

        public override Task<EmployeeDto> UpsertingAsync<EmployeeDto, TAdditionDto>(object id, object update)
        {
            if (int.TryParse(id.ToString(), out int i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpsertingAsync<EmployeeDto, TAdditionDto>(id, update);
        }

        public override Task<EmployeeDto> UpsertingAsync<EmployeeDto, TUpdateDto, TAdditionDto>(object id, JsonPatchDocument<TUpdateDto> jsonPatchDocument, ModelStateDictionary modelStateDictionary)
        {
            if (int.TryParse(id.ToString(), out int i))
            {
                id = i;
            }
            else
            {
                id = int.Parse(this._dataSecurity.AESDescrypt(id.ToString()));
            }

            return base.UpsertingAsync<EmployeeDto, TUpdateDto, TAdditionDto>(id, jsonPatchDocument, modelStateDictionary);
        }

        public async Task<PagedList<ExpandoObject>> GetEmployeesAsync(EmployeeResourceParameters employeeResourceParameters)
        {
            // QueryParameters para fitlrado y/o busqueda de resultados
            QueryParameters<Employees> queryParameters = new QueryParameters<Employees>
            {
                PageSize = employeeResourceParameters.PageSize,
                PageNumber = employeeResourceParameters.PageNumber,
                OrdersBy = employeeResourceParameters.OrderBy,
                PropertyMappings = this.GetPropertyMappingFromAutomapper(new List<string>())
            };

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

            // Conteo de resultados totales
            int count = await this.Repository.CountAsync(queryParameters.WhereList);

            // Resultado con filtro y/o busqueda paginada
            List<EmployeeDto> employeeDtos = this.Mapper.Map<List<EmployeeDto>>(await this.Repository.GetAsync(queryParameters));

            // Retorna el resultado con filtro y/o busqueda y meta datos paginados
            return new PagedList<ExpandoObject>(employeeDtos.ShapeData<EmployeeDto>(employeeResourceParameters.Fields), count, employeeResourceParameters);
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string employeeId)
        {
            List<string> pathRelatedEntities = new List<string>()
            {
                { "Orders" }
            };

            return this.Mapper.Map<List<OrderDto>>((await this.Repository.GetAsync(int.Parse(this._dataSecurity.AESDescrypt(employeeId)), pathRelatedEntities)).Orders.ToList());
        }
    }
}
