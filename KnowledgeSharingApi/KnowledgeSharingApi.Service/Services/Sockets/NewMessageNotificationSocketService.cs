using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using KnowledgeSharingApi.Services.Interfaces.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services.Sockets
{
    public class NewMessageNotificationSocketService(
        INewMessageNotificationSocketSessionManager newMessageNotificationSocketSessionManager,
        IResourceFactory resourceFactory,
        IEncrypt Encrypt
        ) : SocketService(newMessageNotificationSocketSessionManager, resourceFactory, Encrypt), INewMessageNotificationSocketService
    {
        protected override Task HandleMessage(KSSocket socket, string message)
        {
            // do nothing
            return Task.CompletedTask;
        }
    }
}
