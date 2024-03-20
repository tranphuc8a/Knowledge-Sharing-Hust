﻿using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
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
        protected virtual async Task<ResponseConversationModel> GetConversationDetail(string myUid, string conversationId)
        {
            // Lấy về cuộc hội thoại
            Conversation conv = await ConversationRepository.Get(conversationId)
                ?? throw new ValidatorException(ResponseResource.NotFound(ConversationResource));

            // Lấy về danh sách participant
            IEnumerable<ResponseParticipantModel> listParticipants =
                (await ConversationRepository.GetParticipants(conversationId))
                .Select(participant => (new ResponseParticipantModel().Copy(participant) as ResponseParticipantModel)!);

            // Lấy về top tin nhắn
            IEnumerable<ResponseMessageModel> topMessages =
                (await ConversationRepository.GetMessages(myUid, conversationId, limit: DefaultLimit, offset: 0))
                .Select(message => (new ResponseMessageModel().Copy(message) as ResponseMessageModel)!);


            ResponseConversationModel model = new();
            model.Copy(conv);
            model.Participants = listParticipants;
            model.Messages = topMessages;

            return model;
        }

        /// <summary>
        /// Kiểm tra user tồn tại
        /// </summary>
        /// <param name="userId"> id của user kiểm tra </param>
        /// <returns> Trả về user </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<Profile> CheckExistedUser(string userId)
        {
            return await UserRepository.GetDetail(userId)
                ?? throw new ValidatorException(ResponseResource.NotFound(ResourceFactory.GetEntityResource().User()));
        }

        /// <summary>
        /// Kiểm tra Cuộc trò chuyện tồn tại
        /// </summary>
        /// <param name="id"> id của cuộc trò chuyện kiểm tra </param>
        /// <returns> Trả về cuộc trò chuyện </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<Conversation> CheckExistedConversation(string id)
        {
            return await ConversationRepository.Get(id)
                ?? throw new ValidatorException(ResponseResource.NotFound(ConversationResource));
        }

        /// <summary>
        /// Kiểm tra tin nhắn tồn tại
        /// </summary>
        /// <param name="id"> id của tin nhắn kiểm tra </param>
        /// <returns> Trả về tin nhắn </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ViewMessage> CheckExistedMessage(string id)
        {
            return await MessageRepository.GetDetail(id)
                ?? throw new ValidatorException(ResponseResource.NotFound(ResourceFactory.GetEntityResource().Message()));
        }

        /// <summary>
        /// Tạo mới cuộc trò chuyện
        /// </summary>
        /// <param name="myUid"> id của người thứ nhất </param>
        /// <param name="uid"> id của người thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ServiceResult> CreateNewConversation(Profile user_1, Profile user_2)
        {
            // Tạo cuộc trò chuyện
            Conversation conversation = new()
            {
                ConversationId = Guid.NewGuid(),
                ConversationName = $"{user_1.FullName} và {user_2.FullName}",
                CreatedTime = DateTime.Now,
                CreatedBy = user_1.FullName
            };

            // Thêm 2 thành viên
            UserConversation userConversation1 = new()
            {
                UserConversationId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                UserId = user_1.UserId,
                Time = DateTime.Now,
                LastDeleteTime = DateTime.Now,
                LastReadTime = DateTime.Now,
                Nickname = user_1.FullName
            };
            UserConversation userConversation2 = new()
            {
                UserConversationId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                UserId = user_2.UserId,
                Time = DateTime.Now,
                LastDeleteTime = DateTime.Now,
                LastReadTime = DateTime.Now,
                Nickname = user_2.FullName
            };

            // Bắt đầu transaction
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.RegisterRepository(ConversationRepository)
                    .RegisterRepository(UserConversationRepository);

                _ = await ConversationRepository.Insert(conversation.ConversationId.ToString(), conversation)
                    ?? throw new Exception();
                _ = await UserConversationRepository.Insert(userConversation1.UserConversationId.ToString(), userConversation2)
                    ?? throw new Exception();
                _ = await UserConversationRepository.Insert(userConversation2.UserConversationId.ToString(), userConversation2)
                    ?? throw new Exception();

                UnitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                UnitOfWork.RollbackTransaction();
                return ServiceResult.ServerError(ResponseResource.ServerError());
            }

            // Trả về cuộc trò chuyện
            return ServiceResult.Success(
                ResponseResource.Success(),
                string.Empty,
                await GetConversationDetail(user_1.UserId.ToString(), conversation.ConversationId.ToString())
            );
        }

        #endregion


        #region Get Conversation
        public virtual async Task<ServiceResult> GetConversation(string myUId, string conversationId)
        {
            // Kiểm tra userId phải tồn tại
            Profile user = await CheckExistedUser(myUId);

            // Kiểm tra conversation phải tồn tại
            Conversation conversation = await CheckExistedConversation(conversationId);

            // Kiểm tra conversation phải chứa myUid
            IEnumerable<ViewUserConversation> participants = await ConversationRepository.GetParticipants(conversationId);
            if (!participants.Any(participant => participant.UserId.ToString() == myUId))
                return ServiceResult.Forbidden("Bạn chưa tham gia cuộc hội thoại này");

            // Lấy về thông tin chi tiết conservation
            ResponseConversationModel model = await GetConversationDetail(myUId, conversationId);

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.GetSuccess(ConversationResource), string.Empty, model);
        }

        public virtual async Task<ServiceResult> GetConversations(string myUid, int? limit, int? offset)
        {
            // Kiểm tra user tồn tại
            await CheckExistedUser(myUid);

            // Lấy về danh sách cuộc trò chuyện
            IEnumerable<Conversation> conversations = await ConversationRepository.GetListConversationByUserId(myUid);

            // Trả về thành công
            return ServiceResult.Success(
                UserMessage: ResponseResource.GetMultiSuccess(ConversationResource),
                DevMessage: string.Empty,
                new PaginationResponseModel<Conversation>()
                {
                    Total = conversations.Count(),
                    Limit = conversations.Count(),
                    Offset = 0,
                    Results = conversations
                }
            );
        }

        public virtual async Task<ServiceResult> GetConversationWith(string myUid, string uid)
        {
            Profile user_1 = await CheckExistedUser(myUid);
            Profile user_2 = await CheckExistedUser(uid);

            Conversation? conversation = await ConversationRepository.GetConversationWithUser(myUid, uid);
            if (conversation == null)
                return ServiceResult.NotFound(ResponseResource.NotFound(ConversationResource));

            return ServiceResult.Success(
                ResponseResource.NotFound(ConversationResource),
                string.Empty,
                await GetConversationDetail(myUid, conversation.ConversationId.ToString())
            );
        }

        #endregion

        #region Conversation Operations
        public virtual async Task<ServiceResult> StartConversation(string myUid, string uid)
        {
            Profile user_1 = await CheckExistedUser(myUid);
            Profile user_2 = await CheckExistedUser(uid);

            Conversation? conversation = await ConversationRepository.GetConversationWithUser(myUid, uid);
            if (conversation != null)
            {
                return ServiceResult.Success(
                    ResponseResource.NotFound(ConversationResource),
                    string.Empty,
                    await GetConversationDetail(myUid, conversation.ConversationId.ToString())
                );
            }

            return await CreateNewConversation(user_1, user_2);
        }

        public virtual async Task<ServiceResult> DeleteConversation(string myUid, string conversationId)
        {
            Profile user = await CheckExistedUser(myUid);
            Conversation conv = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            IEnumerable<ViewUserConversation> participants = await ConversationRepository.GetParticipants(conversationId);
            ViewUserConversation userConversation =
                participants.Where(participant => participant.UserId.ToString() == myUid).FirstOrDefault()
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Update Last Delete
            userConversation.LastDeleteTime = DateTime.Now;
            int res = await UserConversationRepository.Update(userConversation.UserConversationId.ToString(), userConversation);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure());

            // Xóa những message bị thừa (Task)
            _ = ConversationRepository.DeleteExpiredMessages(conversationId);

            // Trả về xóa thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess());
        }

        public virtual async Task<ServiceResult> UpdateConversation(string myUid, UpdateConversationModel updateModel)
        {
            // Check existed
            Profile user = await CheckExistedUser(myUid);
            Conversation conv = await CheckExistedConversation(updateModel.ConversationId.ToString());

            // Check join
            ViewUserConversation? participant =
                (await ConversationRepository.GetParticipants(updateModel.ConversationId.ToString()))
                .Where(participant => participant.UserId.ToString() == myUid)
                .FirstOrDefault();
            if (participant == null) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc trò chuyện này");

            // Update cuộc trò chuyện, upload image
            conv.ConversationName = updateModel.ConversationName ?? conv.ConversationName;
            if (updateModel.Thumbnail != null)
            {
                string? thumbnailUrl = await Storage.SaveImage(updateModel.Thumbnail);
                conv.Thumbnail = thumbnailUrl;
            }

            // Trả về thành công
            conv.ModifiedTime = DateTime.Now;
            conv.ModifiedBy = user.FullName;
            int res = await ConversationRepository.Update(conv.ConversationId.ToString(), conv);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure(ConversationResource));
            return ServiceResult.Success(ResponseResource.UpdateSuccess(ConversationResource));
        }

        public virtual async Task<ServiceResult> UpdateUserConversation(string myUid, UpdateUserConversationModel updateModel)
        {
            // Kiểm tra tồn tại user 
            Profile user = await CheckExistedUser(myUid);

            // Kiểm tra tồn tại participant
            UserConversation? participant = await UserConversationRepository.Get(updateModel.UserConversationId.ToString())
                ?? throw new ValidatorException(ResponseResource.NotExist(ResourceFactory.GetEntityResource().UserConversation()));

            // Kiểm tra participant đúng là của user
            if (participant.UserId.ToString() != myUid) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc trò chuyện này");

            // Update nick name
            participant.Nickname = updateModel.NickName;
            participant.ModifiedTime = DateTime.Now;
            participant.ModifiedBy = user.FullName;
            int res = await UserConversationRepository.Update(participant.UserConversationId.ToString(), participant);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());

            // Trả về thành công 
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public async virtual Task<ServiceResult> SetReadConversation(string myUid, string conversationId)
        {
            Profile user = await CheckExistedUser(myUid);
            Conversation conv = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            IEnumerable<ViewUserConversation> participants = await ConversationRepository.GetParticipants(conversationId);
            ViewUserConversation userConversation =
                participants.Where(participant => participant.UserId.ToString() == myUid).FirstOrDefault()
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Update Last Read
            userConversation.LastReadTime = DateTime.Now;
            int res = await UserConversationRepository.Update(userConversation.UserConversationId.ToString(), userConversation);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.Failure());

            // Trả về xóa thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        #endregion

        #region Message Operations
        public async virtual Task<ServiceResult> GetMessages(string myUid, string conversationId, int? limit, int? offset)
        {
            Profile user = await CheckExistedUser(myUid);
            Conversation conv = await CheckExistedConversation(conversationId);

            // Kiểm tra đúng quyền
            IEnumerable<ViewUserConversation> participants = await ConversationRepository.GetParticipants(conversationId);
            ViewUserConversation userConversation =
                participants.Where(participant => participant.UserId.ToString() == myUid).FirstOrDefault()
                ?? throw new ValidatorException("Bạn chưa tham gia cuộc hội thoại này");

            // Validate limit và offset
            int limitValue = limit ?? DefaultLimit;
            int offsetValue = offset ?? 0;

            // Lấy về danh sách tin nhắn
            IEnumerable<ResponseMessageModel> messages =
                (await ConversationRepository.GetMessages(myUid, conversationId, limit: limitValue, offset: offsetValue))
                .Select(message => (new ResponseMessageModel().Copy(message) as ResponseMessageModel)!);

            // Trả về thành công
            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(),
                string.Empty,
                messages
            );
        }

        public virtual async Task<ServiceResult> SendMessage(string myUid, SendMessageModel model)
        {
            Profile profile = await CheckExistedUser(myUid);

            // Check Conversation phù hợp
            Conversation conv = await CheckExistedConversation(model.ConversationId.ToString());

            // Check participant
            ViewUserConversation? participant =
                (await ConversationRepository.GetParticipants(conv.ConversationId.ToString()))
                .Where(part => part.UserId.ToString() == myUid)
                .FirstOrDefault();
            if (participant == null) return ServiceResult.Forbidden("Bạn chưa tham gia cuộc hội thoại này");

            // Thêm message
            Message msg = new()
            {
                MessageId = Guid.NewGuid(),
                UserConversationId = participant.UserConversationId,
                Content = model.Content!,
                Time = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatedBy = profile.FullName,
                IsEdited = false,
                ReplyId = null
            };
            string? id = await MessageRepository.Insert(msg.MessageId.ToString(), msg);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới các participant khác
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }

        public virtual async Task<ServiceResult> DeleteMessage(string myUid, string messageId)
        {
            // Check myUid và messageId tồn tại
            Profile profile = await CheckExistedUser(myUid);
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

        public virtual async Task<ServiceResult> UpdateMessage(string myUid, UpdateMessageModel model)
        {
            // Check myUid và messageId tồn tại
            Profile profile = await CheckExistedUser(myUid);
            ViewMessage message = await CheckExistedMessage(model.MessageId.ToString());

            // Kiểm tra quyền messageId với uid
            if (message.UserId != profile.UserId)
                return ServiceResult.Forbidden("Đây không phải là tin nhắn của bạn");

            // Cập nhật Tin nhắn
            Message message1 = new();
            message1.Copy(message);
            message1.MessageId = message.MessageId;
            message1.Content = model.Content!;
            message1.ModifiedTime = DateTime.Now;
            message1.ModifiedBy = profile.FullName;
            message1.IsEdited = true;
            int rows = await MessageRepository.Update(message1.MessageId.ToString(), message1);
            if (rows <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public virtual async Task<ServiceResult> ReplyMessage(string myUid, ReplyMessageModel model)
        {
            // Check myUid
            Profile profile = await CheckExistedUser(myUid);

            // Check ConversationId và ReplyId tồn tại
            Conversation conversation = await CheckExistedConversation(model.ConversationId.ToString());
            ViewMessage message = await CheckExistedMessage(model.ReplyId.ToString());

            // Check participant (conversation chứa user)
            ViewUserConversation? participant =
                (await ConversationRepository.GetParticipants(conversation.ConversationId.ToString()))
                .Where(p => p.UserId == profile.UserId)
                .FirstOrDefault();
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
                Time = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatedBy = profile.FullName,
                IsEdited = false,
                ReplyId = message.MessageId
            };
            string? id = await MessageRepository.Insert(msg.MessageId.ToString(), msg);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Gửi thông báo tới các participant khác
            // Làm sau...

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
        #endregion
    }
}