
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
}

export default Common;
