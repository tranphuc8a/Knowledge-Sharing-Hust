using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;

namespace KnowledgeSharingApi.Services.Services.Knowledges
{
    public class ImageService(
        IImageRepository imageRepository,
        IResourceFactory resourceFactory,
        IStorage storage
        ) : IImageService
    {
        public IImageRepository ImageRepository = imageRepository;
        public IResourceFactory ResourceFactory = resourceFactory;
        public IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        public IStorage Storage = storage;

        public string ImageResource = resourceFactory.GetEntityResource().Image();

        public async Task<ServiceResult> DeleteImage(Guid myUid, Guid imageId)
        {
            Image image = await ImageRepository.CheckExisted(imageId, ResponseResource.NotExist(ImageResource));

            if (image.UserId != myUid)
            {
                return ServiceResult.Forbidden("Đây không phải ảnh của bạn");
            }

            int raws = await ImageRepository.Delete(imageId);
            if (raws <= 0)
                return ServiceResult.ServerError(ResponseResource.DeleteFailure(ImageResource));
            return ServiceResult.Success(ResponseResource.DeleteSuccess(ImageResource));
        }

        public async Task<ServiceResult> GetListImage(Guid userId)
        {
            List<Image> res = await ImageRepository.GetByUserId(userId);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(ImageResource), string.Empty, res);
        }

        public async Task<ServiceResult> UploadImage(Guid myUid, UploadImageModel model)
        {
            // check giới hạn upload // lam sau

            // upload
            string? url = await Storage.SaveImage(model.Image!);
            if (string.IsNullOrEmpty(url))
            {
                return ServiceResult.ServerError("Upload image failed!");
            }

            // save to db:
            Image imageToAdd = new()
            {
                // Entity:
                CreatedBy = myUid.ToString(),
                CreatedTime = DateTime.UtcNow,
                // Image:
                ImageId = Guid.NewGuid(),
                UserId = myUid,
                ImageUrl = url
            };
            Guid? id = await ImageRepository.Insert(imageToAdd.ImageId, imageToAdd);
            if (id == null)
                return ServiceResult.ServerError("Upload image failed!");
            return ServiceResult.Success("Upload image success", string.Empty, imageToAdd);
        }
    }
}
