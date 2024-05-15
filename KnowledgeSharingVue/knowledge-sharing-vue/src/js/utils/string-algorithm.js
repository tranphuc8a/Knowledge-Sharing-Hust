
import Criteria from "./criteria";

class StringAlgorithm{
    static longestCommonSubsequence(text1, text2) {
        let dp = new Array(text1.length + 1).fill(0).map(() => new Array(text2.length + 1).fill(0));
        for (let i = 0; i < text1.length; i++) {
            for (let j = 0; j < text2.length; j++) {
                if (text1[i] === text2[j]) {
                    dp[i + 1][j + 1] = dp[i][j] + 1;
                } else {
                    dp[i + 1][j + 1] = Math.max(dp[i][j + 1], dp[i + 1][j]);
                }
            }
        }
        return dp[text1.length][text2.length];
    }
    static calculateLCSimilarity(text1, text2) {
        let lcsLength = this.longestCommonSubsequence(text1, text2);
        let maxLength = this.conditionalAverage(text1.length, text2.length);
    
        if (maxLength === 0) {
            return 1.0; // Both strings are empty
        }
    
        return lcsLength / maxLength;
    }

    static longestCommonSubsequenceContinuous(str1, str2) {
        let lengths = new Array(str1.length + 1).fill(0).map(() => new Array(str2.length + 1).fill(0));
    
        // Build the lengths array in bottom-up manner
        for (let i = 0; i <= str1.length; i++) {
            for (let j = 0; j <= str2.length; j++) {
                if (i === 0 || j === 0)
                    lengths[i][j] = 0;
                else if (str1[i - 1] === str2[j - 1])
                    lengths[i][j] = lengths[i - 1][j - 1] + 1;
                else
                    lengths[i][j] = Math.max(lengths[i - 1][j], lengths[i][j - 1]);
            }
        }
    
        // Return the length of longest common subsequence
        return lengths[str1.length][str2.length];
    }
    
    static calculateLCSCimilarity(str1, str2) {
        let lcscLength = this.longestCommonSubsequenceContinuous(str1, str2);
        let maxLength = this.conditionalAverage(str1.length, str2.length);
    
        if (maxLength === 0) {
            return 1.0; // Both strings are empty
        }
    
        return (lcscLength / maxLength);
    }


    static calculateLevenshteinDistance(a, b) {
        const n = a.length;
        const m = b.length;
        const d = Array.from(Array(n + 1), () => Array(m + 1).fill(0));
    
        if (n === 0) {
            return m;
        }
    
        if (m === 0) {
            return n;
        }
    
        for (let i = 0; i <= n; i++) {
            d[i][0] = i;
        }
        for (let j = 0; j <= m; j++) {
            d[0][j] = j;
        }
    
        for (let i = 1; i <= n; i++) {
            for (let j = 1; j <= m; j++) {
                const cost = (b[j - 1] === a[i - 1]) ? 0 : 1;
    
                d[i][j] = Math.min(
                    d[i - 1][j] + 1,
                    d[i][j - 1] + 1,
                    d[i - 1][j - 1] + cost
                );
            }
        }
    
        return d[n][m];
    }

    static calculateLevenshteinSimilarity = (a, b) => {
        if (!a || !b) {
            return 0.0;
        }
    
        const levenshteinDistance = this.calculateLevenshteinDistance(a, b);
        const maxLength = Math.max(a.length, b.length);
    
        // Ensure not dividing by 0
        if (maxLength === 0) {
            return 1.0; // both strings are empty
        }
    
        const similarity = 1.0 - (levenshteinDistance / maxLength);
        return similarity;
    }

    static jaccardSimilarity(a, b) {
        const setA = new Set(a);
        const setB = new Set(b);
        const intersection = new Set([...setA].filter(x => setB.has(x)));
        const union = new Set([...setA, ...setB]);
        return intersection.size / union.size;
    }

    static averageLength(a, b){
        if (a.length <= 0 || b.length <= 0) return 1;
        return 2 / (1/a.length + 1/b.length);
    }
    static conditionalAverage(a, b){
        if (a <= 0 || b <= 0) return 1;
        return 2 / (1/a + 1/b);
    }

    static similiar(a, b) {
        if (a.length <= 0 || b.length <= 0) return 0;

        let jacScore = this.jaccardSimilarity(a, b);
        let levScore = this.calculateLevenshteinSimilarity(a, b);
        let lcsScore = this.calculateLCSimilarity(a, b);
        let lcscScore = this.calculateLCSCimilarity(a, b);

        let jacWeight = 1;
        let lcsWeight = 3;
        let levWeight = 3;
        let lcscWeight = 10;

        let res = Criteria.aggregateUsingAverage(jacScore*jacWeight, lcsScore*lcsWeight, levScore*levWeight, lcscScore*lcscWeight);
        return res;
    }
}


export default StringAlgorithm;
