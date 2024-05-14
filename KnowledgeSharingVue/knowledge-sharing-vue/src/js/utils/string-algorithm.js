

class StringAlgorithm{
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

    static normalizedLevenshteinDistance(a, b) {
        const maxLen = Math.max(a.length, b.length);
        if (maxLen === 0) {
            return 0;
        }
    
        return this.calculateLevenshteinDistance(a, b) / maxLen;
    }

    static jaccardSimilarity(a, b) {
        const intersection = new Set([...a].filter(x => b.has(x)));
        const union = new Set([...a].concat([...b]));
        return intersection.size / union.size;
    }


    static similiar(a, b) {
        const levenshteinDistance = this.normalizedLevenshteinDistance(a, b);
        const jaccardSimilarity = this.jaccardSimilarity(new Set(a), new Set(b));
        return (levenshteinDistance + jaccardSimilarity) / 2;
    }
}


export default StringAlgorithm;
