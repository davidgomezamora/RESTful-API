using Common.DTO;
using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace ApplicationCore.Services
{
    public class ProductModelService : IProductModelService
    {
        private readonly IRepository<ProductModel> _repository;
        private readonly IMapper _mapper;

        // Inyección de los servicios: Repository y Mapper
        public ProductModelService(IRepository<ProductModel> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public ProductModelDto GetProductModel<T>(T productModelId)
        {
            return this._mapper.Map<ProductModelDto>(this._repository.GetById(productModelId));
        }
    }
}
