using Common.DTO.Store;
using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;
using Common.DTO.Customer;

namespace ApplicationCore.Services
{
    public interface IStoreService
    {
        List<EmployeeDto> GetStores(StoreResourceParameters storeResourceParameters);
        EmployeeDto GetStore(Guid rowguid);
        List<CustomerDto> GetCustomers(Guid rowguid);

        EmployeeDto AddStore(EmployeeForAdditionDto storeForAdditionDto);
    }
}
