
import statusCodeEnum from "../resources/status-code-enum";
// import appConfig from "@/app-config";
import { myEnum } from "../resources/enum";
import { language } from "../resources/language";
// const loginPageUrl = `${appConfig.getBackendUrl()}/login`;
var lang = language.vi;

class ResolveAxiosResponse {
    toastManager = null;
    popupManager = null;

    /**
     * Đặt ngôn ngữ cho đối tượng handle lỗi
     * @param {*} lang - ngôn ngữ cần đặt (VIETNAMESE, ENGLISH)
     * @returns none
     * @Created PhucTV (05/03/24)
     * @Modified None
     */
    static setLanguage(lang){
        try {
            if (lang === myEnum.language.ENGLISH){
                lang = language.en;
                return;
            }
            // Mặc định, đặt là Tiếng Việt:
            lang = lang.vi;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Thực hiện cập nhật toast và popup manager cho đối tượng
     * @param {*} toast 
     * @param {*} popup 
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    updateManager(toast, popup){
        this.toastManager = toast;
        this.popupManager = popup;
    }

    /**
     * Hàm xử lý lỗi tương ứng với mã BAD_REQUEST
     * @param {*} error - Lỗi cần handle
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveBadRequest = async (error) => {
        let defaultMsg = "Yêu cầu không hợp lệ. Vui lòng kiểm tra lại thông tin";
        let msg = await this.tryGetUserMessage(error);
        let message = msg ?? defaultMsg;
        if (this.toastManager != null){
            this.toastManager.error(message);
        } else {
            console.error(error);
        }
    }
    
    /**
     * Hàm xử lý lỗi tương ứng với mã UNAUTHORIZED
     * @param {*} error - Lỗi cần handle
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveUnAuthorized = async (error) => {
        let defaultMsg = "Bạn cần đăng nhập để thực hiện tác vụ này";
        let msg = await this.tryGetUserMessage(error);
        let message = msg ?? defaultMsg;
        if (this.popupManager != null){
            this.popupManager.requiredLogin();
        } else {
            if (this.toastManager != null){
                this.toastManager.error(message);
            } else {
                console.error(error);
            }
        }
    }

    /**
     * Hàm xử lý lỗi tương ứng với mã FORBIDDEN
     * @param {*} error - Lỗi cần handle
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveForbidden = async (error) => {
        let defaultMsg = "Bạn không có quyền thực hiện tác vụ này. Vui lòng liên hệ Admin để được hỗ trợ";
        let msg = await this.tryGetUserMessage(error);
        let message = msg ?? defaultMsg;
        if (this.toastManager != null){
            this.toastManager.error(message);
        } else {
            console.error(error);
        }
    }

    /**
     * Hàm xử lý lỗi tương ứng với mã SERVER_ERROR
     * @param {*} error - Lỗi cần handle
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveServerError = async (error) => {
        let defaultMsg = "Có lỗi hệ thống xảy ra. Vui lòng thử lại sau";
        let msg = await this.tryGetUserMessage(error);
        let message = msg ?? defaultMsg;
        if (this.toastManager != null){
            this.toastManager.infor(message);
        } else {
            console.error(error);
        }   
    }


    /**
     * Hàm xử lý lỗi tương ứng với mã CONNECT_ERROR
     * @param {*} null
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveConnectError = async () => {
        let msg = lang.validator.messages.connectError;
        if (this.toastManager != null){
            this.toastManager.infor(msg);
        } else {
            console.error(msg);
        }   
    }


    defaultResolveError = {
        [statusCodeEnum.CONNECT_ERROR]: this.resolveConnectError,
        [statusCodeEnum.BAD_REQUEST]:   this.resolveBadRequest,
        [statusCodeEnum.UNAUTHORIZED]:  this.resolveUnAuthorized,
        [statusCodeEnum.FORBIDDEN]:     this.resolveForbidden,
        [statusCodeEnum.SERVER_ERROR]:  this.resolveServerError,
        [statusCodeEnum.NOT_FOUND]:     this.resolveConnectError
    };


    /****************************************************************** */

    /**
     * Đọc statusCode từ response trả về từ api
     * @param {*} response - response trả về từ api
     * @return statusCode
     * @Created PhucTV (22/3/24)
     * @Modified None
     */
    tryGetStatusCode = (response) => {
        try {
            return response?.status ?? response?.data?.StatusCode ?? response?.response?.data?.StatusCode;
        } catch (error){
            console.error(error);
            return null;
        }
    }

    /**
     * Đọc body từ response trả về từ api khi thành công
     * @param {*} response - response trả về từ api
     * @return body - body đọc được từ response
     * @Created PhucTV (31/1/24)
     * @Modified None
     */
    tryGetBody = (response) => {
        try {
            return response?.Body ?? response?.data?.Body ?? response?.response?.data?.Body;
        } catch (error){
            console.error(error);
            return null;
        }
    }

    /**
     * Đọc UserMessage từ axios error
     * @param {*} error - ngoại lệ ném ra khi lỗi
     * @return userMessage - message đọc được từ error
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    tryGetUserMessage = (error) => {
        try {
            return error?.UserMessage ?? error?.data?.UserMessage ?? error?.response?.data?.UserMessage;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Đọc ra key của Body trả về từ api
     * @param {*} response - đối tượng trả về chứa Body
     * @param {*} key - trường cần đọc
     * @returns - giá trị đọc được hoặc null nếu không đọc được
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    tryGetKey = (response, key) => {
        try {
            if (response){
                if (response[key]){
                    return response[key];
                }
                if (response.data){
                    if (response.data[key]){
                        return response.data[key];
                    }
                }
                if (response.response?.data){
                    if (response.response.data[key]){
                        return response.response.data[key];
                    }
                }
            }
            return null;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Hàm xử lý lỗi chung axios
     * @param {*} error - lỗi nhận được
     * @param {*} errorCallbacks - hàm xử lý custom của user
     * @returns none
     * @Created PhucTV (23/2/24)
     * @Modified None
     */
    resolveAxiosError = async (error, errorCallbacks) => {
        try {
            if (!errorCallbacks) {
                errorCallbacks = this.defaultResolveError;
            }

            if (error.response) {
                // The request was made and the server responded with a status code
                const { status, data } = error.response;
                let callback = errorCallbacks[status] ?? this.defaultResolveError[status]?.bind(this);
                if (callback) {
                    await callback(data);
                    return;
                }
            } else if (error.request) {
                // Not Connected to server
                let status = statusCodeEnum.CONNECT_ERROR;
                let callback = errorCallbacks[status] ?? this.defaultResolveError[status]?.bind(this);
                if (callback) {
                    await callback();
                    return;
                }
            } else {
                // Logic Exception
                console.error(error);
                return;
            }
            // console.log(error.config);
        } catch (error){
            console.error(error);
        }
    }
}

export default new ResolveAxiosResponse();