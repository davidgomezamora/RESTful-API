using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.Customer;

namespace ApplicationCore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        // Inyección de los servicios: Repository y Mapper
        public CustomerService(IRepository<Customer> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public CustomerDto GetCustomer<T>(T productModelId)
        {
            throw new NotImplementedException();
        }

        /*public ProductModelDto GetCustomer<T>(T productModelId)
        {
            return this._mapper.Map<ProductModelDto>(this._repository.GetById(productModelId));
        }*/
    }
}
