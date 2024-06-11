using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.DecorationRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace KnowledgeSharingApi.Services.Services
{
    public class CategoryService : ICategoryService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        //protected readonly IDecorationRepository DecorationRepository;

        protected readonly string CategoryResource, CategoryExisted, CategoryNotExisted;
        protected readonly int DefaultLimit = 20;
        protected readonly int LimitNumberCategoryOfKnowledge = 5;

        public CategoryService(
            IResourceFactory resourceFactory,
            //IDecorationRepository decorationRepository,
            ICategoryRepository categoryRepository,
            IKnowledgeRepository knowledgeRepository
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();
            CategoryRepository = categoryRepository;
            KnowledgeRepository = knowledgeRepository;
            //DecorationRepository = decorationRepository;

            CategoryResource = EntityResource.Category();
            CategoryExisted = ResponseResource.ExistName(CategoryResource);
            CategoryNotExisted = ResponseResource.NotExist(CategoryResource);
        }

        public async Task<ServiceResult> AddCategory(string catName)
        {
            // Check catName chưa tồn tại
            Category? cat = await CategoryRepository.GetByName(catName);
            if (cat != null) return ServiceResult.BadRequest(CategoryExisted);

            // Thêm mới
            Category category = new()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = catName,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = "Knowledge Sharing admin"
            };
            Guid? catId = await CategoryRepository.Insert(category.CategoryId, category);
            if (catId == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(CategoryResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(CategoryResource));
        }

        public async Task<ServiceResult> DeleteCategory(Guid catId)
        {
            // Check cate đã tồn tại
            Category? cat = await CategoryRepository.Get(catId);
            if (cat == null) return ServiceResult.BadRequest(CategoryNotExisted);

            // Check category chưa gắn với knowledge nào
            List<ViewKnowledgeCategory> listKnowledges = await CategoryRepository.GetKnowledgesByCategory(catId);
            if (listKnowledges.Count != 0)
                return ServiceResult.BadRequest("Không thể xóa do có knowledge khác đang sử dụng category này");

            // Xóa 
            int res = await CategoryRepository.Delete(catId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CategoryResource));

            // Thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CategoryResource));
        }

        public async Task<ServiceResult> DeleteCategoryByName(string catName)
        {
            // Check cate đã tồn tại
            Category? cat = await CategoryRepository.GetByName(catName);
            if (cat == null) return ServiceResult.BadRequest(CategoryNotExisted);

            // Check category chưa gắn với knowledge nào
            List<ViewKnowledgeCategory> listKnowledges = await CategoryRepository.GetKnowledgesByCategory(catName);
            if (listKnowledges.Count != 0)
                return ServiceResult.BadRequest("Không thể xóa do có knowledge khác đang sử dụng category này");

            // Xóa 
            int res = await CategoryRepository.Delete(cat.CategoryId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CategoryResource));

            // Thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CategoryResource));
        }

        public async Task<ServiceResult> GetCategory(Guid catId)
        {
            // Get 
            Category? cat = await CategoryRepository.Get(catId);
            if (cat == null) return ServiceResult.BadRequest(CategoryNotExisted);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetSuccess(CategoryResource), string.Empty, cat);
        }

        public async Task<ServiceResult> GetCategory(string catName)
        {
            // Get 
            Category? cat = await CategoryRepository.GetByName(catName);
            if (cat == null) return ServiceResult.BadRequest(CategoryNotExisted);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetSuccess(CategoryResource), string.Empty, cat);
        }

        public async Task<ServiceResult> GetListCategories(PaginationDto pagination)
        {
            // Get
            PaginationResponseModel<Category> res = await CategoryRepository.Get(pagination);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CategoryResource), string.Empty, res.Results);
        }

        public async Task<ServiceResult> GetListCategoryOfKnowledge(Guid knowledgeId)
        {
            // Check knowledgeId tồn tại 
            // Không nhất thiết, trường hợp knowledge không tồn tại, trả về rỗng

            // Get 
            List<Category> res = await CategoryRepository.GetByKnowledgeId(knowledgeId);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CategoryResource), string.Empty, res);
        }

        public async Task<ServiceResult> UpdateCategory(Guid catId, string catName)
        {
            // Check cate đã tồn tại
            Category? cat = await CategoryRepository.Get(catId);
            if (cat == null) return ServiceResult.BadRequest(CategoryNotExisted);

            // Check category chưa gắn với knowledge nào
            List<ViewKnowledgeCategory> listKnowledges = await CategoryRepository.GetKnowledgesByCategory(catId);
            if (listKnowledges.Count != 0)
                return ServiceResult.BadRequest("Không thể cập nhật do có knowledge khác đang sử dụng category này");

            // Cập nhật
            cat.CategoryName = catName;
            cat.ModifiedBy = "PhucTV";
            cat.ModifiedTime = DateTime.UtcNow;
            int res = await CategoryRepository.Update(catId, cat);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(CategoryResource));

            // Thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CategoryResource), string.Empty, cat);
        }

        public async Task<ServiceResult> UpdateKnowledgeCategories(Guid myUid, UpdateKnowledgeCategoriesModel model)
        {
            // Check knowledge tồn tại và phải là của chính mình làm chủ
            _ = await KnowledgeRepository.CheckExisted(model.KnowledgeId!.Value,
                ResponseResource.NotExist(EntityResource.Knowledge()));

            // Kiểm tra danh sách category phải nhỏ hơn số phân loại:
            if (model.Categories!.Count > LimitNumberCategoryOfKnowledge)
            {
                return ServiceResult.BadRequest($"Mỗi phần tử kiến thức không được có quá {LimitNumberCategoryOfKnowledge} phân loại");
            }

            // Cập nhật danh sách cate của knowledge
            int rows = await CategoryRepository.UpdateKnowledgeCategories(model.KnowledgeId!.Value, model.Categories!);
            if (rows <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(CategoryResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CategoryResource));
        }
    }
}
