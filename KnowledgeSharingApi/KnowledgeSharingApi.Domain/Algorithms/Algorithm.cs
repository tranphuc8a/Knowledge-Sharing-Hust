using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Algorithms
{
    public static class Algorithm
    {
        public static bool IsPermutation<T>(IEnumerable<T> A, IEnumerable<T> B)
        {
            // Kiểm tra xem hai IEnumerable có cùng độ dài không
            if (A.Count() != B.Count())
                return false;

            // Tạo một Dictionary để đếm số lần xuất hiện của từng phần tử trong IEnumerable B
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

            // Kiểm tra từng phần tử trong IEnumerable A
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

    }
}
