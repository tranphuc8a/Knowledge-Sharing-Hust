
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
}

export default Common;
