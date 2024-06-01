using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class BroadcastSocket(
        IBroadcastSocketSesstionManager broadcastSocketSesstionManager,
        IResourceFactory resourceFactory,
        IEncrypt encrypt
    ) : SocketService(broadcastSocketSesstionManager, resourceFactory, encrypt), IBroadcastSocket
    {
        protected override async Task HandleMessage(KSSocket socket, string message)
        {
            string username = socket.Username;
            string response = username + ": " + message;
            await SendBroadcast(response);
        }
    }
}
