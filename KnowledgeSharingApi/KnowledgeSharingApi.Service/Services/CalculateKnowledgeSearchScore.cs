using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Infrastructures.Interfaces.Markdown;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CalculateKnowledgeSearchScore(IMarkdownConverter markdownConverter) : ICalculateKnowledgeSearchScore
    {
        protected readonly IMarkdownConverter MarkdownConverter = markdownConverter;

        public virtual Dictionary<Guid, double> Calculate(string search, List<(Guid UserItemId, string Title, string Fullname, string Content, string? Abstract)> listKnowledges)
        {
            // init Weight:
            search = search.ToLower();
            double titleWeight = 0.4, fullnameWeight = 0.3, contentWeight = 0.2, abstractWeight = 0.1;

            // init list Items:
            Dictionary<Guid, string> markdownConverted = listKnowledges.ToDictionary(
                item => item.UserItemId,
                item => MarkdownConverter.GetPureText(item.Content)
            );
            List<string> listTitle = listKnowledges.Select(kn => kn.Title).Distinct().ToList();
            List<string> listFullname = listKnowledges.Select(kn => kn.Fullname).Distinct().ToList();
            List<string> listContent = listKnowledges.Select(kn => markdownConverted[kn.UserItemId]).Distinct().ToList();
            List<string> listAbstract = listKnowledges.Select(kn => kn.Abstract ?? "").Distinct().ToList();

            // calculate for each list items
            Dictionary<string, double> titleScore = Algorithm.SimilarityList(search, listTitle);
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, listFullname);
            Dictionary<string, double> contentScore = Algorithm.SimilarityList(search, listContent);
            Dictionary<string, double> abstractScore = Algorithm.SimilarityList(search, listAbstract);

            // combine items
            Dictionary<Guid, double> res = listKnowledges.ToDictionary(
                kn => kn.UserItemId,
                kn => titleWeight * titleScore[kn.Title] + 
                      fullnameWeight * fullnameScore[kn.Fullname] + 
                      contentWeight * contentScore[markdownConverted[kn.UserItemId]] + 
                      abstractWeight * abstractScore[kn.Abstract ?? ""]
            );

            return res;
        }
    }
}
