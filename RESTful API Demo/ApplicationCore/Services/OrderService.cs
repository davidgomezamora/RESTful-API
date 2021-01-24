using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using ApplicationCore.DTO.Order;
using ApplicationCore.ResourceParameters;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions.Impl;
using System.Dynamic;
using Common.DataService;
using Common.DataRepository;

namespace ApplicationCore.Services
{
    public class OrderService : BaseService<Order, OrderDto, OrderForAdditionDto, OrderForUpdateDto, OrderForSortingDto, OrderResourceParameters>, IOrderService
    {
        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public OrderService(IRepository<Order> repository, IMapper mapper) : base(repository, mapper) { }

        public override void BuildSearchQueryFilter(OrderResourceParameters parameters, out QueryParameters<Order> queryParameters)
        {
            queryParameters = new QueryParameters<Order>();

            // Resuelve el filtro: EmployeeId
            if (!(parameters.EmployeeId is null))
            {
                queryParameters.WhereList.Add(x => x.EmployeeId.Equals(parameters.EmployeeId));
            }

            // Resuelve la busqueda: 
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery)){ }
        }
    }
}
