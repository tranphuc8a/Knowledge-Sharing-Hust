using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets
{
    public interface ISocketSessionManager
    {
        /// <summary>
        /// Thêm socket cho người dùng có username
        /// </summary>
        /// <param name="socket"> Socket được tạo </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        void AddSocket(KSSocket socket);

        /// <summary>
        /// Xóa bỏ socket cho trước
        /// </summary>
        /// <param name="socket"> Socket muốn xóa </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        void RemoveSocket(KSSocket socket);

        /// <summary>
        /// Xóa bỏ socket cho trước
        /// </summary>
        /// <param name="socketId"> Id của Socket muốn xóa </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        void RemoveSocket(string socketId);

        /// <summary>
        /// Xóa toàn bộ socket của username
        /// </summary>
        /// <param name="username"> Người dùng muốn xóa socket </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        void RemoveSocketByUsername(string username);


        /// <summary>
        /// Xóa toàn bộ socket
        /// </summary>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        void RemoveAllSocket();


        /// <summary>
        /// Lấy danh sách socket của một username
        /// </summary>
        /// <param name="username"> Người dùng muốn lấy socket </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        List<KSSocket> GetSockets(string username);

        /// <summary>
        /// Lấy danh sách toàn bộ socket
        /// </summary>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        List<KSSocket> GetAllSockets();


        /// <summary>
        /// Lấy danh sách toàn bộ username
        /// </summary>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        List<string> GetUsernames();
    }
}
