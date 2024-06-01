using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class ConversationService(
        IConversationRepository conversationRepository,
        IUserRepository userRepository,
        IUserConversationRepository userConversationRepository,
        IMessageRepository messageRepository,
        IStorage storage,
        IResourceFactory resourceFactory,
        IUnitOfWork unitOfWork
    ) : IConversationService
    {
        protected readonly IConversationRepository ConversationRepository = conversationRepository;
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IMessageRepository MessageRepository = messageRepository;
        protected readonly IStorage Storage = storage;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IUserConversationRepository UserConversationRepository = userConversationRepository;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;
        protected readonly string ConversationResource = resourceFactory.GetEntityResource().Conversation();
        protected readonly int DefaultLimit = 20; // Lấy top 20 tin nhắn mới nhất

        #region Functional methods
        /// <summary>
        /// Hàm lấy về thông tin chi tiết của cuộc trò chuyện, đảm bảo cuộc trò chuyện tồn tại
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ResponseConversationModel> GetConversationDetail(Guid myUid, Conversation conversation)
        {
            // Lấy về cuộc hội thoại
            //Conversation conv = await ConversationRepository.Get(conversationId)
            //    ?? throw new ValidatorException(ResponseResource.NotFound(ConversationResource));

            // Lấy về danh sách participant
            List<ResponseParticipantModel> listParticipants =
                (await ConversationRepository.GetParticipants(conversation.ConversationId))
                .Select(participant => (new ResponseParticipantModel().Copy(participant) as ResponseParticipantModel)!)
                .ToList();

            // Lấy về top tin nhắn
            //List<ResponseMessageModel> topMessages =
            //    (await ConversationRepository.GetMessages(myUid, conversationId, new PaginationDto()
            //    {
            //        Limit = DefaultLimit,
            //        Offset = 0,
            //        Orders = null,
            //        Filters = null
            //    }))
            //    .Select(message => (new ResponseMessageModel().Copy(message) as ResponseMessageModel)!)
            //    .ToList();


            ResponseConversationModel model = new();
            model.Copy(conversation);
            model.Participants = listParticipants;
            model.Messages = [];

            return model;
        }

        /// <summary>
        /// Kiểm tra user tồn tại
        /// </summary>
        /// <param name="userId"> id của user kiểm tra </param>
        /// <returns> Trả về user </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ViewUser> CheckExistedUser(Guid userId)
        {
            return (ViewUser) ((await UserRepository.GetDetail(userId))?.Clone()
                ?? throw new ValidatorException(ResponseResource.NotFound(ResourceFactory.GetEntityResource().User())));
        }

        /// <summary>
        /// Kiểm tra Cuộc trò chuyện tồn tại
        /// </summary>
        /// <param name="id"> id của cuộc trò chuyện kiểm tra </param>
        /// <returns> Trả về cuộc trò chuyện </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<Conversation> CheckExistedConversation(Guid id)
        {
            return (Conversation) ((await ConversationRepository.Get(id))?.Clone()
                ?? throw new ValidatorException(ResponseResource.NotFound(ConversationResource)));
        }

        /// <summary>
        /// Kiểm tra tin nhắn tồn tại
        /// </summary>
        /// <param name="id"> id của tin nhắn kiểm tra </param>
        /// <returns> Trả về tin nhắn </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ViewMessage> CheckExistedMessage(Guid id)
        {
            return (ViewMessage) ((await MessageRepository.GetDetail(id))?.Clone()
                ?? throw new ValidatorException(ResponseResource.NotFound(ResourceFactory.GetEntityResource().Message())));
        }

        /// <summary>
        /// Tạo mới cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> id của người thứ nhất </param>
        /// <param name="uid"> id của người thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ServiceResult> CreateNewConversation(Guid user_1, Guid user_2)
        {
            Conversation? conversation = await ConversationRepository.CreateConversation(user_1, user_2);
            if (conversation == null)
                return ServiceResult.BadRequest(ResponseResource.Failure());

            // Trả về cuộc trò chuyện
            return ServiceResult.Success(
                ResponseResource.Success(),
                string.Empty,
                await GetConversationDetail(user_1, conversation)
            );
        }

        #endregion


        #region Get Conversation
        public virtual async Task<ServiceResult> GetConversation(Guid myUId, Guid conversationId)
        {
            // Kiểm tra userId phải tồn tại
            //ViewUser user = await CheckExistedUser(myUId);

            // Kiểm tra conversation phải tồn tại
            Conversation conversation = await CheckExistedConversation(conversationId);

            // Lấy về thông tin chi tiết conservation
            ResponseConversationModel model = await GetConversationDetail(myUId, conversation);

            // Kiểm tra conversation phải chứa myUid
            if (!model.Participants.Any(participant => participant.UserId == myUId))
                return ServiceResult.Forbidden("Bạn chưa tham gia cuộc hội thoại này");


            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetSuccess(ConversationResource), string.Empty, model);
        }

        public virtual async Task<ServiceResult> GetConversations(Guid myUid, PaginationDto pagination)
        {
            // Kiểm tra user tồn tại
            //await CheckExistedUser(myUid);

            // Lấy về danh sách cuộc trò chuyện
            List<Conversation> conversations = await ConversationRepository.GetListConversationByUserId(myUid);
            conversations = ConversationRepository.ApplyPagination(conversations, pagination);

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.GetMultiSuccess(ConversationResource),
                DevMessage: string.Empty,
                new PaginationResponseModel<Conversation>()
                {
                    Total = conversations.Count,
                    Limit = conversations.Count,
                    Offset = 0,
                    Results = conversations
                }
            );
        }

        public virtual async Task<ServiceResult> GetConversationWith(Guid myUid, Guid uid)
        {
            //ViewUser user_1 = await CheckExistedUser(myUid);
            //_ = await CheckExistedUser(uid);

            Conversation? conversation = await ConversationRepository.GetConversationWithUser(myUid, uid);
            if (conversation == null)
                return ServiceResult.NotFound(ResponseResource.NotFound(ConversationResource));

            return ServiceResult.Success(
                ResponseResource.GetSuccess(ConversationResource),
                string.Empty,
                await GetConversationDetail(myUid, conversation)
            );
        }

        #endregion

        #region Conversation Operations
        public virtual async Task<ServiceResult> StartConversation(Guid myUid, Guid uid)
        {
            //ViewUser user_1 = await CheckExistedUser(myUid);
            //ViewUser user_2 = await CheckExistedUser(uid);

            Conversation? conversation = await ConversationRepository.GetConversationWithUser(myUid, uid);
            if (conversation != null)
            {
                return ServiceResult.Success(
                    ResponseResource.InsertSuccess(ConversationResource),
                    string.Empty,
                    await GetConversationDetail(myUid, conversation)
                );
            }

            return await CreateNewConversation(myUid, uid);
        }

        public virtual async Task<ServiceResult> DeleteConversation(Guid myUid, Guid conversationId)
        {
            //ViewUser user = await CheckExistedUser(myUid);
            _ = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            ViewUserConversation userConversation = await ConversationRepository.GetParticipant(myUid, conversationId)
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Update Last Delete
            userConversation.LastDeleteTime = DateTime.UtcNow;
            UserConversation toUpdate = new();
            toUpdate.Copy(userConversation);
            toUpdate.ModifiedTime = DateTime.UtcNow;
            toUpdate.ModifiedBy = myUid.ToString();
            int res = await UserConversationRepository.Update(userConversation.UserConversationId, toUpdate);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Xóa những message bị thừa (Task)
            _ = ConversationRepository.DeleteExpiredMessages(conversationId);

            // Trả về xóa thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> UpdateConversation(Guid myUid, UpdateConversationModel updateModel)
        {
            // Check existed
            ViewUser user = await CheckExistedUser(myUid);
            Conversation conv = await CheckExistedConversation(updateModel.ConversationId);

            // Check join
            ViewUserConversation? participant = await ConversationRepository.GetParticipant(myUid, updateModel.ConversationId);
            if (participant == null) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc trò chuyện này");

            // Update cuộc trò chuyện, upload image
            conv.ConversationName = updateModel.ConversationName ?? conv.ConversationName;
            if (updateModel.Thumbnail != null)
            {
                string? thumbnailUrl = await Storage.SaveImage(updateModel.Thumbnail);
                conv.Thumbnail = thumbnailUrl;
            }

            // Trả về thành công
            conv.ModifiedTime = DateTime.UtcNow;
            conv.ModifiedBy = user.FullName;
            int res = await ConversationRepository.Update(conv.ConversationId, conv);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(ConversationResource));
            return ServiceResult.Success(ResponseResource.UpdateSuccess(ConversationResource));
        }

        public virtual async Task<ServiceResult> UpdateUserConversation(Guid myUid, UpdateUserConversationModel updateModel)
        {
            // Kiểm tra tồn tại user 
            ViewUser user = await CheckExistedUser(myUid);

            // Kiểm tra tồn tại participant
            UserConversation? participant = await UserConversationRepository.Get(updateModel.UserConversationId)
                ?? throw new ValidatorException(ResponseResource.NotExist(ResourceFactory.GetEntityResource().UserConversation()));

            // Kiểm tra participant đúng là của user
            if (participant.UserId != myUid) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc trò chuyện này");

            // Update nick name
            participant.Nickname = updateModel.NickName;
            participant.ModifiedTime = DateTime.UtcNow;
            participant.ModifiedBy = user.FullName;
            int res = await UserConversationRepository.Update(participant.UserConversationId, participant);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công 
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public async virtual Task<ServiceResult> SetReadConversation(Guid myUid, Guid conversationId)
        {
            //ViewUser user = await CheckExistedUser(myUid);
            _ = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            ViewUserConversation userConversation = await ConversationRepository.GetParticipant(myUid, conversationId)
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Update Last Read
            userConversation.LastReadTime = DateTime.UtcNow;
            UserConversation toUpdate = new();
            toUpdate.Copy(userConversation);
            toUpdate.ModifiedTime = DateTime.UtcNow;
            toUpdate.ModifiedBy = myUid.ToString();
            int res = await UserConversationRepository.Update(userConversation.UserConversationId, toUpdate);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.Failure());

            // Trả về xóa thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        #endregion

        #region Message Operations
        public async virtual Task<ServiceResult> GetMessages(Guid myUid, Guid conversationId, PaginationDto pagination)
        {
            //ViewUser user = await CheckExistedUser(myUid);
            //Conversation conv = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            ViewUserConversation userConversation = await ConversationRepository.GetParticipant(myUid, conversationId)
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Lấy về danh sách tin nhắn
            List<ResponseMessageModel> messages =
                (await ConversationRepository.GetMessages(conversationId, lastDelete: userConversation.LastDeleteTime, pagination))
                .Select(message => (new ResponseMessageModel().Copy(message) as ResponseMessageModel)!)
                .ToList();

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                messages
            );
        }

        public virtual async Task<ServiceResult> SendMessage(Guid myUid, SendMessageModel model)
        {
            ViewUser profile = await CheckExistedUser(myUid);

            // Check Conversation phù hợp
            _ = await CheckExistedConversation(model.ConversationId);

            // Check participant
            ViewUserConversation? participant = await ConversationRepository.GetParticipant(myUid, model.ConversationId);
            if (participant == null) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc hội thoại này");

            // Thêm message
            Message msg = new()
            {
                MessageId = Guid.NewGuid(),
                UserConversationId = participant.UserConversationId,
                Content = model.Content!,
                Time = DateTime.UtcNow,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = profile.FullName,
                IsEdited = false,
                ReplyId = null
            };
            Guid? id = await MessageRepository.Insert(msg.MessageId, msg);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới các participant khác
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, msg);
        }

        public virtual async Task<ServiceResult> DeleteMessage(Guid myUid, Guid messageId)
        {
            // Check myUid và messageId tồn tại
            ViewUser profile = await CheckExistedUser(myUid);
            ViewMessage message = await CheckExistedMessage(messageId);

            // Kiểm tra quyền messageId với uid
            if (message.UserId != profile.UserId)
                return ServiceResult.Forbidden("Đây không phải là tin nhắn của bạn");

            // Xóa Tin nhắn
            int deleted = await MessageRepository.Delete(messageId);
            if (deleted <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> UpdateMessage(Guid myUid, UpdateMessageModel model)
        {
            // Check myUid và messageId tồn tại
            ViewUser profile = await CheckExistedUser(myUid);
            ViewMessage message = await CheckExistedMessage(model.MessageId);

            // Kiểm tra quyền messageId với uid
            if (message.UserId != profile.UserId)
                return ServiceResult.Forbidden("Đây không phải là tin nhắn của bạn");

            // Cập nhật Tin nhắn
            Message message1 = new();
            message1.Copy(message);
            message1.MessageId = message.MessageId;
            message1.Content = model.Content!;
            message1.ModifiedTime = DateTime.UtcNow;
            message1.ModifiedBy = profile.FullName;
            message1.IsEdited = true;
            int rows = await MessageRepository.Update(message1.MessageId, message1);
            if (rows <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public virtual async Task<ServiceResult> ReplyMessage(Guid myUid, ReplyMessageModel model)
        {
            // Check myUid existed
            ViewUser profile = await CheckExistedUser(myUid);

            // Check ConversationId và ReplyId tồn tại
            Conversation conversation = await CheckExistedConversation(model.ConversationId);
            ViewMessage message = await CheckExistedMessage(model.ReplyId);

            // Check participant (conversation chứa user)
            ViewUserConversation? participant = await ConversationRepository.GetParticipant(myUid, model.ConversationId);
            if (participant == null)
                return ServiceResult.Forbidden("Bạn chưa tham gia cuộc trò chuyện này");

            // Check ReplyId phải trong ConversationId
            if (message.ConversationId != conversation.ConversationId)
                return ServiceResult.BadRequest("Tin nhắn phản hồi không nằm trong cuộc trò chuyện này");

            // Check ReplyId chưa Reply tin nhắn nào khác (cò nhất thiết không?)

            // Thêm message
            Message msg = new()
            {
                MessageId = Guid.NewGuid(),
                UserConversationId = participant.UserConversationId,
                Content = model.Content!,
                Time = DateTime.UtcNow,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = profile.FullName,
                IsEdited = false,
                ReplyId = message.MessageId
            };
            Guid? id = await MessageRepository.Insert(msg.MessageId, msg);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới các participant khác
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }

        public virtual async Task<(bool, ViewMessage?, List<string>)> SendMessageBySocket(Guid myUId, SendMessageSocketModel model)
        {
            try
            {
                //ViewUser profile = await CheckExistedUser(myUId);
                // Check Conversation phù hợp
                //_ = await CheckExistedConversation(model.ConversationId);
                // Check participant

                List<ViewUserConversation> participants = await ConversationRepository.GetParticipants(model.ConversationId);
                ViewUserConversation? participant = participants.Where(p => p.UserId == myUId).FirstOrDefault();
                if (participant == null) return (false, null, []);

                // Thêm message
                Message msg = new()
                {
                    MessageId = Guid.NewGuid(),
                    UserConversationId = participant.UserConversationId,
                    Content = model.Content!,
                    Time = DateTime.UtcNow,
                    CreatedTime = DateTime.UtcNow,
                    CreatedBy = participant.FullName,
                    IsEdited = false,
                    ReplyId = null
                };
                Guid? id = await MessageRepository.Insert(msg.MessageId, msg);
                if (id == null) return (false, null, []);

                ViewMessage res = new();
                res.Copy(msg);
                res.Copy(participant);

                return (true, res, participants.Select(p => p.Username).ToList());
            }
            catch (Exception)
            {
                return (false, null, []);
            }
        }
        #endregion
    }
}
