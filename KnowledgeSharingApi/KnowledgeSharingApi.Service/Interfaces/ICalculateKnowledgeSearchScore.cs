using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICalculateKnowledgeSearchScore
    {

        /// <summary>
        /// Tinh toan search score giua search text va list knowledge rut gon
        /// </summary>
        /// <param name="search"></param>
        /// <param name="listKnowledges"> Danh sach thong tin rut gon cua danh sachs knowledge </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Dictionary<Guid, double> Calculate(string search, List<(Guid UserItemId, string Title, string Fullname, string Content, string? Abstract)> listKnowledges);

    }
}
