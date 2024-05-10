using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CommentService : ICommentService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;

        protected readonly IDecorationRepository DecorationRepository;
        protected readonly ICommentRepository CommentRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        //protected readonly IStarRepository StarRepository;

        protected readonly string CommentResource, KnowledgeResource;
        protected readonly string NotExistedComment, NotExistKnowledge;
        protected readonly int DefaultLimit = 20;


        public CommentService(
            IResourceFactory resourceFactory,
            ICommentRepository commentRepository,
            IKnowledgeRepository knowledgeRepository,
            IStarRepository starRepository,
            IDecorationRepository decorationRepository
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
            EntityResource = ResourceFactory.GetEntityResource();

            CommentRepository = commentRepository;
            KnowledgeRepository = knowledgeRepository;
            DecorationRepository = decorationRepository;
            //StarRepository = starRepository;

            CommentResource = EntityResource.Comment();
            KnowledgeResource = EntityResource.Knowledge();
            NotExistedComment = ResponseResource.NotExist(CommentResource);
            NotExistKnowledge = ResponseResource.NotExist(KnowledgeResource);
        }
        
        #region Functionality Methods

        /// <summary>
        /// Tạo mới comment từ các thông tin input của api
        /// </summary>
        /// <param name="myUid"> id của user là chủ comment </param>
        /// <param name="commentModel"> Nội dung bình luận </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        protected virtual Comment CreateNewComment(Guid myUid, CreateCommentModel commentModel)
        {
            Guid newId = Guid.NewGuid();
            Comment comment = new()
            {
                // Entity:
                CreatedTime = DateTime.Now,
                ModifiedBy = myUid.ToString(),
                // UserItem:
                UserItemId = newId,
                UserItemType = EUserItemType.Comment,
                UserId = myUid,
                // Comment:
                KnowledgeId = commentModel.KnowledgeId!.Value,
                Content = commentModel.Content!,
                ReplyId = null
            };
            return comment;
        }

        /// <summary>
        /// Tạo mới comment từ các thông tin input của api
        /// </summary>
        /// <param name="myUid"> id của user là chủ comment </param>
        /// <param name="commentModel"> Nội dung bình luận </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        protected virtual Comment CreateNewComment(Guid myUid, ReplyCommentModel commentModel, Guid knowledgeId)
        {
            Guid newId = Guid.NewGuid();
            Comment comment = new()
            {
                // Entity:
                CreatedTime = DateTime.Now,
                ModifiedBy = myUid.ToString(),
                // UserItem:
                UserItemId = newId,
                UserItemType = EUserItemType.Comment,
                UserId = myUid,
                // Comment:
                KnowledgeId = knowledgeId,
                Content = commentModel.Content!,
                ReplyId = commentModel.ReplyId
            };
            return comment;
        }

        protected virtual bool IsSimiliar(string commentContent, string searchKey)
        {
            // Chuyển đổi cả hai chuỗi thành chữ thường
            commentContent = commentContent.ToLowerInvariant();
            searchKey = searchKey.ToLowerInvariant();

            // Loại bỏ khoảng trống đầu và cuối chuỗi
            commentContent = commentContent.Trim();
            searchKey = searchKey.Trim();

            // Kiểm tra xem chuỗi searchKey có xuất hiện trong commentContent không
            if (commentContent.Contains(searchKey))
                return true;

            // Sử dụng độ tương đồng Levenshtein distance để xác định mức độ tương tự
            // Coi là tương đồng nếu khoảng cách Levenshtein nhỏ hơn hoặc bằng 20% chiều dài của searchKey
            int levenshteinDistance = Algorithm.CalculateLevenshteinDistance(commentContent, searchKey);
            return levenshteinDistance <= (searchKey.Length * 0.2);
        }
        #endregion

        #region Admin and Anonymous actors
        public virtual async Task<ServiceResult> AdminBlockCommentOfKnowledge(Guid knowledgeId, bool isBlock)
        {
            // Kiểm tra knowledge tồn tại
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);

            // Update block
            knowledge.IsBlockComment = isBlock;
            await KnowledgeRepository.Update(knowledge.UserItemId, knowledge);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(KnowledgeResource));
        }

        public virtual async Task<ServiceResult> AdminDeleteComment(Guid commentId)
        {
            // Check comment existed
            Comment comment = await CommentRepository.CheckExisted(commentId, NotExistedComment);

            // Delete comment
            int res = await CommentRepository.Delete(comment.UserItemId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CommentResource));

            // Return success
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CommentResource));
        }

        public virtual async Task<ServiceResult> AdminGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset)
        {
            // Check knowledge existed
            _ = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);

            // Get list comments
            PaginationResponseModel<ViewComment> comments =
                await KnowledgeRepository.GetListComments(knowledgeId, limit ?? DefaultLimit, offset ?? 0);

            // Return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, comments);
        }

        public virtual async Task<ServiceResult> AnonymousGetListKnowledgeComments(Guid knowledgeId, int? limit, int? offset)
        {
            // Kiểm tra knowledge tồn tại
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);

            // Kiểm tra knowledge public
            if (knowledge.Privacy != EPrivacy.Public)
                return ServiceResult.Forbidden("Phần tử kiến thức không công khai");

            // Lấy về ds bình luận
            PaginationResponseModel<ViewComment> listComments =
                await KnowledgeRepository.GetListComments(knowledgeId, limit ?? DefaultLimit, offset ?? 0);

            // Trả về thành công
            PaginationResponseModel<IResponseCommentModel> res = new()
            {
                Total = listComments.Total,
                Limit = listComments.Limit,
                Offset = listComments.Offset,
                Results = (await DecorationRepository
                    .DecorateResponseCommentModel(null, listComments.Results.ToList()))
                    .OfType<IResponseCommentModel>().ToList()
            };
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, res);
        }

        public async Task<ServiceResult> GetComment(Guid commentId)
        {
            // Check comment tồn tại
            ViewComment comment = await CommentRepository.CheckExistedComment(commentId, NotExistedComment);

            // Trang trí comment
            IResponseCommentModel? res = 
                (await DecorationRepository.DecorateResponseCommentModel(null, [comment], true))
                .FirstOrDefault();
            if (res == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetSuccess(CommentResource), string.Empty, res);
        }

        public async Task<ServiceResult> GetListCommentReplies(Guid commentId, int? limit, int? offset)
        {
            // Check comment existed
            _ = await CommentRepository.CheckExistedComment(commentId, NotExistedComment);

            // Get list replies
            PaginationResponseModel<ViewComment> comments =
                await CommentRepository.GetRepliesOfComment(commentId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel
            PaginationResponseModel<IResponseCommentModel> res = new()
            {
                Total = comments.Total,
                Limit = comments.Limit,
                Offset = comments.Offset,
                Results = (await DecorationRepository
                    .DecorateResponseCommentModel(null, comments.Results.ToList(), isDecorateReplies: false))
                    .OfType<IResponseCommentModel>().ToList()
            };

            // return success
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, res);
        }
        #endregion

        #region User Actors

        public async Task<ServiceResult> UserAddComment(Guid myUid, CreateCommentModel commentModel)
        {
            // Kiểm tra quyền truy cập user -> knowledge
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(commentModel.KnowledgeId ?? Guid.Empty, NotExistKnowledge);
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledge.UserItemId);
            if (!isAccessible) return ServiceResult.Forbidden("Bạn không có quyền truy cập bài viết này");

            // Kiểm tra knowledge đang mở cho phép bình luận
            if (knowledge.IsBlockComment)
                return ServiceResult.Forbidden("Bài viết hiện đang bị khóa bình luận, vui lòng thử lại sau");

            // Thêm mới comment
            Comment comment = CreateNewComment(myUid, commentModel);
            Guid? res = await CommentRepository.Insert(comment.UserItemId, comment);
            if (res == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(CommentResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(CommentResource), string.Empty, comment);
        }

        public async Task<ServiceResult> UserBlockKnowledgeComments(Guid myUid, Guid knowledgeId, bool isBlock)
        {
            // User phải là owner của knowledge
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);
            if (knowledge.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải chủ bài viết");

            // Cập nhật block, trả về thành công
            knowledge.IsBlockComment = isBlock;
            await KnowledgeRepository.Update(knowledge.UserItemId, knowledge);
            return ServiceResult.Success(ResponseResource.UpdateSuccess(KnowledgeResource));
        }

        public async Task<ServiceResult> UserDeleteComment(Guid myUid, Guid commentId)
        {
            // Kiểm tra user là chủ của comment hoặc comment đang nằm trong knowledge mà user đang là chủ
            ViewComment comment = await CommentRepository.CheckExistedComment(commentId, NotExistedComment);
            bool isDeletable = false;
            if (comment.UserId == myUid)
            {   // Comment này là của chủ myUid
                isDeletable = true;
            }
            else
            {   // Check comment nằm trong bài mà myUid là chủ
                Knowledge? knowledge = await KnowledgeRepository.Get(comment.KnowledgeId);
                if (knowledge != null && knowledge.UserId == myUid)
                {   // OK cho phép xóa comment
                    isDeletable = true;
                }
            }
            if (!isDeletable)
                return ServiceResult.Forbidden("Bạn không có quyền xóa comment này");

            // Xóa comment
            int res = await CommentRepository.Delete(commentId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(CommentResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(CommentResource));
        }

        public async Task<ServiceResult> UserGetListKnowledgeComments(Guid myUid, Guid knowledgeId, int? limit, int? offset)
        {
            // Kiểm tra user có quyền truy cập tài nguyên knowledge
            _ = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledgeId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập bài viết này");

            // Lấy về danh sách comment
            PaginationResponseModel<ViewComment> comments =
                await KnowledgeRepository.GetListComments(knowledgeId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel comments
            PaginationResponseModel<IResponseCommentModel> res = new()
            {
                Total = comments.Total,
                Limit = comments.Limit,
                Offset = comments.Offset,
                Results = await DecorationRepository
                    .DecorateResponseCommentModel(myUid, comments.Results.ToList(), isDecorateReplies: true)
            };

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, res);
        }

        public Task<ServiceResult> UserGetMyCommentsOfKnowledge(Guid myUid, Guid knowledgeId, int? limit, int? offset)
        {
            return UserGetUserCommentsOfKnowledge(myUid, myUid, knowledgeId, limit, offset);
        }

        public async Task<ServiceResult> UserGetUserCommentsOfKnowledge(Guid myUid, Guid userId, Guid knowledgeId, int? limit, int? offset)
        {
            // Kiểm tra myUid có truy cập được knowledgeId hay không
            _ = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledgeId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập vào bài viết này");

            // Lấy về danh sách bình luận
            PaginationResponseModel<ViewComment> comments =
                await CommentRepository.GetCommentsOfUserInKnowledge(userId, knowledgeId, limit ?? DefaultLimit, offset ?? 0);

            // DecorateResponseLessonModel bình luận
            PaginationResponseModel<IResponseCommentModel> res = new()
            {
                Total = comments.Total,
                Limit = comments.Limit,
                Offset = comments.Offset,
                Results = await DecorationRepository
                    .DecorateResponseCommentModel(myUid, comments.Results.ToList(), isDecorateReplies: true)
            };

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserReplyComment(Guid myUid, ReplyCommentModel replyModel)
        {
            // Kiểm tra tồn tại Comment và knowledge
            ViewComment comment = await CommentRepository.CheckExistedComment(replyModel.ReplyId ?? Guid.Empty, NotExistedComment);
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(comment.KnowledgeId, NotExistKnowledge);

            // Kiểm tra quyền truy cập vào knowledge
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledge.UserItemId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập được bài viết này");

            // Kiểm tra Comment level 0 (chưa reply comment nào khác)
            if (comment.ReplyId != null)
                return ServiceResult.BadRequest("Chỉ có thể reply comment gốc của bài viết");

            // OK thêm mới comment
            Comment commentToAdd = CreateNewComment(myUid, replyModel, knowledge.UserItemId);
            Guid? res = await CommentRepository.Insert(commentToAdd.UserItemId, commentToAdd);
            if (res == null) return ServiceResult.ServerError(ResponseResource.InsertFailure(CommentResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.InsertSuccess(CommentResource));

        }

        public async Task<ServiceResult> UserSearchCommentsOfKnowledge(Guid myUid, Guid knowledgeId, string search, int? limit, int? offset)
        {
            // Check từ khóa khác rỗng
            if (String.IsNullOrEmpty(search))
                return ServiceResult.BadRequest("Từ khóa rỗng");

            // Check accessible
            _ = await KnowledgeRepository.CheckExisted(knowledgeId, NotExistKnowledge);
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledgeId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập bài viết này");

            // Search
            // Lấy về toàn bộ
            IEnumerable<ViewComment> listComments = await KnowledgeRepository.GetListComments(knowledgeId);
            int limitValue = limit ?? DefaultLimit, offsetValue = offset ?? 0;

            // Lọc theo từ khóa
            listComments = listComments.Where(comment => IsSimiliar(comment.Content, search))
                .Skip(offsetValue).Take(limitValue);

            // DecorateResponseLessonModel
            IEnumerable<IResponseCommentModel> res = await DecorationRepository
                .DecorateResponseCommentModel(myUid, listComments.ToList(), isDecorateReplies: true);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(CommentResource), string.Empty, res);
        }

        public async Task<ServiceResult> UserUpdateComment(Guid myUid, Guid commentId, UpdateCommentModel commentModel)
        {
            // Check comment tồn tại, knowledge tồn tại
            ViewComment comment = await CommentRepository.CheckExistedComment(commentId, NotExistedComment);
            ViewComment editableComment = (ViewComment) comment.Clone();
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(comment.KnowledgeId, NotExistKnowledge);

            // Check accessible to knowledge
            bool isAccessible = await KnowledgeRepository.CheckAccessible(myUid, knowledge.UserItemId);
            if (!isAccessible)
                return ServiceResult.Forbidden("Bạn không có quyền truy cập bài viết này");

            // Update
            editableComment.Content = commentModel.Content!;
            editableComment.ModifiedBy = myUid.ToString();
            editableComment.ModifiedTime = DateTime.Now;
            Comment commentToUpdate = new();
            commentToUpdate.Copy(editableComment);
            int res = await CommentRepository.Update(editableComment.UserItemId, commentToUpdate);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(CommentResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess(CommentResource), string.Empty, editableComment);
        } 
        #endregion
    }
}
