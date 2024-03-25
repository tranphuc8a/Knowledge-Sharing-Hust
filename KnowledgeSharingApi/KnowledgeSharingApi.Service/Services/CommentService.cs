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
    public class CommentService : ICommentService
    {
        public Task<ServiceResult> AdminBlockCommentOfKnowledge(Guid knowledgeId, bool isBlock)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminDeleteComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AdminGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AnonymousGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetListCommentReplies(Guid commentId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserAddComment(Guid myuid, CreateCommentModel commentModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserBlockKnowledgeComments(Guid myuid, Guid knowledgeId, bool isBlock)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserDeleteComment(Guid myuid, Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetKnowledgeComments(Guid myuid, Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetListKnowledgeComments(Guid myuid, Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetMyCommentsOfKnowledge(Guid myuid, Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserGetUserCommentsOfKnowledge(Guid myuid, Guid userId, Guid knowledgeId, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserReplyComment(Guid myuid, ReplyCommentModel replyModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserSearchCommentsOfKnowledge(Guid myuid, Guid knowledgeId, string search, int? limit, int? offset)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserUpdateComment(Guid myuid, UpdateCommentModel commentModel)
        {
            throw new NotImplementedException();
        }
    }
}
