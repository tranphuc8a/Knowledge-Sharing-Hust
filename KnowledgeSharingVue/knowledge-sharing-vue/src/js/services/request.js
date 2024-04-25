

import axios from "axios";
import { Validator } from "../utils/validator";
import { myEnum } from "../resources/enum";
import appConfig from "@/app-config";
import resolveAxiosResponse from "./resolve-axios-response";
import CurrentUser from "../models/entities/current-user";

const statusCodeEnum    = myEnum.statusCode;
const methodEnum        = myEnum.requestMethod;
const ContentTypeEnum   = myEnum.contentType;
// const timeOut           = 100000; // 10 seconds

/**
 * Instance axios cho các Request thông thường
 * @Created PhucTV (23/2/24)
 * @Modifed None
 */
var globalInstance = axios.create({
    baseURL: appConfig.getBackendUrl(),
    // timeout: timeOut,
    headers: {
        'Content-Type': ContentTypeEnum.JSON
    }
});

/**
 * Instance axios cho các Request gửi formdata
 * @Created PhucTV (23/2/24)
 * @Modified None
 */
var formdataInstance = axios.create({
    baseURL: appConfig.getBackendUrl(),
    // timeout: timeOut,
    headers: {
        'Content-Type': ContentTypeEnum.FORM_DATA
    }
});


class Request {
    constructor(){
        try {
            this.isCallOnce = false;
            this.instance = globalInstance;
            this.config = {
                method: methodEnum.GET,
                url: 'Employees', // default: get list Employees
                data: null
            };
            this.token = localStorage.getItem('access-token');
            this.refreshToken = localStorage.getItem('refresh-token');
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Đặt contentType cho request
     * @param {*} contentType 
     * @returns this
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    setContentType(contentType) {
        if (Object.values(ContentTypeEnum).includes(contentType)) {
            this.instance.defaults.headers['Content-Type'] = contentType;
        } else {
            throw new Error('Invalid Content-Type');
        }
        return this;
    }

    /**
     * Đặt access token cho request
     * @param {*} token 
     * @returns this
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    setToken(token){
        this.token = token;
        this.instance.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        return this;
    }

    /**
     * Đặt tham số cho phép call một lần
     * @param {*} isCallOnce - true: call 1 lần, false: call 2 lần thử access token
     * @returns this
     * @Created PhucTV (3/3/24)
     * @Modified None
     */
    setIsCallOnce(isCallOnce = false){
        this.isCallOnce = isCallOnce;
        return this;
    }

    /**
     * Đặt url cho request
     * @param {*} url 
     * @returns this
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    setUrl(url){
        this.config.url = url;
        return this;
    }

    /**
     * Đặt body cho request
     * @param {*} data 
     * @returns this
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    setBody(data) {
        this.config.data = data;
        return this;
    }

    /**
     * Đặt params cho request
     * @param {*} params 
     * @returns this
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    setParams(params){
        this.config.params = params;
        return this;
    }

    /**
     * Thử làm mới token khi token đã hết hạn
     * @param none
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    async tryRefreshToken(){
        let that = this;
        let response = await globalInstance.post('Authentications/refresh-token', {
            AccessToken: that.token,
            RefreshToken: that.refreshToken
        });
        // success:
        let body = await Request.tryGetBody(response);
        await Request.setTokenToLocalStorage(body.Token, body.RefreshToken);
        this.token = body.Token;
        this.refreshToken = body.RefreshToken;
        return this;
    }

    /**
     * Thực hiện ghi token và refreshToken vào local storage
     * @param {*} token - token cần ghi
     * @param {*} refreshToken - refresh token cần ghi
     * @Created PhucTV (5/3/24)
     * @Modified None
     */
    static async setTokenToLocalStorage(token, refreshToken){
        if (Validator.isEmpty(token)){
            throw new Error("Token is empty");
        }
        if (Validator.isEmpty(refreshToken)){
            throw new Error("RefreshToken is empty");
        }
        localStorage.setItem("access-token", token);
        localStorage.setItem("refresh-token", refreshToken);
    }

    /**
     * Xóa dữ liệu đăng nhập trong token storage
     * @param none
     * @returns none
     * @Created PhucTV (21/3/24)
     * @Modified None
     */
    static async deleteLocalStorage(){
        localStorage.removeItem("access-token");
        localStorage.removeItem("refresh-token");
    }

    /**
     * Thực thi api
     * @params none
     * @returns response - nội dung api trả về
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    async execute(){
        try {
            this.setToken(this.token);
            let response = await this.instance(this.config);
            // success:
            // return Request.tryGetBody(response);
            Request.isTokenRefresh = true;
            return response;
        } catch (error1) {
            if (this.isCallOnce || !Request.isTokenRefresh) throw error1;
            if (error1.response && error1.response.status === statusCodeEnum.UNAUTHORIZED){
                try {
                    await this.tryRefreshToken();
                } catch (error2){
                    Request.isTokenRefresh = false;
                    // console.error(error2);
                    throw error1;
                }

                Request.isTokenRefresh = true;
                this.setToken(this.token);
                let response = await this.instance(this.config);
                // success;
                // return Request.tryGetBody(response);
                return response;
            } else {
                throw error1;
            }
        }
    }

    /**
     * Kiểm tra đã đăng nhập hay chưa thông qua token
     * @param none
     * @returns none
     * @returns Thông tin user đã đăng nhập
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    async checkLogedIn(){
        try {
            let response = await new GetRequest('Users/me').execute();
            let body = this.tryGetBody(response);
            let user = new CurrentUser();
            user.copy(body);
            CurrentUser.setInstance(user);
            return true;
        } catch (error){
            console.error(error);
            return false;
        }
    }


    static isTokenRefresh       = true;
    static tryGetBody           = resolveAxiosResponse.tryGetBody.bind(resolveAxiosResponse);
    static tryGetStatusCode     = resolveAxiosResponse.tryGetStatusCode.bind(resolveAxiosResponse);
    static tryGetUserMessage    = resolveAxiosResponse.tryGetUserMessage.bind(resolveAxiosResponse);
    static tryGetKey            = resolveAxiosResponse.tryGetKey.bind(resolveAxiosResponse);
    static resolveAxiosError    = resolveAxiosResponse.resolveAxiosError.bind(resolveAxiosResponse);
    tryGetBody                  = Request.tryGetBody;
    tryGetStatusCode            = Request.tryGetStatusCode;
    tryGetUserMessage           = Request.tryGetUserMessage;
    tryGetKey                   = Request.tryGetKey;
    resolveAxiosError           = Request.resolveAxiosError;
}


class GetRequest extends Request {
    constructor(url) {
        super();
        this.config = {
            method: methodEnum.GET,
            url: url
        };
    }
}

class PostRequest extends Request {
    constructor(url) {
        super();
        this.config = {
            method: methodEnum.POST,
            url: url,
            data: null
        };
    }

    /**
     * Cấu hình để gửi request với formdata
     * @param none
     * @return none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    prepareFormData(){
        this.formData = new FormData();
        this.config.data = this.formData;
        this.instance = formdataInstance;
        return this;
    }

    /**
     * Thêm một cặp key-val vào formdata
     * @param {*} key 
     * @param {*} value 
     * @return {*} none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    addFormData(key, value){
        if (this.formData == null){
            this.formData = new FormData();
        }
        this.formData.append(key, value);
        return this;
    }
}

class DeleteRequest extends Request {
    constructor(url) {
        super();
        this.config = {
            method: methodEnum.DELETE,
            url: url
        };
    }
}

class PutRequest extends Request {
    constructor(url) {
        super();
        this.config = {
            method: methodEnum.PUT,
            url: url
        };
    }
}

class PatchRequest extends Request {
    constructor(url) {
        super();
        this.config = {
            method: methodEnum.PATCH,
            url: url
        };
    }
}

export { Request, GetRequest, PostRequest, DeleteRequest, PutRequest, PatchRequest }

