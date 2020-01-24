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
        // private readonly IProductModelService _storeModelService;

        // Inyección de los servicios: Repository y Mapper
        public StoreService(IRepository<Store> repository,
            IMapper mapper/*,
            IProductModelService storeModelService*/)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            /*this._storeModelService = storeModelService ??
                throw new ArgumentNullException(nameof(storeModelService));*/

            this._repository.propertyId = "Rowguid";
        }

        public List<StoreDto> GetStores(StoreResourceParameters storeResourceParameters)
        {
            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(storeResourceParameters.ModifiedDate.ToString()) && string.IsNullOrWhiteSpace(storeResourceParameters.SearchQuery))
            {
                var test = this._repository.GetList();

                return this._mapper.Map<List<StoreDto>>(this._repository.GetList());
            }

            // Lista de entidades: Product
            List<Store> stores = new List<Store>();

            // Resuelve el filtro
            if (!string.IsNullOrWhiteSpace(storeResourceParameters.ModifiedDate.ToString()))
            {               
                QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

                queryParameters.Where = x => x.ModifiedDate == storeResourceParameters.ModifiedDate;

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
            return this._mapper.Map<StoreDto>(this._repository.GetById(rowguid));
        }

        public List<CustomerDto> GetCustomers(Guid rowguid)
        {
            QueryParameters<Store> queryParameters = new QueryParameters<Store>(1, 100);

            queryParameters.PathRelatedEntities = new List<string>() { "Customer" };

            return this._mapper.Map<List<CustomerDto>>(this._repository.GetById(rowguid, queryParameters).Customer);
        }

        /*public ProductModelDto GetProductModel<T>(T rowguid)
        {
            return this._mapper.Map<ProductModelDto>(this._storeModelService.GetProductModel(this._repository.GetById(rowguid).SalesPerson));
        }*/
    }
}
