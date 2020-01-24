using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.Customer;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
