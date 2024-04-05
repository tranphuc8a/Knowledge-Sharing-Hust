/* eslint-disable */
import { AxiosError } from "axios";
import { Validator } from "../utils/validator";


class Api{ 
    constructor(url){
        try {
            this.url = url;
            this.method = "get";
            this.headers = {
                'Content-Type': 'application/json'
            }
            this.body = null;
            this.config = {
                method: this.method,
                headers: this.headers,
                mode: 'cors'
            };
        } catch (error){
            throw error;
        }
    }

    /**
    * Các setter dữ liệu cho api
    * @param {*} config params
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    setBody(bodyObj){
        this.body = this.config.body = bodyObj;
        return this;
    }
    setHeader(headerObj){
        this.headers = this.config.headers = headerObj;
        return this;
    }
    setConfig(configObj){
        this.config = configObj;
        return this;
    }

    /**
    * Thực thi api lấy kết quả
    * @param {*} success - hàm callback xử lý api khi thành công
    * @param {*} error - hàm callback xử lý api khi thất bại
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    execute(success, error){
        let that = this;
        fetch(this.url, this.config)
        .then((data) => {
            data = data.json();
            that.data = data;
            return data;
        })
        .then(success)
        .catch(error);
    }

    /**
    * Thực thi api lấy kết quả
    * @param {*} toastManager - đối tượng xử lý hiển thị toast 
    * @param {*} error - đối tượng lỗi trả về từ api
    * @Author TVPhuc (18/12/23)
    * @Edit None
    **/
    static resolveAxiosError(toastManager, error, defaultMsg = 'Đã có lỗi nào đó xảy ra'){
        try {
            if (! (error instanceof AxiosError)){
                throw error;
            }
            let response = error.response;
            if (response === null || response === undefined || 
                response.data === null || response.data === undefined){
                return toastManager.error(error.message);
            }
            
            let msg = response.data.UserMessage;
            if (Validator.isEmpty(msg)){
                msg = defaultMsg;
            }
            switch (response.status){
                case 400:
                    toastManager.error(msg);
                    break;
                case 401:
                    toastManager.error(msg);
                    break;
                case 404:
                    toastManager.error(msg);
                    break;
                case 500:
                    toastManager.error(msg);
                    break;
                default:
                    toastManager.error(mgs);
                    break;
            }
        }
        catch (error){
            console.error(error);
        }
    }

    /**
     * Đọc body từ response trả về từ api khi thành công
     * @param {*} response - response trả về từ api
     * @return body - body đọc được từ response
     * @Created PhucTV (31/1/24)
     * @Modified None
     */
    static tryGetBody(response){
        if (Validator.isEmpty(response) ||
            Validator.isEmpty(response.data)){
            return null;
        }
        return response.data.Body;
    }

    /**
     * Đọc UserMessage từ axios error
     * @param {*} error - ngoại lệ ném ra khi lỗi
     * @return userMessage - message đọc được từ error
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    static tryGetUserMessage(error){
        if (error && error.response && error.response.data && error.response.data.UserMessage){
            return error.response.data.UserMessage;
        }
        if (error && error.response && error.response.UserMessage){
            return error.response.UserMessage;
        }
        if (error && error.UserMessage){
            return error.UserMessage;
        }
        return null;
    }
}


class GetApi extends Api{
    constructor(url){
        super(url);
    }
}


class PostApi extends Api{
    constructor(url){
        super(url);
        this.method = this.config.method = "post";
    }

    /**
    * Thực thi post api lấy kết quả
    * @param {*} success - hàm callback xử lý api khi thành công
    * @param {*} error - hàm callback xử lý api khi thất bại
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    execute(success, error){
        try {
            if (this.config.headers['Content-Type'] === 'application/json'){
                this.config.body = JSON.stringify(this.body);
            }
            super.execute(success, error);
        } catch (exception){
            throw exception;
        }
    }
}

class PutApi extends Api{
    /**
    * Tạo ra một PutApi gắn với liên kết
    * @param {*} url - liên kết của api
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    constructor(url){
        try {
            super(url);
            this.method = this.config.method = "put";
        } catch (error){
            throw error;
        }
    }
}

class DeleteApi extends Api{
    constructor(url){
        try {
            super(url);
            this.method = this.config.method = "delete";
        } catch (error){
            throw error;
        }
    }
}


export { Api, GetApi, PostApi, PutApi, DeleteApi };
