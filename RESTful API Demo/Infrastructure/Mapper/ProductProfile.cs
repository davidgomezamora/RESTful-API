using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.Product;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
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
