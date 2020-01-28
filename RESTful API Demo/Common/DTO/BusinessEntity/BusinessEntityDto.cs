using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.DTO.BusinessEntity
{
    public class BusinessEntityDto
    {
        [IgnoreDataMember]
        [JsonIgnore]
        public int BusinessEntityId { get; set; }
        public Guid Rowguid { get; set; }
    }
}
