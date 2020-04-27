using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Common.ResourceParameters;
using Common.DTO.Employee;
using System.Linq.Expressions;
using Security;
using Common.DTO.Order;
using Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Reflection;

namespace ApplicationCore.Services
{
    public abstract class  DatabaseService<TEntity>: IDatabaseService where TEntity : class
    {
        public IRepository<TEntity> _repository { get; set; }
        public IMapper _mapper { get; set; }

        // Interfaz de servicios de entidades secundarias

        // Inyección de los servicios
        public DatabaseService(/*IRepository<TEntity> repository,
            IDataSecurity dataSecurity,
            IMapper mapper*/)
        {
            /*this._repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this._dataSecurity = dataSecurity ??
                throw new ArgumentNullException(nameof(dataSecurity));

            this._mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));*/
        }

        private bool TrySetProperty(object obj, string propertyName, object value)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, value, null);
                return true;
            }

            return false;
        }

        private object? TryGetProperty(object obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.CanRead)
            {
                return propertyInfo.GetValue(obj, null);
            }

            return null;
        }

        private List<PropertyInfo> TryGetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }

        private async Task<EntityDto> AddAsync<EntityDto>(object id, object addition) where EntityDto : class
        {
            TEntity entity = this._mapper.Map<TEntity>(addition);
            this.TrySetProperty(entity, this._repository.PrimaryKeyName, id);

            if (this.TryGetProperty(entity, "").ToString() == id.ToString())
            {
                if (await this._repository.AddAsync(entity))
                {
                    return this._mapper.Map<EntityDto>(entity);
                }
            }

            return null;
        }

        public virtual async Task<EntityDto> AddAsync<EntityDto>(object addition) where EntityDto : class
        {
            TEntity entity = this._mapper.Map<TEntity>(addition);

            if (await this._repository.AddAsync(entity))
            {
                return this._mapper.Map<EntityDto>(entity);
            }

            return null;
        }

        public virtual async Task<List<EntityDto>> AddAsync<EntityDto, EmployeeForAdditionDto>(List<EmployeeForAdditionDto> additions) where EntityDto : class where EmployeeForAdditionDto : class
        {
            List<EntityDto> entityDtos = new List<EntityDto>();

            foreach (object addition in additions)
            {
                entityDtos.Add(await this.AddAsync<EntityDto>(addition));
            }

            return entityDtos;
        }

        public virtual async Task<bool> ExistsAsync(object id)
        {
            return await this._repository.ExistsAsync(id);
        }

        public virtual async Task<List<EntityDto>> GetAsync<EntityDto>(ResourceParameters resourceParameters)
        {
            // QueryParameters para paginación de resultados
            QueryParameters<TEntity> queryParameters = new QueryParameters<TEntity>();

            queryParameters.PageSize = resourceParameters.PageSize;
            queryParameters.PageNumber = resourceParameters.PageNumber;

            // Retorna el resultado con filtro y/o busqueda paginado
            return this._mapper.Map<List<EntityDto>>(await this._repository.FindByAsync(queryParameters));
        }

        public virtual async Task<EntityDto> GetAsync<EntityDto>(object id) where EntityDto : class
        {
            TEntity entity = await this._repository.GetByIdAsync(id);

            if (entity != null)
            {
                return this._mapper.Map<EntityDto>(entity);
            }

            return null;
        }

        public virtual async Task<ModelStateDictionary> PartiallyUpdateAsync<EntityForUpdateDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument) where EntityForUpdateDto : class
        {
            TEntity entity = await this._repository.GetByIdAsync(id);

            EntityForUpdateDto entityForUpdateDto = this._mapper.Map<EntityForUpdateDto>(entity);

            this._repository.DetachedEntity(entity);

            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
            jsonPatchDocument.ApplyTo(entityForUpdateDto, modelStateDictionary);

            if (modelStateDictionary.IsValid)
            {
                ValidationContext validationContext = new ValidationContext(entityForUpdateDto);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (Validator.TryValidateObject(entityForUpdateDto, validationContext, validationResults))
                {
                    if (!await this._UpdateAsync(id, entityForUpdateDto))
                    {
                        return null;
                    }
                }

                foreach (ValidationResult validationResult in validationResults)
                {
                    modelStateDictionary.AddModelError(validationResult.MemberNames.FirstOrDefault(), validationResult.ErrorMessage);
                }
            }

            return modelStateDictionary;
        }

        private async Task<Boolean> _UpdateAsync(object id, object update)
        {
            TEntity entity = this._mapper.Map<TEntity>(update);
            this.TrySetProperty(entity, this._repository.PrimaryKeyName, id);

            return await this._repository.UpdateAsync(entity);
        }

        public virtual async Task<Boolean> UpdateAsync(object id, object update)
        {
            TEntity entity = this._mapper.Map<TEntity>(update);
            this.TrySetProperty(entity, this._repository.PrimaryKeyName, id);

            return await this._repository.UpdateAsync(entity);
        }

        public virtual async Task<EntityDto> UpsertingAsync<EntityDto, EntityForAdditionDto>(object id, object update) where EntityDto : class
        {
            EntityForAdditionDto additionDto = this._mapper.Map<EntityForAdditionDto>(this._mapper.Map<TEntity>(update));

            return await this.AddAsync<EntityDto>(id, additionDto);
        }

        public virtual async Task<EntityDto> UpsertingAsync<EntityDto, EntityForUpdateDto, EntityForAdditionDto>(object id, JsonPatchDocument<EntityForUpdateDto> jsonPatchDocument, ModelStateDictionary modelStateDictionary) where EntityDto : class where EntityForUpdateDto : class, new()
        {
            EntityForUpdateDto entityForUpdateDto = new EntityForUpdateDto();
            jsonPatchDocument.ApplyTo(entityForUpdateDto, modelStateDictionary);

            if (modelStateDictionary.IsValid)
            {
                ValidationContext validationContext = new ValidationContext(entityForUpdateDto);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (Validator.TryValidateObject(entityForUpdateDto, validationContext, validationResults))
                {
                    EntityForAdditionDto entityForAdditionDto = this._mapper.Map<EntityForAdditionDto>(this._mapper.Map<TEntity>(entityForUpdateDto));

                    return await this.AddAsync<EntityDto>(id, entityForAdditionDto);
                }

                foreach (ValidationResult validationResult in validationResults)
                {
                    modelStateDictionary.AddModelError(validationResult.MemberNames.FirstOrDefault(), validationResult.ErrorMessage);
                }
            }

            return null;
        }
    }
}
