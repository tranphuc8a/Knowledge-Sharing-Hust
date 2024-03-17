using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
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
        IEncrypt Encrypt
        ) : SocketService(liveChatSocketSessionManager, resourceFactory, Encrypt), ILiveChatSocketService
    {
        //    protected override Task HandleMessage(KSSocket socket, string message)
        //    {
        //        // Step 1. Tạo Message ứng với message
        //        try
        //        {
        //            SendMessageSocketModel? messageFromClient = JsonConvert.DeserializeObject<SendMessageSocketModel>(message)
        //                ?? throw new JsonReaderException();

        //            // Tạo message
        //            Message messageToDatabase = new()
        //            {
        //                UserConservationId = messageFromClient.UserConservationId,
        //                Content = messageFromClient.Content,
        //                Time = DateTime.Now,
        //                CreatedTime = DateTime.Now,
        //                CreatedBy = socket.Username,
        //                IsEdited = false,
        //                ReplyId = messageFromClient.ReplyId
        //            };


        //            // Lấy ra Conservation


        //            // Lấy ra người dùng thứ kia và gửi message tới họ

        //            messageFromClient.UserConservationId;

        //            // Step 2. Lưu message vào database

        //            // Step 3. Gửi lại thông báo tới user
        //        }
        //        catch (JsonReaderException ex)
        //        {
        //            // Xử lý khi có lỗi xảy ra trong quá trình phân tích JSON
        //            Console.WriteLine("Error parsing JSON: " + ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Xử lý các ngoại lệ khác nếu có
        //            Console.WriteLine("An error occurred: " + ex.Message);
        //        }



        //        // Step 4. Thành công
        //        return Task.CompletedTask;
        //    }
        //}

        protected override async Task HandleMessage(KSSocket socket, string message)
        {
            // Send Broadcast
            string data = (string) JsonConvert.DeserializeObject(message)!;
            await SendBroadcast(JsonConvert.SerializeObject($"{socket.Username}: {data}"));
        }

        public override async Task<ServiceResult> ConnectSocket(HttpContext HttpContext, string? token)
        {
            // Chấp nhận kết nối
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                KSSocket socket = new(Guid.NewGuid().ToString(), webSocket);
                socketManager.AddSocket(socket);

                // Bắt đầu vòng lặp để lắng nghe tin nhắn từ client
                await ReceiveMessages(socket);

                return ServiceResult.Success(responseResource.Success());
            }
            else
            {
                //HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return ServiceResult.BadRequest(responseResource.Failure());
            }
        }
    }
}
