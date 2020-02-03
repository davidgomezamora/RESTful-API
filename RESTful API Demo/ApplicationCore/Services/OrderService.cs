using AutoMapper;
using Common.DTO.Order;
using Infraestructure.Entities;
using Repository;
using Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Orders> _repository;
        private readonly IMapper _mapper;
        private readonly IDataSecurity _dataSecurity;

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public OrderService(IRepository<Orders> repository,
            IMapper mapper,
            IDataSecurity dataSecurity)
        {
            this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));
        }

        public OrderDto AddOrder(OrderForAdditionDto orderForAdditionDto)
        {
            Orders order = this._mapper.Map<Orders>(orderForAdditionDto);

            if (this._repository.Add(order))
            {
                return this._mapper.Map<OrderDto>(order);
            }

            return null;
        }
    }
}
