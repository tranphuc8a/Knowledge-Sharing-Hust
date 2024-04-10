

import { myEnum } from "@/js/resources/enum";
import Common from "./js/utils/common";

//const defaultBackendUrl = 'http://tranphuc8a.somee.com/api/v1';
const defaultBackendUrl = 'http://localhost:5133/api/v1';
const defaultHomepageUrl = 'http://localhost:8080';


export default {
    getHomePageUrl: function(){
        return process.env.VUE_APP_HOMEPAGE_URL?? defaultHomepageUrl;
    },
    getBackendUrl: function(){
        return Common.removeTrailingSlash(process.env.VUE_APP_BACKEND_URL) ?? defaultBackendUrl;
    },
    getLanguage: function(){
        return process.env.VUE_APP_LANGUAGE ?? myEnum.language.VIETNAMESE;
    },
};
