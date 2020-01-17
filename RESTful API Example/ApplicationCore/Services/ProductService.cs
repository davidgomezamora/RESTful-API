using Common.DTO;
using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

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

        public List<ProductDto> GetProducts()
        {
            return this._mapper.Map<List<ProductDto>>(this._repository.GetList());
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
