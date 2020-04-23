﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.DTO.Employee;
using Infraestructure.Entities;
using Security;

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

            CreateMap<EmployeeForAdditionDto, Employees>();
            CreateMap<EmployeeForUpdateDto, Employees>();
        }
    }
}
