using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.WebSockets
{
    public class SocketSessionManager : ISocketSessionManager
    {
        private readonly Dictionary<string, List<KSSocket>> userSockets = [];

        public virtual void AddSocket(KSSocket socket)
        {
            if (!userSockets.TryGetValue(socket.Username, out List<KSSocket>? value))
            {
                value = [];
                userSockets[socket.Username] = value;
            }

            value.Add(socket);
        }

        public virtual void RemoveSocket(KSSocket socket)
        {
            if (userSockets.TryGetValue(socket.Username, out List<KSSocket>? value))
            {
                value.Remove(socket);
                if (value.Count == 0)
                {
                    userSockets.Remove(socket.Username);
                }
            }
        }

        public virtual void RemoveSocket(string socketId)
        {
            foreach (var sockets in userSockets.Values)
            {
                var socketToRemove = sockets.Find(s => s.SocketId.ToString() == socketId);
                if (socketToRemove != null)
                {
                    sockets.Remove(socketToRemove);
                    if (sockets.Count == 0)
                    {
                        userSockets.Remove(socketToRemove.Username);
                    }
                    break;
                }
            }
        }

        public virtual void RemoveSocketByUsername(string username)
        {
            userSockets.Remove(username);
        }

        public virtual IEnumerable<KSSocket> GetSockets(string username)
        {
            if (userSockets.TryGetValue(username, out List<KSSocket>? value))
            {
                return value;
            }
            return Enumerable.Empty<KSSocket>();
        }

        public virtual void RemoveAllSocket()
        {
            userSockets.Clear();
        }

        public virtual IEnumerable<KSSocket> GetAllSockets()
        {
            return userSockets.Values.SelectMany(list => list);
        }

        public virtual IEnumerable<string> GetUsernames()
        {
            return userSockets.Keys;
        }
    }
}
