using KnowledgeSharingApi.Domains.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ISocketService
    {
        Task<ServiceResult> ConnectSocket(HttpContext httpContext, string? token);

        Task<bool> CloseSocket(KSSocket socket);

        Task<bool> CloseSocket(string username);

        Task<bool> CloseAllSocket();

        Task<bool> Send(string data, KSSocket socket);

        Task<bool> Send(string data, string username);

        Task<bool> SendBroadcast(string data);
    }
}
