using AutoMapper;
using Common.DTO.BusinessEntity;
using Infraestructure.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Services
{
    public class BusinessEntityService : IBusinessEntityService
    {
        private readonly IRepository<BusinessEntity> _repository;
        private readonly IMapper _mapper;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios: Repository y Mapper
        public BusinessEntityService(IRepository<BusinessEntity> repository,
            IMapper mapper)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        public BusinessEntityDto GetBusinessEntity(Guid rowguid)
        {
            QueryParameters<BusinessEntity> queryParameters = new QueryParameters<BusinessEntity>(1, 100);

            queryParameters.Where = x => x.Rowguid == rowguid;

            return this._mapper.Map<BusinessEntityDto>(this._repository.FindBy(queryParameters).FirstOrDefault());
        }
    }
}
