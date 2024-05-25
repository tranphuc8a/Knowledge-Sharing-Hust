
// import Criteria from "./criteria";
import { Validator } from "./validator";
import { Unicode } from "./unicode";

class StringAlgorithm{
    /**
     * Calculate the longest common subsequence between two strings
     * @param {*} text1 
     * @param {*} text2 
     * @returns integer - length of longest subsequence
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
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
    /**
     * Calculates the similarity between two strings using the longest common subsequence algorithm
     * @param {*} text1 
     * @param {*} text2 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static calculateLCSimilarity(text1, text2) {
        let lcsLength = this.longestCommonSubsequence(text1, text2);
        let maxLength = this.conditionalAverage(text1.length, text2.length);
    
        if (maxLength === 0) {
            return 1.0; // Both strings are empty
        }
    
        return lcsLength / maxLength;
    }
    
    /**
     * Calculate the longest common subsequence continuous between two strings
     * @param {*} str1 
     * @param {*} str2 
     * @returns integer - length of longest subsequence
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
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
    /**
     * Calculates the similarity between two strings using the longest common subsequence continuous algorithm
     * @param {*} str1 
     * @param {*} str2 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static calculateLCSCimilarity(str1, str2) {
        let lcscLength = this.longestCommonSubsequenceContinuous(str1, str2);
        let maxLength = this.conditionalAverage(str1.length, str2.length);
    
        if (maxLength === 0) {
            return 1.0; // Both strings are empty
        }
    
        return (lcscLength / maxLength);
    }

    /**
     * Calculate the levenshtein distance between two strings
     * @param {*} a 
     * @param {*} b 
     * @returns integer - number of steps to change the string a to string b
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
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

    /**
     * Calculate the similarity between two strings using the levenshtein distance algorithm
     * @param {*} a 
     * @param {*} b 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
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

    /**
     * Calculate the similarity between two strings using the jaccard similarity algorithm
     * @param {*} a 
     * @param {*} b 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static jaccardSimilarity(a, b) {
        const setA = new Set(a);
        const setB = new Set(b);
        const intersection = new Set([...setA].filter(x => setB.has(x)));
        const union = new Set([...setA, ...setB]);
        return intersection.size / union.size;
    }

    /**
     * Calculates the similarity between two strings using the n-gram similarity algorithm
     * @param {*} a 
     * @param {*} b 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static ngramCharacterSimilarity(a, b) {
        try {
            if (Validator.isEmpty(a) || Validator.isEmpty(b)) 
                return 0;

            let mapGramOfStringA = this.getMapGramCharacter(a);
            let mapGramOfStringB = this.getMapGramCharacter(b);

            let score = 0;
            for (let gram in mapGramOfStringA) {
                if (mapGramOfStringB[gram]!== undefined) {
                    let countInA = mapGramOfStringA[gram];
                    let countInB = mapGramOfStringB[gram];
                    let gramLength = gram.length;
                    let addScore = countInA * countInB * gramLength * gramLength / 1000;
                    score += addScore;
                }
            }

            return score;
        } catch (e){
            console.error(e);
            return 0;
        }
    }

    static ngramCharacterSimilarityList(search, listText){
        try {
            let mapScore = {};
            if (Validator.isEmpty(listText)) return mapScore;
            for (let text in listText){
                mapScore[text] = 0;
            }
            // calculate n-gram of search string
            let mapGramOfStringA = this.getMapGramCharacter(Unicode.unicodeToAscii(search).toLowerCase());

            // calculate n-gram of each text in listText
            for (let text of listText){
                let mapGramOfStringB = this.getMapGramCharacter(Unicode.unicodeToAscii(text).toLowerCase());
                let score = 0;
                for (let gram in mapGramOfStringA) {
                    if (mapGramOfStringB[gram] !== undefined) {
                        let countInA = mapGramOfStringA[gram];
                        let countInB = mapGramOfStringB[gram];
                        let gramLength = gram.length;
                        let addScore = countInA * countInB * gramLength * gramLength / 1000;
                        score += addScore;
                    }
                }
                mapScore[text] = score;
            }
            return mapScore;
        } catch (e) {
            console.error(e);
            return {};
        }
    }

    static getMapGramCharacter(text){
        if (Validator.isEmpty(text)) return {};
        let mapGram = {};
        let words = text.split(" ").filter(word => word.length > 0);
        for (let word of words) {
            let wordLength = word.length;
            for (let l = 1; l <= wordLength; l++) {
                for (let startIndex = 0; startIndex <= wordLength - l; startIndex++) {
                    let gram = word.substring(startIndex, startIndex + l);
                    if (mapGram[gram] === undefined) {
                        mapGram[gram] = 1;
                    } else {
                        mapGram[gram]++;
                    }
                }
            }
        }
        return mapGram;
    }

    static getMapGramWord(text) {
        if (!text || text.trim() === '') {
            return {};
        }
        
        const mapGram = {};
        const words = text.split(/\s+/).filter(word => word !== '');
        
        const maxWords = 100; // only focus first 100 words of text
        if (words.length > maxWords) {
            words.splice(maxWords);
        }
        
        const sentenceLength = words.length;
        
        for (let l = 1; l <= sentenceLength; l++) {
            for (let startIndex = 0; startIndex <= sentenceLength - l; startIndex++) {
                const gram = words.slice(startIndex, startIndex + l).join(' ');
                if (!gram.trim()) continue;
                    mapGram[gram] = (mapGram[gram] || 0) + 1;
            }
        }
        
        return mapGram;
    }
    
    static getNgramWordSimilarity(a, b) {
        try {
            if (a == null || a.trim() === '' || b == null || b.trim() === '') {
            return 0;
            }
        
            const mapGramOfStringA = this.getMapGramWord(Unicode.unicodeToAscii(a).toLowerCase());
            const mapGramOfStringB = this.getMapGramWord(Unicode.unicodeToAscii(b).toLowerCase());
        
            let score = 0;
            for (const [gram, countInA] of Object.entries(mapGramOfStringA)) {
                if (mapGramOfStringB[gram]) {
                    const countInB = mapGramOfStringB[gram];
                    const gramLength = gram.length;
                    const addScore = countInA * countInB * gramLength * gramLength / 100;
                    score += addScore;
                }
            }
        
            return score;
        } catch (e) {
            console.error(e);
            return 0;
        }
    }

    static ngramWordSimilarityList(search, listText) {
        try {
            const mapScore = {};
            if (!listText || listText.length === 0) {
                return mapScore;
            }
            for (const text of listText) {
                mapScore[text] = 0;
            }
        
            const mapGramOfStringA = this.getMapGramWord(Unicode.unicodeToAscii(search).toLowerCase());
        
            for (const text of listText) {
                const mapGramOfStringB = this.getMapGramWord(Unicode.unicodeToAscii(text).toLowerCase());
                let score = 0;
                for (const [gram, countInA] of Object.entries(mapGramOfStringA)) {
                    if (mapGramOfStringB[gram]) {
                        const countInB = mapGramOfStringB[gram];
                        const gramLength = gram.length;
                        const addScore = countInA * countInB * gramLength * gramLength / 100;
                        score += addScore;
                    }
                }
                mapScore[text] = score;
            }
            return mapScore;
        } catch (e) {
            console.error(e);
            return {};
        }
    }
    

    /**
     * Calculate the harmonic mean of the lengths of two strings
     * @param {*} a 
     * @param {*} b 
     * @returns double - the harmonic mean of the lengths of the two strings
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static harmonicMeanLength(a, b){
        if (a.length <= 0 || b.length <= 0) return 1;
        return 2 / (1/a.length + 1/b.length);
    }
    /**
     * Calculates the harmonic mean of two numbers
     * @param {*} a 
     * @param {*} b 
     * @returns double - the harmonic mean of the two numbers
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static harmonicMean(a, b){
        if (a <= 0 || b <= 0) return 1;
        return 2 / (1/a + 1/b);
    }

    /**
     * Calculate the similarity between two strings using the jaccard similarity algorithm
     * @param {*} a 
     * @param {*} b 
     * @returns double in [0, 1]
     * @Created PhucTV (17/5/24)
     * @Modified None
     */
    static similiar(a, b) {
        if (a.length <= 0 || b.length <= 0) return 0;
        a = Unicode.unicodeToAscii(a).toLowerCase(); 
        b = Unicode.unicodeToAscii(b).toLowerCase();
        // let jacScore = this.jaccardSimilarity(a, b);
        // let levScore = this.calculateLevenshteinSimilarity(a, b);
        // let lcsScore = this.calculateLCSimilarity(a, b);
        // let lcscScore = this.calculateLCSCimilarity(a, b);

        // let jacWeight = 1;
        // let lcsWeight = 3;
        // let levWeight = 3;
        // let lcscWeight = 10;

        // let res = Criteria.aggregateUsingAverage(jacScore*jacWeight, lcsScore*lcsWeight, levScore*levWeight, lcscScore*lcscWeight);
        // return res;

        return this.ngramCharacterSimilarity(a, b);
    }

    static similiarityList(search, listText){
        const characterLevels = this.ngramCharacterSimilarityList(search, listText);
        const wordLevels = this.ngramWordSimilarityList(search, listText);

        return [...new Set(listText)].reduce((result, text) => {
            const characterScore = characterLevels[text] || 0;
            const wordScore = wordLevels[text] || 0;
            result[text] = characterScore + wordScore;
            return result;
        }, {});
        // return this.ngramCharacterSimilarityList(search, listText);
    }
}


export default StringAlgorithm;
