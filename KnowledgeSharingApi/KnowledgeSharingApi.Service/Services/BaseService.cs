using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public abstract class BaseService<T>(IResourceFactory resourceFactory) : IService<T> where T : Entity
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

        public virtual async Task<ServiceResult> DeleteService(string id)
        {
            var repository = GetRepository();
            // Kiểm tra xem Entity với id có tồn tại?
            T? entity = await repository.Get(id);
            string msg;
            if (entity == null)
            {
                msg = CrudResource.NotExist(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMessage = msg,
                    DevMessage = msg,
                    StatusCode = EStatusCode.BadRequest
                };
            }

            // Tiến hành xóa Entity
            int res = await repository.Delete(id);
            if (res > 0)
            {
                return new ServiceResult(res);
            }
            msg = CrudResource.DeleteFailure(ResponseTableName);
            return new ServiceResult
            {
                IsSuccess = false,
                UserMessage = msg,
                DevMessage = msg,
                StatusCode = EStatusCode.ServerError
            };
        }

        public virtual async Task<ServiceResult> GetService(int? limit, int? offset)
        {
            var repository = GetRepository();
            List<T> entities;
            if (limit != null)
            {
                entities = (await repository.Get(limit ?? 0, offset ?? 0)).ToList();
            }
            else
            {
                entities = (await repository.Get()).ToList();
            }
            return new ServiceResult(entities.Count, entities);
        }

        public virtual async Task<ServiceResult> GetService(string id)
        {
            var repository = GetRepository();
            T? entity = await repository.Get(id);
            if (entity == null)
            {
                string msg = CrudResource.NotFound(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    StatusCode = EStatusCode.BadRequest,
                    UserMessage = msg,
                    DevMessage = msg
                };
            }
            return new ServiceResult(1, entity);
        }

        public virtual async Task<ServiceResult> GetService(string[] ids)
        {
            List<T> entities = (await GetRepository().Get()).ToList();
            var idProp = typeof(T).GetProperty($"{TableName}Id");
            if (idProp == null)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    StatusCode = EStatusCode.ServerError,
                    UserMessage = CrudResource.ServerError(),
                    DevMessage = CrudResource.NotExist($"{TableName}Id")
                };
            }
            List<T?> results = ids.Select(id =>
                entities.FirstOrDefault<T>(entity =>
                {
                    var id2 = idProp.GetValue(entity);
                    if (id2 == null)
                    {
                        return false;
                    }
                    return id2.ToString() == id;
                })
            ).ToList();
            return new ServiceResult
            {
                IsSuccess = true,
                StatusCode = EStatusCode.Success,
                UserMessage = CrudResource.GetMultiSuccess(ResponseTableName),
                DevMessage = CrudResource.GetMultiSuccess(ResponseTableName),
                Data = results
            };
        }

        public virtual async Task<ServiceResult> InsertService(T entity)
        {
            var repository = GetRepository();

            ValidateBeforeInsert(entity);

            // Insert Entity
            entity.CreatedBy = AuthorName;
            entity.CreatedDate = DateTime.Now;
            string? insertedId = await repository.Insert(entity);
            if (String.IsNullOrEmpty(insertedId))
            {
                string msg = CrudResource.InsertFailure(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMessage = msg,
                    DevMessage = msg,
                    StatusCode = EStatusCode.ServerError
                };
            }
            return new ServiceResult(1, insertedId);
        }

        public virtual async Task<ServiceResult> UpdateService(string id, T entity)
        {
            var repository = GetRepository();
            string msg;

            // check if customer is exist
            if (id == null)
            {
                msg = CrudResource.EmptyId(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMessage = msg,
                    DevMessage = msg,
                    StatusCode = EStatusCode.BadRequest
                };
            }
            T? ent = await repository.Get(id);
            if (ent == null)
            {
                msg = CrudResource.NotExist(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    UserMessage = msg,
                    DevMessage = msg,
                    StatusCode = EStatusCode.BadRequest
                };
            };

            ValidateBeforeUpdate(entity);

            // Update customer infor
            entity.ModifiedBy = ModifierName;
            entity.ModifiedDate = DateTime.Now;
            int res = await repository.Update(id, entity);
            if (res > 0)
            {
                return new ServiceResult(res);
            }
            msg = CrudResource.UpdateFailure(ResponseTableName);
            return new ServiceResult
            {
                IsSuccess = false,
                UserMessage = msg,
                DevMessage = msg,
                StatusCode = EStatusCode.ServerError
            };
        }

        public virtual async Task<ServiceResult> DeleteService(string[] ids)
        {
            var repository = GetRepository();
            int res = await repository.Delete(ids);
            string msg;
            if (res <= 0)
            {
                msg = CrudResource.DeleteMultiFailure(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = false,
                    StatusCode = EStatusCode.BadRequest,
                    UserMessage = msg,
                    DevMessage = msg
                };
            }
            if (res < ids.Length)
            {
                msg = CrudResource.DeletedSomeItems(ResponseTableName);
                return new ServiceResult
                {
                    IsSuccess = true,
                    StatusCode = EStatusCode.Success,
                    UserMessage = msg,
                    DevMessage = msg
                };
            }
            msg = CrudResource.DeleteMultiSuccess(ResponseTableName);
            return new ServiceResult
            {
                IsSuccess = true,
                StatusCode = EStatusCode.Success,
                UserMessage = msg,
                DevMessage = msg,
                Data = res
            };
        }

        public virtual async Task<ServiceResult> FilterService(string search, int? limit = null, int? offset = null)
        {
            List<T> entities;
            if (limit == null)
            {
                entities = (await GetRepository().Filter(search)).ToList();
            }
            else
            {
                entities = (await GetRepository().Filter(search, limit ?? 0, offset ?? 0)).ToList();
            }
            return new ServiceResult(1, entities);
        }
        #endregion
    }
}
