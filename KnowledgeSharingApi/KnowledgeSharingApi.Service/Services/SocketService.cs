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
            IEnumerable<KSSocket> lsSockets = socketManager.GetAllSockets();
            IEnumerable<Task> lsTask = lsSockets.Select(socket => CloseSocket(socket));
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
                IEnumerable<KSSocket> lsSockets = socketManager.GetSockets(username);
                IEnumerable<Task> closeTasks = lsSockets.Select(async socket =>
                {
                    await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", CancellationToken.None);
                    socketManager.RemoveSocket(socket);
                });
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
                IEnumerable<KSSocket> lsSockets = socketManager.GetSockets(username);
                IEnumerable<Task> lsTask = lsSockets.Select<KSSocket, Task>(socket => Send(data, socket));
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
                IEnumerable<KSSocket> lsSockets = socketManager.GetAllSockets();
                IEnumerable<Task> lsTask = lsSockets.Select<KSSocket, Task>(socket => Send(data, socket));
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
            string? username = VerifyToken(token);
            if (username == null)
            {
                return ServiceResult.BadRequest(responseResource.InvalidToken());
            }

            // Chấp nhận kết nối
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                KSSocket socket = new(username, webSocket);
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


        protected virtual string? VerifyToken(string? token)
        {
            try
            {
                if (token == null) return null;

                ClaimsPrincipal? claimsPrincipal = Encrypt.JwtDecryptToClaimsPrincipal(token, isValidateLifeTime: true);
                if (claimsPrincipal == null) return null;

                return claimsPrincipal.Identity?.Name;
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
