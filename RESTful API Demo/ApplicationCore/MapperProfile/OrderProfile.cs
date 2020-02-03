using AutoMapper;
using Common.DTO.Order;
using Infraestructure.Entities;
using Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.MapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Orders, OrderDto>()
                .ForMember(dest => dest.OrderId,
                opt => opt.MapFrom(src => new DataSecurity().AESEncrypt(src.OrderId.ToString())))
                .ForMember(dest => dest.CustomerId,
                opt => opt.MapFrom(src => new DataSecurity().AESEncrypt(src.CustomerId.ToString())))
                .ForMember(dest => dest.EmployeeId,
                opt => opt.MapFrom(src => new DataSecurity().AESEncrypt(src.EmployeeId.ToString())));

            CreateMap<OrderForAdditionDto, Orders>()
                .ForMember(dest => dest.EmployeeId,
                opt => opt.MapFrom(src => new DataSecurity().AESDescrypt(src.EmployeeId)));
        }
    }
}
