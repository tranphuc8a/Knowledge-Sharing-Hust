/* eslint-disable */

import { Validator } from "./validator";

class Common {  
    /**
     * Xóa những dấu / ở cuối liên kết
     * @param {*} url - liên kết cần xóa
     * @returns liên kết sau khi được xóa splash
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    static removeTrailingSlash(url) {
        return url?.replace(/\/+$/, '');
    }

    /**
     * Kiểm tra xem url có hình ảnh không
     * @param {*} url - liên kết cần kiểm tra
     * @returns true nếu có hình ảnh, false nếu không
     * @Created PhucTV (14/04/24)
     * @Modified None
     */
    static isValidImage(url) {
        if (Validator.isEmpty(url)) return false;
        return new Promise((resolve) => {
            const img = new Image();
            img.onload = () => resolve(true);
            img.onerror = () => resolve(false);
            img.src = url; // Thử tải hình ảnh
        });
    }

    /**
     * Lay kích thước hình ảnh từ url
     * @param {*} url 
     * @returns kích thước hình ảnh
     * @Created PhucTV (14/04/24)
     * @Modified None
     */
    static getImageSize(url) {
        return new Promise((resolve) => {
            const img = new Image();
            img.onload = () => resolve({ width: img.width, height: img.height });
            img.onerror = () => resolve(null);
            img.src = url;
        });
    }

    /**
     * Format the number to beautiful format
     * 
     * @param {*} num the number need to be formatted
     * @returns {string} the formatted number
     * @Created PhucTV (15/04/24)
     * @Modified None
     */
    static formatNumber(num) {
        if (Validator.isEmpty(num)) return 0;

        if (num < 1000) {
            return num.toString();
            // or simply return num; for number format
        }
        
        // Defines units
        const units = ["K", "M", "B", "T", "KT", "MT", "BT", "TT",
            "KTT", "MTT", "BTT", "TTT"
        ];
        
        // Divide log by 3 because 10^3 = 1000
        const order = Math.floor(Math.log10(num) / 3);
        
        // Get unitName
        const unitName = units[order - 1];
        
        // Scale number
        const scale = Math.pow(10, order * 3);
        const scaled = num / scale;
        
        // keep up to three significant digits
        let formatted = scaled.toFixed(2 - Math.floor(Math.log10(scaled)));
        
        // Remove trailing zeroes
        formatted = parseFloat(formatted).toString();
        
        return formatted + (unitName ?? "H");
    }


    /**
     * Xóa những dấu / 
     * @param {*} inputStr - chuoi markdown cần xóa
     * @returns chuoi sau khi xoa
     * @Created PhucTV (26/04/24)
     * @Modified None
     */
    static unescapeSpecialCharacters(inputStr) {
        try {
            if (inputStr == null) return null;
            const replacements = {
                '\\n': '\n',
                '\\r': '\r',
                '\\t': '\t',
                '\\b': '\b',
                '\\f': '\f',
                '\'': '\'',
                '\"': '\"',
                '\\\\': '\\'
            };
    
            return inputStr.replace(/\\[nrtbf'"\\]/g, matched => replacements[matched]);
        }
        catch (error) {
            console.error(error);
            return null;
        }   
    }

    static normalizeMarkdownText(markdownText) {
        try {
            if (markdownText == null) return null;
            
            // Xóa khoảng trắng đầu và cuối chuỗi
            let normalizedText = markdownText.trim();
            
            // Xóa khoảng trắng giữa \n và #
            normalizedText = normalizedText.replace(/\n\s+(?=#)/g, '\n');
            
            return normalizedText;
        } catch (error) {
            console.error(error);
            return null;
        }
    }
}

export default Common;
