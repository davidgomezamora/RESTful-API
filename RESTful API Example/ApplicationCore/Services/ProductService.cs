using Common.DTO;
using Infraestructure.Entities;
using Newtonsoft.Json;
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

        // Inyección del servicio: Repository
        public ProductService(IRepository<Product> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public List<ProductDto> GetProducts()
        {
            return this._mapper.Map<List<ProductDto>>(this._repository.GetList());
        }

        public ProductDto GetProduct<T>(T productId)
        {
            return this._mapper.Map<ProductDto>(this._repository.GetById(productId));
        }
    }
}
