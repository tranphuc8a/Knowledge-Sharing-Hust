using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class KSSocket(string username, Guid userId, WebSocket socket)
    {
        public Guid SocketId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } = userId;
        public string Username { get; set; } = username;
        public WebSocket Socket { get; set; } = socket;

    }
}
