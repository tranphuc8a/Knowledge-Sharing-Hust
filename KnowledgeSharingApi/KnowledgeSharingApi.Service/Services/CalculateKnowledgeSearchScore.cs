using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CalculateKnowledgeSearchScore : ICalculateKnowledgeSearchScore
    {
        public Dictionary<Guid, double> Calculate(string search, List<(Guid UserItemId, string Title, string Fullname, string Content, string? Abstract)> listKnowledges)
        {
            // init Weight:
            search = search.ToLower();
            double titleWeight = 0.4, fullnameWeight = 0.25, contentWeight = 0.2, abstractWeight = 0.15;

            // init list Items:
            List<string> listTitle = listKnowledges.Select(kn => kn.Title).ToList();
            List<string> listFullname = listKnowledges.Select(kn => kn.Fullname).ToList();
            List<string> listContent = listKnowledges.Select(kn => kn.Content).ToList();
            List<string> listAbstract = listKnowledges.Select(kn => kn.Abstract ?? "").ToList();

            // calculate for each list items
            Dictionary<string, double> titleScore = Algorithm.NgramSimilarityList(search, listTitle);
            Dictionary<string, double> fullnameScore = Algorithm.NgramSimilarityList(search, listFullname);
            Dictionary<string, double> contentScore = Algorithm.NgramSimilarityList(search, listContent);
            Dictionary<string, double> abstractScore = Algorithm.NgramSimilarityList(search, listAbstract);

            // combine items
            Dictionary<Guid, double> res = listKnowledges.ToDictionary(
                kn => kn.UserItemId,
                kn => titleWeight * titleScore[kn.Title] + 
                      fullnameWeight * fullnameScore[kn.Fullname] + 
                      contentWeight * contentScore[kn.Content] + 
                      abstractWeight * abstractScore[kn.Abstract ?? ""]
            );

            return res;
        }
    }
}
