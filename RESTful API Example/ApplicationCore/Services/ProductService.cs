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

        public ProductService(IRepository<Product> repository)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public List<ProductDTO> GetProducts()
        {
            return ParseToObject<ProductDTO>(this._repository.GetList());
        }

        private List<O> ParseToObject<O>(List<Product> entityList)
        {
            string serializeObjectSource = JsonConvert.SerializeObject(entityList);
            return JsonConvert.DeserializeObject<List<O>>(serializeObjectSource);
        }
    }
}
