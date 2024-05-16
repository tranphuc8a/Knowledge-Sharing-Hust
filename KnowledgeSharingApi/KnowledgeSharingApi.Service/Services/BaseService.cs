using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Interfaces;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public abstract class BaseService<T>(IResourceFactory resourceFactory) : IEntityService<T> where T : Entity
    {
        #region Attributes and Constructor
        protected readonly IResourceFactory _ResourceFactory = resourceFactory;
        protected readonly IResponseResource CrudResource = resourceFactory.GetResponseResource();
        protected readonly IValidatorResource validatorResource = resourceFactory.GetValidatorResource();
        protected readonly IEntityResource EntityResource = resourceFactory.GetEntityResource();

        protected string TableName = typeof(T).Name;
        protected string ResponseTableName = "Entity";
        protected string AuthorName = "PHUCTV";
        protected string ModifierName = "PHUCTV";
        #endregion


        #region Abstract Steps of Template Methods

        /// <summary>
        /// Factory Method để lấy ra Repository tương ứng làm việc
        /// </summary>
        /// <returns> Repository của Service tương ứng </returns>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        protected abstract IRepository<T> GetRepository();


        /// <summary>
        /// Template method thực hiện validate dữ liệu trước khi insert
        /// </summary>
        /// <param name="entity"> Giá trị cần validate </param>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        protected abstract void ValidateBeforeInsert(T entity);


        /// <summary>
        /// Template method thực hiện validate dữ liệu trước khi update
        /// </summary>
        /// <param name="entity"> Giá trị cần validate </param>
        /// Created: PhucTV (8/1/24)
        /// Modified: None
        protected abstract void ValidateBeforeUpdate(T entity);

        #endregion


        #region Template Services

        public virtual async Task<ServiceResult> DeleteService(Guid id)
        {
            var repository = GetRepository();
            // Kiểm tra xem Entity với id có tồn tại?
            T? entity = await repository.Get(id);
            string msg;
            if (entity == null)
            {
                msg = CrudResource.NotExist(ResponseTableName);
                return ServiceResult.BadRequest(msg);
            }

            // Tiến hành xóa Entity
            int res = await repository.Delete(id);
            if (res > 0)
            {
                return new ServiceResult(res);
            }
            msg = CrudResource.DeleteFailure(ResponseTableName);
            return ServiceResult.ServerError(msg);
        }

        public virtual async Task<ServiceResult> GetService(PaginationDto pagination)
        {
            var repository = GetRepository();
            PaginationResponseModel<T> paginationResponseModel = await repository.Get(pagination);

            // Success:
            return new ServiceResult(paginationResponseModel.Count ?? 0, paginationResponseModel);
        }

        public virtual async Task<ServiceResult> GetService(Guid id)
        {
            var repository = GetRepository();
            T? entity = await repository.Get(id);
            if (entity == null)
            {
                string msg = CrudResource.NotFound(ResponseTableName);
                return ServiceResult.BadRequest(msg);
            }
            // Success
            return new ServiceResult(1, entity);
        }

        public virtual async Task<ServiceResult> GetService(Guid[] ids)
        {
            List<T> entities = (await GetRepository().Get()).ToList();
            var idProp = typeof(T).GetProperty($"{TableName}Id");
            if (idProp == null)
                return ServiceResult.ServerError(
                    UserMessage: CrudResource.ServerError(),
                    DevMessage: CrudResource.NotExist($"{TableName}Id"));
            List<T?> results = ids.Select(id =>
                entities.FirstOrDefault<T>(entity =>
                {
                    var id2 = idProp.GetValue(entity);
                    if (id2 != null && Guid.TryParse(id2.ToString(), out Guid value))
                        return id == value;
                    return false;
                })
            ).ToList();
            // Success:
            string msg = CrudResource.GetMultiSuccess(ResponseTableName);
            return ServiceResult.Success(msg, msg, results);
        }

        public virtual async Task<ServiceResult> InsertService(T entity)
        {
            var repository = GetRepository();

            ValidateBeforeInsert(entity);

            // Insert Entity
            entity.CreatedBy = AuthorName;
            entity.CreatedTime = DateTime.Now;
            Guid? insertedId = await repository.Insert(entity);
            if (insertedId == null)
            {
                string msg = CrudResource.InsertFailure(ResponseTableName);
                return ServiceResult.ServerError(msg);
            }
            // Success
            return new ServiceResult(1, insertedId);
        }

        public virtual async Task<ServiceResult> UpdateService(Guid id, T entity)
        {
            var repository = GetRepository();
            string msg;

            // check if entity is exist
            T? ent = await repository.Get(id);
            if (ent == null)
            {
                msg = CrudResource.NotExist(ResponseTableName);
                return ServiceResult.BadRequest(msg);
            };

            ValidateBeforeUpdate(entity);

            // Update entity infor
            entity.ModifiedBy = ModifierName;
            entity.ModifiedTime = DateTime.Now;
            int res = await repository.Update(id, entity);
            if (res > 0)
            {
                // Success
                return new ServiceResult(res);
            }
            msg = CrudResource.UpdateFailure(ResponseTableName);
            return ServiceResult.ServerError(msg);
        }

        public virtual async Task<ServiceResult> DeleteService(Guid[] ids)
        {
            var repository = GetRepository();
            int res = await repository.Delete(ids);
            string msg;
            if (res <= 0)
            {
                msg = CrudResource.DeleteMultiFailure(ResponseTableName);
                return ServiceResult.BadRequest(msg);
            }
            
            // Success
            msg = CrudResource.DeleteMultiSuccess(ResponseTableName);
            if (res < ids.Length)
            {
                msg = CrudResource.DeletedSomeItems(ResponseTableName);
            }
            return ServiceResult.Success(msg);
        }

        public virtual async Task<ServiceResult> FilterService(string search, PaginationDto pagination)
        {
            var repository = GetRepository();
            PaginationResponseModel<T> paginationResponseModel = await repository.Filter(search, pagination);
            
            // Success
            return new ServiceResult(paginationResponseModel.Count ?? 0, paginationResponseModel);

        }
        #endregion
    }
}
