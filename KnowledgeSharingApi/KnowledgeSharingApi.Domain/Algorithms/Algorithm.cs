using KnowledgeSharingApi.Domains.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Algorithms
{
    public static class Algorithm
    {
        private static readonly string[] separator = [" ", ",", ".", ";", "!", ":", "?"];

        public static bool IsPermutation<T>(List<T> A, List<T> B)
        {
            // Kiểm tra xem hai List có cùng độ dài không
            if (A.Count != B.Count)
                return false;

            // Tạo một Dictionary để đếm số lần xuất hiện của từng phần tử trong List B
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            Dictionary<T, int> count = [];
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            foreach (var item in B)
            {
                if (count.TryGetValue(item, out int value))
                    count[item] = ++value;
                else
                    count[item] = 1;
            }

            // Kiểm tra từng phần tử trong List A
            foreach (T item in A)
            {
                // Nếu phần tử không tồn tại trong Dictionary hoặc đã xuất hiện quá số lần trong Dictionary, trả về false
                if (!count.TryGetValue(item, out int value) || value == 0)
                    return false;

                // Giảm số lần xuất hiện của phần tử trong Dictionary
                count[item] = --value;
            }

            // Nếu đã kiểm tra tất cả các phần tử trong A và chúng đều xuất hiện trong B với số lần phù hợp, trả về true
            return true;
        }

        public static int LongestCommonSubsequence(string text1, string text2)
        {
            int[,] dp = new int[text1.Length + 1, text2.Length + 1];
            for (int i = 0; i < text1.Length; i++)
            {
                for (int j = 0; j < text2.Length; j++)
                {
                    if (text1[i] == text2[j])
                    {
                        dp[i + 1, j + 1] = dp[i, j] + 1;
                    }
                    else
                    {
                        dp[i + 1, j + 1] = Math.Max(dp[i, j + 1], dp[i + 1, j]);
                    }
                }
            }
            return dp[text1.Length, text2.Length];
        }

        public static double CalculateLCSimilarity(string text1, string text2)
        {
            int lcsLength = LongestCommonSubsequence(text1, text2);
            int maxLength = Math.Max(text1.Length, text2.Length);

            if (maxLength == 0)
            {
                return 1.0; // Cả hai chuỗi đều rỗng
            }

            return (double)lcsLength / maxLength;
        }

        public static int LongestCommonSubsequenceContinuous(string str1, string str2)
        {
            int[,] lengths = new int[str1.Length + 1, str2.Length + 1];

            // Build the lengths array in bottom-up manner
            for (int i = 0; i <= str1.Length; i++)
            {
                for (int j = 0; j <= str2.Length; j++)
                {
                    if (i == 0 || j == 0)
                        lengths[i, j] = 0;
                    else if (str1[i - 1] == str2[j - 1])
                        lengths[i, j] = lengths[i - 1, j - 1] + 1;
                    else
                        lengths[i, j] = Math.Max(lengths[i - 1, j], lengths[i, j - 1]);
                }
            }

            // Return the length of longest common subsequence
            return lengths[str1.Length, str2.Length];
        }

        public static double CalculateLCSCimilarity(string str1, string str2)
        {
            int lcscLength = LongestCommonSubsequenceContinuous(str1, str2);
            int maxLength = Math.Max(str1.Length, str2.Length);

            if (maxLength == 0)
            {
                return 1.0; // Cả hai chuỗi đều rỗng
            }

            return (double)lcscLength / maxLength;
        }

        public static int CalculateLevenshteinDistance(string a, string b)
        {
            int n = a.Length;
            int m = b.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        public static double CalculateLevenshteinSimilarity(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                return 0.0;
            }

            int levenshteinDistance = CalculateLevenshteinDistance(a, b);
            int maxLength = Math.Max(a.Length, b.Length);

            // Đảm bảo không chia cho 0
            if (maxLength == 0)
            {
                return 1.0; // cả hai chuỗi đều rỗng
            }

            double similarity = 1.0 - (double)levenshteinDistance / maxLength;
            return similarity;
        }

        public static double CalculateJaccardSimilarity(string a, string b)
        {
            // Kiểm tra nếu một trong hai chuỗi là null hoặc rỗng
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                return 0.0;
            }

            // Tạo các tập hợp ký tự từ hai chuỗi
            HashSet<char> setA = new(a);
            HashSet<char> setB = new(b);

            // Tính giao của hai tập hợp
            HashSet<char> intersection = new(setA);
            intersection.IntersectWith(setB);

            // Tính hợp của hai tập hợp
            HashSet<char> union = new(setA);
            union.UnionWith(setB);

            // Tính điểm số Jaccard
            double jaccardScore = (double)intersection.Count / union.Count;
            return jaccardScore;
        }


        private static Dictionary<string, int> GetMapGram(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return [];

            var mapGram = new Dictionary<string, int>();
            var words = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                // If word is longer than 100 characters, trim it
                var processedWord = word.Length > 100 ? word[..100] : word;

                int wordLength = processedWord.Length;
                for (int l = 1; l <= wordLength; l++)
                {
                    for (int startIndex = 0; startIndex <= wordLength - l; startIndex++)
                    {
                        string gram = processedWord.Substring(startIndex, l);
                        if (!mapGram.TryGetValue(gram, out int value))
                        {
                            mapGram[gram] = 1;
                        }
                        else
                        {
                            mapGram[gram] = value + 1;
                        }
                    }
                }
            }

            return mapGram;
        }

        /**
         * Calculates the similarity between two strings using the n-gram similarity algorithm
         * @param a 
         * @param b 
         * @returns double in [0, 1]
         * @Created PhucTV (17/5/24)
         * @Modified None
         */
        public static double NgramSimilarity(string a, string b)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                    return 0;

                Dictionary<string, int> mapGramOfStringA = GetMapGram(a);
                Dictionary<string, int> mapGramOfStringB = GetMapGram(b);

                double score = 0;
                foreach (var gram in mapGramOfStringA)
                {
                    if (mapGramOfStringB.TryGetValue(gram.Key, out int value))
                    {
                        int countInA = gram.Value;
                        int countInB = value;
                        int gramLength = gram.Key.Length;
                        double addScore = countInA * countInB * gramLength * gramLength;
                        score += addScore;
                    }
                }

                return score;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 0;
            }
        }

        public static Dictionary<string, double> NgramSimilarityList(string search, List<string> listText)
        {
            try
            {
                Dictionary<string, double> mapScore = [];
                if (string.IsNullOrEmpty(string.Join("", listText)))
                    return mapScore;
                foreach (var text in listText)
                {
                    mapScore[text] = 0;
                }
                // calculate n-gram of search string
                Dictionary<string, int> mapGramOfStringA = GetMapGram(Unicode.RemoveVietnameseTone(search).ToLower());

                // calculate n-gram of each text in listText
                foreach (var text in listText)
                {
                    Dictionary<string, int> mapGramOfStringB = GetMapGram(Unicode.RemoveVietnameseTone(text).ToLower());
                    double score = 0;
                    foreach (var gram in mapGramOfStringA)
                    {
                        if (mapGramOfStringB.TryGetValue(gram.Key, out int value))
                        {
                            int countInA = gram.Value;
                            int countInB = value;
                            int gramLength = gram.Key.Length;
                            double addScore = countInA * countInB * gramLength * gramLength / 1e3;
                            score += addScore;
                        }
                    }
                    mapScore[text] = score;
                }
                return mapScore;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return [];
            }
        }



        public static Dictionary<string, double> SimilarityList(string search, List<string> listText)
        {
            return NgramSimilarityList(search, listText);
        }

        public static double CalculateCompositeSimilarity(string a, string b)
        {
            double levenshteinSimilarity = CalculateLevenshteinSimilarity(a, b);
            double jaccardSimilarity = CalculateJaccardSimilarity(a, b);
            double lcsSimilarity = CalculateLCSimilarity(a, b);
            double lcscSimilarity = CalculateLCSCimilarity(a, b);

            // Kết hợp các độ tương đồng bằng trung bình cộng
            double compositeSimilarity = (levenshteinSimilarity + jaccardSimilarity + lcsSimilarity + lcscSimilarity) / 4;

            return compositeSimilarity;
        }




        public static double AggregateUsingMax(params double[] criteria)
        {
            return criteria.Max();
        }
        
        public static double AggregateWithWeight(params double[] criteria)
        {
            double product = 1.0;
            foreach (double criterion in criteria)
            {
                product *= Math.Pow(criterion, 0.5); // Sử dụng căn bậc hai để tăng tốc độ khi giá trị gần 1
            }
            return product;
        }
        
        public static double AggregateWithBoost(params double[] criteria)
        {
            double sum = 0.0;
            double boost = 0.0;

            foreach (double criterion in criteria)
            {
                sum += criterion;
                boost += 1 - Math.Pow(1 - criterion, 5); // Sử dụng lũy thừa lớn để tăng tốc độ khi giá trị gần 1
            }

            double average = sum / criteria.Length;
            double boostedAverage = (average + boost) / (1 + criteria.Length); // Kết hợp với yếu tố bù đắp

            return boostedAverage;
        }

        public static double MaxCriteria(params double[] criteria)
        {
            return criteria.Max();
        }

        public static double ExponentialCriteria(params double[] criteria)
        {
            double sum = criteria.Sum();
            return 1 - Math.Exp(-sum);
        }

        public static double YagerCriteria(double p, params double[] criteria)
        {
            double sumOfPowers = criteria.Select(a => Math.Pow(1 - a, p)).Sum();
            return 1 - Math.Min(1, Math.Pow(sumOfPowers, 1 / p));
        }
    }
}
