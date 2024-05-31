/* eslint-disable */

import { Validator } from "./validator";

class Common {  
    /**
     * Load recaptcha script from google api to the head of the document
     * @Created PhucTV (22/2/24)
     * @Modified None
     * @param void
     * @returns void
     * @example Common.loadRecaptchaScript();
     */
    static loadRecaptchaScript() {
        let script = document.createElement('script');
        script.src = 'https://www.google.com/recaptcha/api.js';
        script.async = true;
        script.defer = true;
        document.head.appendChild(script);
    }

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
        // Kiểm tra URL có trống không
        if (Validator.isEmpty(url)) return false;

        // Kiểm tra URL có bắt đầu với 'https://' không
        const isHttps = url.startsWith('https://');
        if (!isHttps) return false;

        // Kiểm tra URL có phải là hình ảnh hợp lệ không
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

    /**
     * Chuẩn hóa chuỗi markdown
     * @param {*} markdownText - nội dung markdown cần chuẩn hóa
     * @returns văn bản markdown sau khi chuẩn hóa
     * @Created PhucTV (26/04/24)
     * @Modified None
     */
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

    /**
     * Biểu diên số tiền theo định dạng tiền tệ Việt Nam
     * @param {*} number - số tiền cần biểu diễn
     * @returns chuỗi biểu diễn số tiền theo định dạng tiền tệ Việt Nam
     * @Created PhucTV (15/5/24)
     * @Modified None
     */ 
    static visualizeCurrency(number) {
        // Kiểm tra xem giá trị đầu vào có phải là kiểu số không, nếu không cố gắng ép kiểu thành số
        if (typeof number !== 'number') {
            number = Number(number);
        }

        // Kiểm tra nếu giá trị không phải là số hoặc là NaN (bởi vì Number('abc') cho NaN)
        if (isNaN(number)) {
            return null;
        }

        // Làm tròn số nếu nó không phải là số nguyên và lấy giá trị tuyệt đối
        number = Math.round(Math.abs(number));

        // Chuyển số thành chuỗi và định dạng theo tiền tệ Việt Nam
        return number.toFixed().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' VNĐ';
    }
}

export default Common;

