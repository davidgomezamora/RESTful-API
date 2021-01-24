using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApplicationCore.DTO.Order;
using ApplicationCore.ResourceParameters;
using Infrastructure.Entities;
using System.Dynamic;
using Common.DataService;

namespace ApplicationCore.Services
{
    public interface IOrderService : IBaseService<OrderDto, OrderForAdditionDto, OrderForUpdateDto, OrderForSortingDto, OrderResourceParameters>
    {
    }
}
