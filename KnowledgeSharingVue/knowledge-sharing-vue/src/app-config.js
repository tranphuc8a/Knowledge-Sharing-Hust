

import { myEnum } from "@/js/resources/enum";
import Common from "./js/utils/common";

const defaultBackendUrl = 'http://localhost:5000/api/v1';
const defaultSocketUrl = 'ws://localhost:5000/api/v1';

// const defaultBackendUrl = 'https://tranphuc8a.somee.com/api/v1';
// const defaultSocketUrl = 'wss://tranphuc8a.somee.com/api/v1';

// const defaultBackendUrl = 'https://localhost:7277/api/v1';
// const defaultSocketUrl = 'wss://localhost:7277/api/v1';
const defaultHomepageUrl = 'http://localhost:8080';
const defaultCapchaSiteKey = '6Leu3-wpAAAAAK-gFpFCjbSOTBXl_q1nCqLe6XDR';


export default {
    getHomePageUrl: function(){
        return process.env.VUE_APP_HOMEPAGE_URL ?? defaultHomepageUrl;
    },
    getBackendUrl: function(){
        return Common.removeTrailingSlash(process.env.VUE_APP_BACKEND_URL) ?? defaultBackendUrl;
    },
    getLanguage: function(){
        return process.env.VUE_APP_LANGUAGE ?? myEnum.language.VIETNAMESE;
    },
    getCaptchaSiteKey: function(){
        return process.env.VUE_APP_CAPTCHA_SITE_KEY ?? defaultCapchaSiteKey;
    },
    getSocketUrl: function(){
        return Common.removeTrailingSlash(process.env.VUE_APP_SOCKET_URL) ?? defaultSocketUrl;
    }
};
