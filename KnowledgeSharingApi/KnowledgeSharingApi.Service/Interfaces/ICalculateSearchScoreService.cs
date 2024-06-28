using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICalculateSearchScoreService
    {

        /// <summary>
        /// Tinh toan search score giua search text va list knowledge rut gon
        /// </summary>
        /// <param name="search"></param>
        /// <param name="listKnowledges"> Danh sach thong tin rut gon cua danh sachs knowledge </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Dictionary<Guid, double> CalculateKnowledgeScore(string search, List<(Guid UserItemId, string Title, string Fullname, string? Abstract, int? sumStar, int? totalStar)> listKnowledges);

        /// <summary>
        /// Tinh toan search score giua search text va list user rut gon
        /// </summary>
        /// <param name="search"></param>
        /// <param name="listUsers"> Danh sach thong tin rut gon cua danh sach user </param>
        /// <returns></returns>
        /// Created PhucTV (19/5/24)
        /// Modified None
        Dictionary<Guid, double> CalculateUserScore(string search, List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber, int? sumStar, int? totalStar)> listUsers);

        Dictionary<Guid, double> CalculateUserSimiliarityScore(string search, List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber)> listUsers);
        
        Dictionary<Guid, double> CalculateKnowledgeSimiliarityScore(string search, List<(Guid UserItemId, string Title, string Fullname, string? Abstract)> lsKnowledges);

        Dictionary<Guid, double> CalculateStarScore(List<(Guid id, int? sumStar, int? totalStar)> listItems);
    }
}
