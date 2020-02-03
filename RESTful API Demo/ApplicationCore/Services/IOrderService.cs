using Common.DTO.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IOrderService
    {
        OrderDto AddOrder(OrderForAdditionDto orderForAdditionDto);
    }
}
