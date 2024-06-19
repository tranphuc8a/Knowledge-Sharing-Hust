using KnowledgeSharingApi.Domains.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KnowledgeSharingApi.Domains.Algorithms
{
    public static class Algorithm
    {
        //private static readonly string[] separator = [" ", ",", ".", ";", "!", ":", "?"];
        private static char[] separator = [' ', '\t', '\n', '\r', ',', '.', ';', '!', '?'];

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


        private static Dictionary<string, int> GetMapGramCharacter(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return [];

            var mapGram = new Dictionary<string, int>();
            var words = text.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            int maxLengthSentences = 200;
            if (words.Count > 200) words = words[..maxLengthSentences];

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

        private static Dictionary<string, int> GetMapGramCharacterPattern(string text, Dictionary<string, int> patternMapGram)
        {
            if (string.IsNullOrEmpty(text)) return [];

            var mapGram = new Dictionary<string, int>();
            var words = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var wordLength = word.Length;
                for (var startIndex = 0; startIndex < wordLength; startIndex++)
                {
                    var maxLength = wordLength - startIndex;
                    for (var l = 1; l <= maxLength; l++)
                    {
                        var gram = word.Substring(startIndex, l);
                        if (string.IsNullOrWhiteSpace(gram))
                            continue;
                        if (!patternMapGram.TryGetValue(gram, out var value) || value == 0)
                            break;
                        if (!mapGram.TryGetValue(gram, out int value2))
                            mapGram[gram] = 1;
                        else
                            mapGram[gram] = ++value2;
                    }
                }
            }
            return mapGram;
        }

        private static Dictionary<string, int> GetMapGramWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return [];

            var mapGram = new Dictionary<string, int>();
            List<string> words = [.. text.Split(separator, StringSplitOptions.RemoveEmptyEntries)];

            int maxWords = 100; // only focus first 100 words of text
            if (words.Count > maxWords)
                words = words.Take(maxWords).ToList();

            int sentenceLength = words.Count;

            for (int l = 1; l <= sentenceLength; l++)
            {
                for (int startIndex = 0; startIndex <= sentenceLength - l; startIndex++)
                {
                    string gram = string.Join(" ", words.Skip(startIndex).Take(l));
                    if (string.IsNullOrWhiteSpace(gram)) continue;
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

            return mapGram;
        }

        private static Dictionary<string, int> GetMapGramWordPattern(string text, Dictionary<string, int> patternMapGram)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return [];

            var mapGram = new Dictionary<string, int>();
            var words = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            const int maxWords = 100; // only focus first 100 words of text
            if (words.Length > maxWords)
            {
                Array.Resize(ref words, maxWords);
            }

            var sentenceLength = words.Length;

            for (var startIndex = 0; startIndex < sentenceLength; startIndex++)
            {
                var maxLength = sentenceLength - startIndex;
                for (var l = 1; l <= maxLength; l++)
                {
                    var gram = string.Join(" ", words, startIndex, l);
                    if (string.IsNullOrWhiteSpace(gram))
                        continue;
                    if (!patternMapGram.TryGetValue(gram, out var value) || value == 0)
                        break;
                    if (!mapGram.TryGetValue(gram, out int value2))
                        mapGram[gram] = 1;
                    else
                        mapGram[gram] = ++value;
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
        public static double NgramCharacterSimilarity(string a, string b)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                    return 0;

                Dictionary<string, int> mapGramOfStringA = GetMapGramCharacter(Unicode.RemoveVietnameseTone(a).ToLower());
                Dictionary<string, int> mapGramOfStringB = GetMapGramCharacter(Unicode.RemoveVietnameseTone(b).ToLower());

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

                return score;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 0;
            }
        }

        public static Dictionary<string, double> NgramCharacterSimilarityList(string search, List<string> listText)
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
                Dictionary<string, int> mapGramOfStringA = GetMapGramCharacter(Unicode.RemoveVietnameseTone(search).ToLower());

                // calculate n-gram of each text in listText
                foreach (var text in listText)
                {
                    Dictionary<string, int> mapGramOfStringB = GetMapGramCharacterPattern(Unicode.RemoveVietnameseTone(text).ToLower(), mapGramOfStringA);
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

        /**
         * Calculates the similarity between two strings using the n-gram similarity algorithm
         * @param a 
         * @param b 
         * @returns double in [0, 1]
         * @Created PhucTV (17/5/24)
         * @Modified None
         */
        public static double NgramWordSimilarity(string a, string b)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                    return 0;

                Dictionary<string, int> mapGramOfStringA = GetMapGramWord(Unicode.RemoveVietnameseTone(a).ToLower());
                Dictionary<string, int> mapGramOfStringB = GetMapGramWord(Unicode.RemoveVietnameseTone(b).ToLower());

                double score = 0;
                foreach (var gram in mapGramOfStringA)
                {
                    if (mapGramOfStringB.TryGetValue(gram.Key, out int value))
                    {
                        int countInA = gram.Value;
                        int countInB = value;
                        int gramLength = gram.Key.Length;
                        double addScore = countInA * countInB * gramLength * gramLength / 1e2;
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

        public static Dictionary<string, double> NgramWordSimilarityList(string search, List<string> listText)
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
                Dictionary<string, int> mapGramOfStringA = GetMapGramWord(Unicode.RemoveVietnameseTone(search).ToLower());

                // calculate n-gram of each text in listText
                foreach (var text in listText)
                {
                    Dictionary<string, int> mapGramOfStringB = GetMapGramWordPattern(Unicode.RemoveVietnameseTone(text).ToLower(), mapGramOfStringA);
                    double score = 0;
                    foreach (var gram in mapGramOfStringA)
                    {
                        if (mapGramOfStringB.TryGetValue(gram.Key, out int value))
                        {
                            int countInA = gram.Value;
                            int countInB = value;
                            int gramLength = gram.Key.Length;
                            double addScore = countInA * countInB * gramLength * gramLength / 2e2;
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
            Dictionary<string, double> characterLevels = NgramCharacterSimilarityList(search, listText);
            Dictionary<string, double> wordLevels = NgramWordSimilarityList(search, listText);
            return listText.Distinct().ToDictionary(
                text => text,
                text =>
                {
                    double characterScore = characterLevels.TryGetValue(text, out double value) ? value : 0;
                    double wordScore = wordLevels.TryGetValue(text, out double value2) ? value2 : 0;
                    return characterScore + wordScore;
                }
            );
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
