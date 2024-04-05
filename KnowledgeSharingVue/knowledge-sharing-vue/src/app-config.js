

import { myEnum } from "@/js/resources/enum";
// const defaultBackendUrl = 'http://tranphuc8a.somee.com/api/v1';
const defaultBackendUrl = 'http://localhost:5000/api/v1';
const defaultHomepageUrl = 'http://localhost:8080';

/**
 * Xóa những dấu / ở cuối liên kết
 * @param {*} url - liên kết cần xóa
 * @returns liên kết sau khi được xóa splash
 * @Created PhucTV (22/2/24)
 * @Modified None
 */
function removeTrailingSlash(url) {
    return url?.replace(/\/+$/, '');
}


export default {
    getHomePageUrl: function(){
        return process.env.VUE_APP_HOMEPAGE_URL?? defaultHomepageUrl;
    },
    getBackendUrl: function(){
        return removeTrailingSlash(process.env.VUE_APP_BACKEND_URL) ?? defaultBackendUrl;
    },
    getLanguage: function(){
        return process.env.VUE_APP_LANGUAGE ?? myEnum.language.VIETNAMESE;
    },
};
