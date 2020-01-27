using Common.DTO.Store;
using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;
using Common.DTO.Customer;

namespace ApplicationCore.Services
{
    public class StoreService : IStoreService
    {
        private readonly IRepository<Store> _repository;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios: Repository y Mapper
        public StoreService(IRepository<Store> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public List<StoreDto> GetStores(StoreResourceParameters storeResourceParameters)
        {
            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(storeResourceParameters.ModifiedDate.ToString()) && string.IsNullOrWhiteSpace(storeResourceParameters.SearchQuery))
            {
                return this._mapper.Map<List<StoreDto>>(this._repository.GetList());
            }

            // Lista de entidades: Store
            List<Store> stores = new List<Store>();

            // Resuelve el filtro
            if (!string.IsNullOrWhiteSpace(storeResourceParameters.ModifiedDate.ToString()))
            {               
                QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

                queryParameters.Where = x => x.ModifiedDate.Date.Equals(storeResourceParameters.ModifiedDate.Value.Date);

                stores = this._repository.FindBy(queryParameters);
            }

            // Resuelve la busqueda
            if (!string.IsNullOrWhiteSpace(storeResourceParameters.SearchQuery))
            {
                storeResourceParameters.SearchQuery = storeResourceParameters.SearchQuery.Trim();

                if (!string.IsNullOrWhiteSpace(storeResourceParameters.ModifiedDate.ToString()))
                {
                    return this._mapper.Map<List<StoreDto>>(stores.Where(x => x.Name.Contains(storeResourceParameters.SearchQuery)));
                }

                QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

                queryParameters.Where = x => x.Name.Contains(storeResourceParameters.SearchQuery);

                stores = this._repository.FindBy(queryParameters);
            }

            // Retorna el resultado con filtro y/o busqueda
            return this._mapper.Map<List<StoreDto>>(stores);
        }

        public StoreDto GetStore(Guid rowguid)
        {
            QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

            queryParameters.Where = x => x.Rowguid == rowguid;

            return this._mapper.Map<StoreDto>(this._repository.FindBy(queryParameters).FirstOrDefault());
        }

        public List<CustomerDto> GetCustomers(Guid rowguid)
        {
            QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

            queryParameters.Where = x => x.Rowguid == rowguid;
            queryParameters.PathRelatedEntities = new List<string>() { "Customer" };

            return this._mapper.Map<List<CustomerDto>>(this._repository.FindBy(queryParameters).SelectMany(x => x.Customer).ToList());
        }
    }
}
