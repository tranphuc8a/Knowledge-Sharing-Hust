using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class LessonService : ILessonService
    {
        public Task<ServiceResult> AdminDeletePost(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetListPostsOfCourse(Guid courseId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetPostDetail(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetPosts(int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetUserPosts(Guid userId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AnonymousGetListPostsOfCategory(string catName, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AnonymousGetPostDetail(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AnonymousGetPosts(int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AnonymousGetUserPosts(Guid userId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserCreatePost(Guid myUid, CreatePostModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserDeletePost(Guid myUid, Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetListPostsOfCategory(Guid myUid, string catName, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetListPostsOfCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetListPostsOfMyCourse(Guid myUid, Guid courseId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetMyPostDetail(Guid myUid, Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetMyPosts(Guid myUid, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetPostDetail(Guid myUid, Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetPosts(Guid myUid, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetUserPosts(Guid myUid, Guid userId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserUpdatePost(Guid myUid, Guid postId, UpdatePostModel model)
        {
            throw new NotImplementedException();
        }
    }
}
