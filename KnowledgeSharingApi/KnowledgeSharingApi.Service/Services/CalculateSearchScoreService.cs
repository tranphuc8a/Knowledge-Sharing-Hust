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
    public class CalculateSearchScoreService(IMarkdownConverter markdownConverter) : ICalculateSearchScoreService
    {
        protected readonly IMarkdownConverter MarkdownConverter = markdownConverter;

        public virtual Dictionary<Guid, double> CalculateKnowledgeScore(string search, List<(Guid UserItemId, string Title, string Fullname, string? Abstract, int? sumStar, int? totalStar)> listKnowledges)
        {
            listKnowledges = listKnowledges.GroupBy(kn => kn.UserItemId).Select(gr => gr.First()).ToList();
            Dictionary<Guid, double> simScore = CalculateKnowledgeSimiliarityScore(search, listKnowledges.Select(a => (a.UserItemId, a.Title, a.Fullname, a.Abstract)).ToList());
            Dictionary<Guid, double> starScore = CalculateStarScore(listKnowledges.Select(item => (item.UserItemId, item.sumStar, item.totalStar)).ToList());

            Dictionary<Guid, double> res = listKnowledges.ToDictionary(
                kn => kn.UserItemId,
                kn =>
                {
                    double sims = 0.0001, stars = 0.5;
                    if (simScore.TryGetValue(kn.UserItemId, out double value))
                    {
                        sims = value;
                    }
                    if (starScore.TryGetValue(kn.UserItemId, out double value2))
                    {
                        stars = value2;
                    }
                    return 2 * sims * stars / (sims + stars);
                }
            );
            return res;
        }

        public virtual Dictionary<Guid, double> CalculateKnowledgeSimiliarityScore(string search, List<(Guid UserItemId, string Title, string Fullname, string? Abstract)> listKnowledges)
        {
            listKnowledges = listKnowledges
                .GroupBy(item => item.UserItemId)
                .Select(group => group.First())
                .ToList();
            if (listKnowledges.Count <= 0) return [];

            // init Weight:
            search = search.ToLower();
            double titleWeight = 0.6, fullnameWeight = 0.2, 
                //contentWeight = 0.2, 
                abstractWeight = 0.2;

            // init list Items:
            //Dictionary<Guid, string> markdownConverted = listKnowledges
            //    .ToDictionary(
            //        item => item.UserItemId,
            //        item => MarkdownConverter.GetPureText(item.Content)
            //    );
            List<string> listTitle = listKnowledges.Select(kn => kn.Title).Distinct().ToList();
            List<string> listFullname = listKnowledges.Select(kn => kn.Fullname).Distinct().ToList();
            //List<string> listContent = listKnowledges.Select(kn => markdownConverted[kn.UserItemId]).Distinct().ToList();
            List<string> listAbstract = listKnowledges.Select(kn => kn.Abstract ?? "").Distinct().ToList();

            // calculate for each list items
            Dictionary<string, double> titleScore = Algorithm.SimilarityList(search, listTitle);
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, listFullname);
            //Dictionary<string, double> contentScore = Algorithm.SimilarityList(search, listContent);
            Dictionary<string, double> abstractScore = Algorithm.SimilarityList(search, listAbstract);

            // combine items
            Dictionary<Guid, double> res = listKnowledges.ToDictionary(
                kn => kn.UserItemId,
                kn => titleWeight * titleScore[kn.Title] + 
                      fullnameWeight * fullnameScore[kn.Fullname] + 
                      //contentWeight * contentScore[markdownConverted[kn.UserItemId]] + 
                      abstractWeight * abstractScore[kn.Abstract ?? ""]
            );

            // Tìm maxScore
            double rounder = 0.000001;
            double maxScore = res.Values.Max() + rounder;

            // Chuẩn hóa các giá trị trong từ điển res về khoảng [0, 1]
            Dictionary<Guid, double> normalizedRes = res.ToDictionary(
                kvp => kvp.Key,
                kvp => (kvp.Value + rounder) / (maxScore + rounder)
            );

            return normalizedRes;
        }

        public virtual Dictionary<Guid, double> CalculateStarScore(List<(Guid id, int? sumStar, int? totalStar)> listItems)
        {
            if (listItems.Count <= 0) return [];

            const double rounder = 1e-2;

            Dictionary<Guid, double> scoreAverageStar = listItems.ToDictionary(
                item => item.id,
                item =>
                {
                    if (item.sumStar == null || item.totalStar == null || item.totalStar == 0)
                        return rounder / (5 + rounder);
                    return (double)(item.sumStar * 1.0 / item.totalStar + rounder) / (5 + rounder);
                }
            );

            int maxTotalStar = listItems.Select(item => (item.totalStar ?? 0 + 1)).Max();

            Dictionary<Guid, double> scoreTotalStar = listItems.ToDictionary(
                item => item.id,
                item =>
                {
                    return (item.totalStar ?? 0 + 1) * 1.0 / maxTotalStar;
                }
            );

            Dictionary<Guid, double> res = listItems.ToDictionary(
                it => it.id,
                it =>
                {
                    double f1score = 2 / (1 / scoreAverageStar[it.id] + 1 / scoreTotalStar[it.id]);
                    return 0.5 + f1score / 2;
                } // norm into [0.5; 1]
            );

            return res;
        }

        public virtual Dictionary<Guid, double> CalculateUserScore(string search, List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber, int? sumStar, int? totalStar)> listUsers)
        {
            listUsers = listUsers.GroupBy(kn => kn.UserId).Select(gr => gr.First()).ToList();
            Dictionary<Guid, double> simScore = CalculateUserSimiliarityScore(search, listUsers.Select(a => (a.UserId, a.FullName, a.Username, a.Email, a.PhoneNumber)).ToList());
            Dictionary<Guid, double> starScore = CalculateStarScore(listUsers.Select(item => (item.UserId, item.sumStar, item.totalStar)).ToList());

            Dictionary<Guid, double> res = listUsers.ToDictionary(
                kn => kn.UserId,
                kn =>
                {
                    double sims = 0.0001, stars = 0.5;
                    if (simScore.TryGetValue(kn.UserId, out double value))
                    {
                        sims = value;
                    }
                    if (starScore.TryGetValue(kn.UserId, out double value2))
                    {
                        stars = value2;
                    }
                    return 2 * sims * stars / (sims + stars);
                }
            );
            return res;
        }

        public virtual Dictionary<Guid, double> CalculateUserSimiliarityScore(string search, List<(Guid UserId, string FullName, string Username, string Email, string? PhoneNumber)> listUsers)
        {
            listUsers = listUsers
                .GroupBy(item => item.UserId)
                .Select(group => group.First())
                .ToList();
            if (listUsers.Count <= 0) return [];

            // init Weight:
            search = search.ToLower();
            double fullnameWeight = 0.4, usernameWeight = 0.3, emailWeight = 0.2, phoneWeight = 0.1;

            // init list Items:
            List<string> listFullname = listUsers.Select(u => u.FullName).Distinct().ToList();
            List<string> listUsername = listUsers.Select(u => u.Username).Distinct().ToList();
            List<string> listEmail = listUsers.Select(u => u.Email).Distinct().ToList();
            List<string> listPhoneNumber = listUsers.Select(u => u.PhoneNumber ?? "").Distinct().ToList();

            // calculate for each list items
            Dictionary<string, double> fullnameScore = Algorithm.SimilarityList(search, listFullname);
            Dictionary<string, double> usernameScore = Algorithm.SimilarityList(search, listUsername);
            Dictionary<string, double> emailScore = Algorithm.SimilarityList(search, listEmail);
            Dictionary<string, double> phoneScore = Algorithm.SimilarityList(search, listPhoneNumber);

            // combine items
            Dictionary<Guid, double> res = listUsers.ToDictionary(
                u => u.UserId,
                u => fullnameWeight * fullnameScore[u.FullName] +
                      usernameWeight * usernameScore[u.Username] +
                      emailWeight * emailScore[u.Email] +
                      phoneWeight * phoneScore[u.PhoneNumber ?? ""]
            );

            // Tìm maxScore
            double rounder = 1e6;
            double maxScore = res.Values.Max() + rounder;

            // Chuẩn hóa các giá trị trong từ điển res về khoảng [0, 1]
            Dictionary<Guid, double> normalizedRes = res.ToDictionary(
                kvp => kvp.Key,
                kvp => (kvp.Value + rounder) / (maxScore + rounder)
            );

            return normalizedRes;
        }
    }
}
