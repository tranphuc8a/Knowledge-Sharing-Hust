﻿using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class ImageService : IImageService
    {
        public IImageRepository ImageRepository;
        public IResourceFactory ResourceFactory;
        public IResponseResource ResponseResource;
        public IStorage Storage;

        public string ImageResource;

        public ImageService(
            IImageRepository imageRepository,
            IResourceFactory resourceFactory,
            IStorage storage
        )
        {
            ImageRepository = imageRepository;

            ResourceFactory = resourceFactory;
            ResponseResource = resourceFactory.GetResponseResource();

            Storage = storage;

            ImageResource = resourceFactory.GetEntityResource().Image();
        }


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
            IEnumerable<Image> res = await ImageRepository.GetByUserId(userId);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(ImageResource), string.Empty, res);
        }

        public async Task<ServiceResult> UploadImage(Guid myUid, UploadImageModel model)
        {
            // check giới hạn upload // lam sau

            // upload
            string? url = await Storage.SaveImage(model.Image!);
            if (String.IsNullOrEmpty(url))
            {
                return ServiceResult.ServerError("Upload image failed!");
            }

            // save to db:
            Image imageToAdd = new()
            {
                // Entity:
                CreatedBy = myUid.ToString(),
                CreatedTime = DateTime.Now,
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