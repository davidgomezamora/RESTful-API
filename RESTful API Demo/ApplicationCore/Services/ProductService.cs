using Common.DTO;
using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias
        private readonly IProductModelService _productModelService;

        // Inyección de los servicios: Repository y Mapper
        public ProductService(IRepository<Product> repository,
            IMapper mapper,
            IProductModelService productModelService)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this._productModelService = productModelService ??
                throw new ArgumentNullException(nameof(productModelService));
        }

        public List<ProductDto> GetProducts(ProductResourceParameters productResourceParameters)
        {
            // Resuelve la solicitud de lista de resultados sin filtros
            if (string.IsNullOrWhiteSpace(productResourceParameters.Color) && string.IsNullOrWhiteSpace(productResourceParameters.SearchQuery))
            {
                return this._mapper.Map<List<ProductDto>>(this._repository.GetList());
            }

            // Lista de entidades: Product
            List<Product> products = new List<Product>();

            // Resuelve el filtro
            if (!string.IsNullOrWhiteSpace(productResourceParameters.Color))
            {
                productResourceParameters.Color = productResourceParameters.Color.Trim();                

                QueryParameters<Product> queryParameters = new QueryParameters<Product>(1, 100);

                queryParameters.where = x => x.Color == productResourceParameters.Color;

                products = this._repository.FindBy(queryParameters);
            }

            // Resuelve la busqueda
            if (!string.IsNullOrWhiteSpace(productResourceParameters.SearchQuery))
            {
                productResourceParameters.SearchQuery = productResourceParameters.SearchQuery.Trim();

                if (!string.IsNullOrWhiteSpace(productResourceParameters.Color))
                {
                    return this._mapper.Map<List<ProductDto>>(products.Where(x => x.Name.Contains(productResourceParameters.SearchQuery)));
                }

                QueryParameters<Product> queryParameters = new QueryParameters<Product>(1, 100);

                queryParameters.where = x => x.Name.Contains(productResourceParameters.SearchQuery);

                products = this._repository.FindBy(queryParameters);
            }

            // Retorna el resultado con filtro y/o busqueda
            return this._mapper.Map<List<ProductDto>>(products);
        }

        public ProductDto GetProduct<T>(T productId)
        {
            return this._mapper.Map<ProductDto>(this._repository.GetById(productId));
        }

        public ProductModelDto GetProductModel<T>(T productId)
        {
            return this._mapper.Map<ProductModelDto>(this._productModelService.GetProductModel(this._repository.GetById(productId).ProductModelId));
        }
    }
}
