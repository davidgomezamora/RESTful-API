using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.DTO.Employee;
using AutoMapper;
using Infrastructure.Entities;

namespace ApplicationCore.MapperProfile
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Definición de como debe mapearse la entidad y el DTO
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<Employee, EmployeeForUpdateDto>();
            CreateMap<Employee, EmployeeForAdditionDto>();

            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<EmployeeForAdditionDto, Employee>();
            CreateMap<EmployeeForSortingDto, Employee>()
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
