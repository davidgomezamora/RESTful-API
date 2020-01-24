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
        List<StoreDto> GetStores(StoreResourceParameters productResourceParameters);
        StoreDto GetStore(Guid rowguid);
        List<CustomerDto> GetCustomers(Guid rowguid);
    }
}
