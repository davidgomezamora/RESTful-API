using Common.DTO.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Services
{
    public interface IBusinessEntityService
    {
        BusinessEntityDto GetBusinessEntity(Guid rowguid);
    }
}
