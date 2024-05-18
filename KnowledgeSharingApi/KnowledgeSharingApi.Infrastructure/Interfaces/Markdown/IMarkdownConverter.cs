using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Markdown
{
    public interface IMarkdownConverter
    {
        /// <summary>
        /// Xóa thẻ html trong nội dung content
        /// </summary>
        /// <param name="content">Nội dung cần xóa</param>
        /// <returns>Chuỗi sau khi format</returns>
        /// Created: PhucTV (18/5/24)
        /// Modified: None
        string RemoveHtmlTag(string content);

        /// <summary>
        /// Xóa cú pháp markdown
        /// </summary>
        /// <param name="content">Nội dung format</param>
        /// <returns>Chuỗi sau khi format</returns>
        /// Created: PhucTV (18/5/24)
        /// Modified: None
        string RemoveMarkdownSyntax(string content);

        /// <summary>
        /// Lấy về text tinh khiết sau khi remove các syntax
        /// </summary>
        /// <param name="content">Nội dung format</param>
        /// <returns>Chuỗi sau khi format</returns>
        /// Created: PhucTV (18/5/24)
        /// Modified: None
        string GetPureText(string content);
    }
}
