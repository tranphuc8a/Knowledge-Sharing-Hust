using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class LiveChatSocketService(
        ILiveChatSocketSessionManager liveChatSocketSessionManager,
        IResourceFactory resourceFactory,
        //IConversationService conversationService,
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IEncrypt Encrypt
        ) : SocketService(liveChatSocketSessionManager, resourceFactory, Encrypt), ILiveChatSocketService
    {
        //protected readonly IConversationService ConversationService = conversationService;
        protected readonly IConversationRepository ConversationRepository = conversationRepository;
        protected readonly IMessageRepository MessageRepository = messageRepository;

        protected override async Task HandleMessage(KSSocket socket, string message)
        {
            // Step 1. Tạo Message ứng với message
            try
            {
                SendMessageSocketModel? messageFromClient = JsonConvert.DeserializeObject<SendMessageSocketModel>(message)
                    ?? throw new JsonReaderException();

                List<ViewUserConversation> participants = await ConversationRepository.GetParticipants(messageFromClient.ConversationId);
                ViewUserConversation? participant = participants.Where(p => p.UserId == socket.UserId).FirstOrDefault();
                if (participant == null) return;

                // Thêm message
                Message msg = new()
                {
                    MessageId = Guid.NewGuid(),
                    UserConversationId = participant.UserConversationId,
                    Content = messageFromClient.Content,
                    Time = DateTime.UtcNow,
                    CreatedTime = DateTime.UtcNow,
                    CreatedBy = participant.FullName,
                    IsEdited = false,
                    ReplyId = null
                };

                ViewMessage res = new();
                res.Copy(msg);
                res.Copy(participant);

                string messageToSend = JsonConvert.SerializeObject(res);
                await Task.WhenAll(participants.Select(p => Send(messageToSend, p.Username)));

                await MessageRepository.Insert(msg.MessageId, msg);
            }
            catch (JsonReaderException ex)
            {
                // Xử lý khi có lỗi xảy ra trong quá trình phân tích JSON
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác nếu có
                Console.WriteLine("An error occurred: " + ex.Message);
            }



            // Step 4. Thành công
        }

        //protected override async Task HandleMessage(KSSocket socket, string message)
        //{
        //     Send Broadcast
        //    string data = (string) JsonConvert.DeserializeObject(message)!;
        //    await SendBroadcast(JsonConvert.SerializeObject($"{socket.Username}: {data}"));
        //}

        //public override async Task<ServiceResult> ConnectSocket(HttpContext HttpContext, string? token)
        //{
        //    // check token
        //    if (string.IsNullOrEmpty(token)) return ServiceResult.BadRequest("Token is Empty");
        //    JwtTokenDto? jwtToken = Encrypt.JwtDecrypt(token, isValidateLifeTime: true);
        //    if (jwtToken == null) return ServiceResult.BadRequest("Token is invalid or expired");

        //    // Chấp nhận kết nối
        //    if (HttpContext.WebSockets.IsWebSocketRequest)
        //    {
        //        WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        //        KSSocket socket = new(jwtToken.Username, jwtToken.UserId, webSocket);
        //        socketManager.AddSocket(socket);

        //        // Bắt đầu vòng lặp để lắng nghe tin nhắn từ client
        //        await ReceiveMessages(socket);

        //        return ServiceResult.Success(responseResource.Success());
        //    }
        //    else
        //    {
        //        //HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //        return ServiceResult.BadRequest(responseResource.Failure());
        //    }
        //}
    }
}
