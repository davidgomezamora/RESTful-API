using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.NameNumber,
                    opt => opt.MapFrom(src => src.Name + "-" + src.ProductNumber)
                ).ReverseMap();
        }
    }
}
