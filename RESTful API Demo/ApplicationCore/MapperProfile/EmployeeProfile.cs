using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.DTO.Employee;
using AutoMapper;
using Common.Security;
using Infraestructure.Entities;

namespace ApplicationCore.MapperProfile
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Employees, EmployeeDto>()
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.EmployeeId,
                opt => opt.MapFrom(src => new DataSecurity().AESEncrypt(src.EmployeeId.ToString())));
            CreateMap<Employees, EmployeeForUpdateDto>();
            CreateMap<Employees, EmployeeForAdditionDto>();

            CreateMap<EmployeeForUpdateDto, Employees>();
            CreateMap<EmployeeForAdditionDto, Employees>();
            CreateMap<EmployeeForSortingDto, Employees>()
                .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.FullName))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
