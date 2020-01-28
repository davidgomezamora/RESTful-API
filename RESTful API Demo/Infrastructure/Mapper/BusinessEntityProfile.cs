using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.BusinessEntity;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class BusinessEntityProfile : Profile
    {
        public BusinessEntityProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<BusinessEntity, BusinessEntityDto>().ReverseMap();
        }
    }
}
