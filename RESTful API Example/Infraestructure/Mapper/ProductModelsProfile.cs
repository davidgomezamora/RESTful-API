using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class ProductModelsProfile : Profile
    {
        public ProductModelsProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<ProductModel, ProductModelDto>().ReverseMap();
        }
    }
}
