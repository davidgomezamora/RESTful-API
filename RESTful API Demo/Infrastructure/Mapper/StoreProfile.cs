using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.Store;
using Infraestructure.Entities;

namespace Common.Mapper
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Store, EmployeeDto>()
                .ForMember(
                    dest => dest.ConcatenatedData,
                    opt => opt.MapFrom(src => src.Name + "-" + src.BusinessEntityId)
                ).ReverseMap();

            CreateMap<EmployeeForAdditionDto, Store>()
                .ForMember(
                    dest => dest.BusinessEntity,
                    opt => opt.Ignore()
                ).ReverseMap();
        }
    }
}
