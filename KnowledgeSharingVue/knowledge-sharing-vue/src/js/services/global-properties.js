
import axios from "axios";
import { language } from "../resources/language";
import { myEnum } from "../resources/enum";
import appConfig from "@/app-config";
const api = axios.create({
    baseURL: appConfig.getBackendUrl(),
});


class GlobalProperties{
    constructor(){
        this.api = api;
        this.enum = myEnum;
        this.lang = language['vi'];

        this.buildGlobalData();
        this.buildGlobalMethod();
    }

    /**
     * Tạo ra các dữ liệu toàn cục cho ứng dụng
     *
     * @memberof GlobalProperties
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    buildGlobalData(){
        let that = this;
        that.globalData = {
            api: that.api,
            enum: that.enum,
            lang: that.lang,
        }
    }

    /**
     * Tạo ra các phương thức toàn cục cho ứng dụng
     *
     * @memberof GlobalProperties
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    buildGlobalMethod(){
        let that = this;
        that.globalMethod = {
            changeLanguage: that.changeLanguage.bind(that)
        }
    }

    /**
     * Thay đổi ngôn ngữ hiển thị trên trang web
     * @param {*} lang - ngôn ngữ cần thay đổi (VIETNAMESE, ENGLISH) 
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    changeLanguage(lang){
        try {
            if (lang === this.enum.language.VIETNAMESE){
                this.lang = language['vi'];
            } else if (lang === this.enum.language.ENGLISH){
                this.lang = language['en'];
            }
            this.globalData.lang = this.lang;
        } catch (error){
            console.error(error);
        }
    }
}

export default new GlobalProperties();
