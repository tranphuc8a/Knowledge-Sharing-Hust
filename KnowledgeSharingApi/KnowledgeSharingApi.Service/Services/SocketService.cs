using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public abstract class SocketService(
            ISocketSessionManager socketManager,
            IResourceFactory resourceFactory,
            IEncrypt Encrypt
        ) : ISocketService
    {
        protected readonly ISocketSessionManager socketManager = socketManager;
        protected readonly IResourceFactory resourceFactory = resourceFactory;
        protected readonly IResponseResource responseResource = resourceFactory.GetResponseResource();
        protected IEncrypt Encrypt = Encrypt;

        #region Send messages and Close Socket
        public virtual async Task<bool> CloseAllSocket()
        {
            List<KSSocket> lsSockets = socketManager.GetAllSockets();
            List<Task<bool>> lsTask = lsSockets.Select(socket => CloseSocket(socket)).ToList();
            await Task.WhenAll(lsTask);
            socketManager.RemoveAllSocket();
            return true;
        }

        public virtual async Task<bool> CloseSocket(KSSocket socket)
        {
            try
            {
                await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
                socketManager.RemoveSocket(socket);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing socket: " + ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> CloseSocket(string username)
        {
            try
            {
                List<KSSocket> lsSockets = socketManager.GetSockets(username);
                List<Task> closeTasks = lsSockets.Select(async socket =>
                {
                    await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
                    socketManager.RemoveSocket(socket);
                }).ToList();
                await Task.WhenAll(closeTasks);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public virtual async Task<bool> Send(string data, KSSocket socket)
        {
//            string message = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            await socket.Socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            return true;
        }

        public virtual async Task<bool> Send(string data, string username)
        {
            try
            {
                List<KSSocket> lsSockets = socketManager.GetSockets(username);
                List<Task> lsTask = lsSockets.Select<KSSocket, Task>(socket => Send(data, socket)).ToList();
                await Task.WhenAll(lsTask);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SendBroadcast(string data)
        {
            try
            {
                List<KSSocket> lsSockets = socketManager.GetAllSockets();
                List<Task> lsTask = lsSockets.Select<KSSocket, Task>(socket => Send(data, socket)).ToList();
                await Task.WhenAll(lsTask);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        #region Connect to Socket
        public virtual async Task<ServiceResult> ConnectSocket(HttpContext HttpContext, string? token)
        {
            // Check token lấy username
            JwtTokenDto? tokenDto = VerifyToken(token);
            if (tokenDto == null)
            {
                return ServiceResult.BadRequest(responseResource.InvalidToken());
            }

            // Chấp nhận kết nối
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                KSSocket socket = new(tokenDto.Username, tokenDto.UserId, webSocket);
                socketManager.AddSocket(socket);

                // Bắt đầu vòng lặp để lắng nghe tin nhắn từ client
                await ReceiveMessages(socket);

                return ServiceResult.Success(responseResource.Success());
                //await WaitForCloseSession(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return ServiceResult.BadRequest(responseResource.Failure());
            }
        }


        public virtual async Task<ServiceResult> ConnectSocket(HttpContext HttpContext)
        {
            // Chấp nhận kết nối
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Guid newId = Guid.NewGuid();
                KSSocket socket = new(newId.ToString(), newId, webSocket);
                socketManager.AddSocket(socket);

                // Bắt đầu vòng lặp để lắng nghe tin nhắn từ client
                await ReceiveMessages(socket);

                return ServiceResult.Success(responseResource.Success(), string.Empty, newId);
                //await WaitForCloseSession(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return ServiceResult.BadRequest(responseResource.Failure());
            }
        }


        protected virtual JwtTokenDto? VerifyToken(string? token)
        {
            try
            {
                if (token == null) return null;

                JwtTokenDto? tokenDto = Encrypt.JwtDecrypt(token, isValidateLifeTime: true);
                if (token == null) return null;

                return tokenDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual async Task ReceiveMessages(KSSocket socket)
        {
            WebSocket webSocket = socket.Socket;

            // Buffer để lưu trữ dữ liệu từ client
            byte[] buffer = new byte[1024];

            // Vòng lặp vô hạn để lắng nghe tin nhắn từ client
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                // Kiểm tra nếu client đã đóng kết nối
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    socketManager.RemoveSocket(socket);
                }
                else
                {
                    // Đọc dữ liệu từ buffer và xử lý tin nhắn từ client ở đây
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Xử lý tin nhắn từ client
                    await HandleMessage(socket, message);
                }
            }
        } 
        #endregion

        protected abstract Task HandleMessage(KSSocket socket, string message);

    }
}
