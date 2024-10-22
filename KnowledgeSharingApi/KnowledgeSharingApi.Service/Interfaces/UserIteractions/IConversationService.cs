﻿using KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces.UserIteractions
{
    public interface IConversationService
    {
        /// <summary>
        /// Lấy về chi tiết cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muốn lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversation(Guid myUId, Guid conversationId);

        /// <summary>
        /// Lấy về danh sách cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="pagination"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversations(Guid myUId, PaginationDto pagination);

        /// <summary>
        /// Lấy về chi tiết cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="uid"> Id của người dùng thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetConversationWith(Guid myUId, Guid uid);

        /// <summary>
        /// Bắt đầu cuộc trò chuyện mới hoặc lấy về cuộc trò chuyện đã có sẵn
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="uid"> Id của người dùng thứ hai </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> StartConversation(Guid myUId, Guid uid);

        /// <summary>
        /// Xóa cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muỗn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteConversation(Guid myUId, Guid conversationId);

        /// <summary>
        /// Cập nhật cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="updateModel"> Thông tin cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateConversation(Guid myUId, UpdateConversationModel updateModel);

        /// <summary>
        /// Cập nhật thông tin người tham gia cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="updateModel"> Thông tin cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateUserConversation(Guid myUId, UpdateUserConversationModel updateModel);

        /// <summary>
        /// Đánh dấu đã đọc cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> SetReadConversation(Guid myUId, Guid conversationId);

        /// <summary>
        /// Lấy về danh sách tin nhắn của cuộc trò chuyện
        /// </summary>
        /// <param name="myUId"> Id của người muốn lấy </param>
        /// <param name="conversationId"> Id của cuộc trò chuyện muốn lấy </param>
        /// <param name="pagination"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> GetMessages(Guid myUId, Guid conversationId, PaginationDto pagination);

        /// <summary>
        /// Gửi tin nhắn mới
        /// </summary>
        /// <param name="myUId"> Id của người muốn gửi </param>
        /// <param name="model"> Nội dung tin nhắn muốn gửi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> SendMessage(Guid myUId, SendMessageModel model);

        /// <summary>
        /// Gửi tin nhắn mới
        /// </summary>
        /// <param name="myUId"> Id của người muốn gửi </param>
        /// <param name="model"> Nội dung tin nhắn muốn gửi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<(bool, ViewMessage?, List<string>)> SendMessageBySocket(Guid myUId, SendMessageSocketModel model);

        /// <summary>
        /// Xóa tin nhắn
        /// </summary>
        /// <param name="myUId"> Id của người muốn xóa </param>
        /// <param name="messageId"> Id của tin nhắn muốn xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> DeleteMessage(Guid myUId, Guid messageId);

        /// <summary>
        /// Cập nhật tin nhắn
        /// </summary>
        /// <param name="myUId"> Id của người muốn cập nhật </param>
        /// <param name="model"> Nội dung cập nhật tin nhắn </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMessage(Guid myUId, UpdateMessageModel model);

        /// <summary>
        /// Phản hồi tin nhắn
        /// </summary>
        /// <param name="myUId"> Id của người muốn thực hiện </param>
        /// <param name="model"> Nội dung tin nhắn muốn phản hồi </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<ServiceResult> ReplyMessage(Guid myUId, ReplyMessageModel model);
    }
}
