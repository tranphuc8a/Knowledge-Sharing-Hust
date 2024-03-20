using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces
{
    public interface IEntityResource
    {
        #region Entity Name
        /// <summary>
        /// Tên của các bảng, lớp, đối tượng, thực thể
        /// </summary>
        /// <returns> string </returns>
        /// Created: PhucTV (10/1/24)
        /// Modified: None
        String Block();
        String Category();
        String Comment();
        String Conversation();
        String Course();
        String CourseLesson();
        String CoursePayment();
        String CourseRegister();
        String CourseRelation();
        String Follow();
        String Friend();
        String Knowledge();
        String KnowledgeCategory();
        String Lesson();
        String Mark();
        String Message();
        String Notification();
        String Post();
        String PostEditHistory();
        String Profile();
        String Question();
        String RequestFriend();
        String Session();
        String Star();
        String StudyProgress();
        String User();
        String UserConversation();
        String UserItem();
        String UserRelation();

        #endregion


        #region FieldName
        /// <summary>
        /// Tên của một số trường hay dùng
        /// </summary>
        /// <returns> string </returns>
        /// Created: PhucTV (10/1/24)
        /// Modified: None
        string Username();
        string Password();
        string Email();
        string DateOfBirth();
        string Time();
        String Sender();
        String Receicer();
        string Avatar();
        string Cover();
        string Privacy(string? entityName);
        String Title(string? entityName);
        string Abstract(string? entityName);
        string Introduction(string? entityName);
        string Content(string? entityName);
        string Thumbnail(string? entityName);
        string Id(string? entityName);
        string Name(string? entityName);
        #endregion

    }
}
