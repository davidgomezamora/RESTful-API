using Common.DTO;
using Infraestructure.Entities;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        // Inyección del servicio: Repository
        public ProductService(IRepository<Product> repository)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public List<ProductDto> GetProducts()
        {
            return ParseToObject<ProductDto>(this._repository.GetList());
        }

        public ProductDto GetProduct<T>(T productId)
        {
            return ParseToObject<ProductDto>(this._repository.GetById(productId));
        }

        private O ParseToObject<O>(Product entity)
        {
            string serializeObjectSource = JsonConvert.SerializeObject(entity);
            return JsonConvert.DeserializeObject<O>(serializeObjectSource);
        }

        private List<O> ParseToObject<O>(List<Product> entityList)
        {
            string serializeObjectSource = JsonConvert.SerializeObject(entityList);
            return JsonConvert.DeserializeObject<List<O>>(serializeObjectSource);
        }
    }
}
