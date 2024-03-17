using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Algorithms
{
    public static class Algorithm
    {
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
    }
}
