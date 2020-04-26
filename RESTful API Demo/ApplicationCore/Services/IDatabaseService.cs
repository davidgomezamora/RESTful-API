using Common.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Text;
using Common.DTO.Employee;
using Common.DTO.Order;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository;

namespace ApplicationCore.Services
{
    public interface IDatabaseService
    {
        Task<EntityDto> AddAsync<EntityDto>(object addition) where EntityDto : class;
        Task<List<EntityDto>> AddAsync<EntityDto, EntityForAdditionDto>(List<EntityForAdditionDto> additions) where EntityDto : class where EntityForAdditionDto : class;
        Task<bool> ExistsAsync(object id);
        Task<List<EntityDto>> GetAsync<EntityDto>(ResourceParameters resourceParameters);
        Task<EntityDto> GetAsync<EntityDto>(object id) where EntityDto : class;
        Task<ModelStateDictionary> PartiallyUpdateAsync<EntityForUpdateDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument) where EntityForUpdateDto : class;
        Task<Boolean> UpdateAsync(object id, object update);
        Task<EntityDto> UpsertingAsync<EntityDto, EntityForAdditionDto>(object id, object update) where EntityDto : class;
        Task<EntityDto> UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument, ModelStateDictionary modelStateDictionary) where EntityDto : class where EntityForUpdateDto : class, new();
    }
}
