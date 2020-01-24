using Common.DTO.Customer;
using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface ICustomerService
    {
        CustomerDto GetCustomer<T>(T productModelId);
    }
}
