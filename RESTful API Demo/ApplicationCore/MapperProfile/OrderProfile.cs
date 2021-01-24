using ApplicationCore.DTO.Order;
using AutoMapper;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.MapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Order, OrderDto>();
            CreateMap<Order, OrderForUpdateDto>()
                .ReverseMap();
            CreateMap<Order, OrderForAdditionDto>()
                .ReverseMap();

            CreateMap<OrderForSortingDto, Order>()
                .ForMember(dest => dest.OrderId,
                opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.Freight,
                opt => opt.MapFrom(src => src.Freight))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
