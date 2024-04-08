

<template>
    <div class="pl-background">
        <div class="pl-form-container">
            <div class="pl-form">
                <div class="pl-logo-frame">
                    <div class="pl-logo"></div>
                </div>
                <form class="pl-input" v-on:keypress.enter="resolveEnterForm">
                    <!-- 2 thẻ input -->
                    <AmisTextField :autocomplete="'username'" type="text" :placeholder=" getLabel()?.username " :isShowTitle="false" 
                        ref="username" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.username"
                        :errorMessage="getLabel()?.invalidUsername"
                        />
                    <AmisPasswordTextField :autocomplete="'current-password'" :placeholder="getLabel()?.password"
                        ref="password" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.password"
                        :errorMessage="getLabel()?.invalidPassword"
                        />
                    <div class="pl-input-captcha" v-show="isLoginWithCaptcha">
                        <div class="pl-input-captcha-textfield">
                            <AmisTextField type="text" :placeholder=" getLabel()?.captcha " :isShowTitle="false" 
                                ref="captcha" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.captcha"
                                :errorMessage="getLabel()?.invalidCaptcha">
                                <div class="p-refresh-captcha-icon" @:click="refreshCaptcha">
                                    <img src="@/assets/login/icon-reload.svg"/>
                                </div>
                            </AmisTextField>
                        </div>
                        <div class="pl-captcha-image-frame">
                            <img v-show="captchaImageData != null" class="pl-captcha-image" :src="`data:image/png;base64,${captchaImageData}`" alt="Captcha">
                        </div>
                    </div>
                </form>
                <div class="pl-error-message" v-show="isShowError">
                    {{ errorMessage }}
                </div>
                <div class="pl-forgotpass-link pa-link">
                    <router-link to="/forgotpassword" >
                        {{ getLabel()?.forgotpassword }}
                    </router-link>

                    <router-link to="/register" >
                        {{ getLabel()?.register }}
                    </router-link>
                </div>
                <div class="pl-button">
                    <!-- Thẻ button login -->
                    <AmisSubmitButton :label=" getLabel()?.login" :onclick="resolveSubmitLogin" ref="button" />
                </div>
                <div class="pl-divide">
                    <div class="pl-segment-frame">
                        <div class="pl-segment"></div>
                    </div>
                    <div class="pl-text-divide">
                        {{ getLabel()?.others }}
                    </div>
                </div>
                <div class="pl-login-options">
                    <div class="pl-option pl-login-google">
                    </div>
                    <div class="pl-option pl-login-apple">
                    </div>
                    <div class="pl-option pl-login-microsoft">
                    </div>
                </div>
            </div>
            <div class="pl-copyright">
                Copyright © 2012 - 2024 KS JSC
            </div>
        </div>
        <div class="pl-change-language-button">
            <ChangeLanguageButton />
        </div>
    </div>
</template>


<script>
import ChangeLanguageButton from '@/components/base/authentication/MChangeLanguageButton.vue';
import AmisTextField from '@/components/base/authentication/MSlotedTextField.vue';
import AmisPasswordTextField from '@/components/base/authentication/MPasswordTextfield.vue';
import AmisSubmitButton from '@/components/base/authentication/MSubmitButton.vue';
import { UsernameValidator, PasswordValidator, Validator, NotEmptyValidator } from '@/js/utils/validator';
import { Request, PostRequest, GetRequest } from '@/js/services/request';
import statusCodeEnum from '@/js/resources/status-code-enum';
import appConfig from '@/app-config';

export default {
    name: 'KSLoginPage',
    data(){
        return {
            label: null,
            isLoginWithCaptcha: false,
            captchaImageData: null,
            captchaToken: null,
            global: this.globalData,
            isShowError: false,
            errorMessage: 'Tên đăng nhập hoặc mật khẩu không đúng',
            input: {},
            account: {
                username: null,
                password: null,
                captcha: null
            },
            validator: {
                username: new UsernameValidator(this.getLabel()?.invalidUsername),
                password: new PasswordValidator(this.getLabel()?.invalidPassword),
                captcha: new NotEmptyValidator(this.getLabel()?.invalidCaptcha)
            },
            button: null
        }
    },
    async mounted(){
        let userLogin = await new GetRequest().checkLogedIn();
        if (userLogin != null){
            let lastUrl = localStorage.getItem("redirect-to");
            if (Validator.isEmpty(lastUrl)){
                lastUrl = '/'; // to home page
            }
            this.$router.push(lastUrl);
        }
        this.getLabel();
        this.input = {
            username: this.$refs.username,
            password: this.$refs.password,
            captcha: this.$refs.captcha
        };
        this.button = this.$refs.button;
        if (this.isLoginWithCaptcha){
            this.refreshCaptcha();
        }
    },
    components: {
        ChangeLanguageButton, AmisTextField, AmisPasswordTextField,
        AmisSubmitButton
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.inject.language != null){
                this.label = this.inject.language.pages.login;
            }
            return this.label;
        },

        /**
         * Hai hàm thực hiện ẩn hiện lỗi đăng nhập từ api
         * @param {*} msg - lỗi hiển thị
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None 
        */
        async showErrorMsg(msg){
            try {
                if (Validator.isEmpty(msg)) return;
                this.isShowError = true;
                this.errorMessage = msg;
            } catch (error){
                console.error(error);
            }
        },
        async hideErrorMsg(){
            this.isShowError = false;
        },
        /**
         * Hàm lấy dữ liệu từ form username, password đổ vào biến account
         * @param none
         * @returns - account - tài khoản lấy được
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        async getAccount(){
            try {
                this.account.username = await this.input.username.getValue();
                this.account.password = await this.input.password.getValue();
                this.account.captcha = await this.input.captcha.getValue();
                return this.account;
            } catch (error){
                console.error(error);
                return null;
            }
        },

        /**
         * Xử lý sự kiện bấm enter
         * @param none
         * @returns none
         * @Created PhucTV (01/03/24)
         * @Modified None
        */
        async resolveEnterForm(){
            try {
                if (this.button?.resolveOnclick){
                    await this.button.resolveOnclick();
                } else {
                    console.log(this.button);
                }
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Thực hiện lấy mã Captcha
         * @param none
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async refreshCaptcha(){
            try {
                let result = await new GetRequest('/Authenticate/refresh-captcha').execute();
                // success:
                let body = Request.tryGetBody(result);
                this.captchaImageData = body.ImageData;
                this.captchaToken = body.Token;
            } catch (error){
                Request.resolveAxiosError(error);
            }
        },

        /**
         * Hàm xử lý khi nhấn đăng nhập
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        async resolveSubmitLogin(){
            try {
                await this.hideErrorMsg();

                let isOkay = await this.validateForm();
                if (!isOkay) {
                    return;
                }

                if (!this.isLoginWithCaptcha){
                    await this.resolveLoginNormal();
                } else {
                    await this.resolveLoginCaptcha();
                } 
            } catch (error){
                Request.resolveAxiosError(error);
            }
        },

        /**
         * Xử lý đăng nhập thông thường
         * @param none
         * @return none
         * @Created PhucTV (03/03/24)
         * @Modified None
        */
        async resolveLoginNormal(){
            let account = await this.getAccount();
            try {
                let result = await new PostRequest('/Authenticate/login').setBody({
                    Username: account.username,
                    Password: account.password
                }).setIsCallOnce(true).execute();

                await this.resolveLoginSuccess(Request.tryGetBody(result));
            } catch (error){
                await this.resolveLoginFailed(error);
            }
        },

        /**
         * Xử lý đăng nhập bằng captcha
         * @param none
         * @return none
         * @Created PhucTV (03/03/24)
         * @Modified None
        */
        async resolveLoginCaptcha(){
            let account = await this.getAccount();
            try {
                let result = await new PostRequest('/Authenticate/login-with-captcha').setBody({
                    Username: account.username,
                    Password: account.password,
                    Captcha: account.captcha,
                    Token: this.captchaToken
                }).setIsCallOnce(true).execute();

                await this.resolveLoginSuccess(Request.tryGetBody(result));
            } catch (error){
                await this.resolveLoginFailed(error);
            }
        },


        /**
         * Xử lý đăng nhập thất bại
         * @param {*} error - lỗi trả về từ api
         * @returns none
         * @Created PhucTV (21/3/24)
         * @Modified None 
        */
        async resolveLoginFailed(error){
            try {
                // Ngừng validate động
                this.input.username.stopDynamicValidate();
                this.input.password.stopDynamicValidate();

                // Gọi hàm xử lý lỗi chung từ Request
                let doNothing = async function(){};
                Request.resolveAxiosError(error, {
                    // [statusCodeEnum.SERVER_ERROR]: doNothing,
                    [statusCodeEnum.UNAUTHORIZED]: doNothing,
                    [statusCodeEnum.BAD_REQUEST]: doNothing,
                    [statusCodeEnum.FORBIDDEN]: doNothing,
                });

                // Lấy về mã lỗi
                let statusCode = Request.tryGetStatusCode(error);
                // Hiển thị message lỗi nếu không phải lỗi server
                if (statusCode != statusCodeEnum.SERVER_ERROR){
                    this.showErrorMsg(this.getLabel()?.invalidUsernameOrPassword);
                    let userMessage = Request.tryGetUserMessage(error);
                    this.showErrorMsg(userMessage);
                }                
                // Nếu lỗi là BAD_REQUEST thì yêu cầu đăng nhập lại với Captcha
                if (statusCode == statusCodeEnum.BAD_REQUEST){
                    let body = Request.tryGetBody(error);
                    if (body != null && body != undefined){
                        this.isLoginWithCaptcha = true;
                        this.captchaImageData = body.ImageData;
                        this.captchaToken = body.Token;
                    }
                }
                // Ngừng validate động cho Captcha input
                if (this.isLoginWithCaptcha){
                    this.input.captcha.stopDynamicValidate();
                    this.input.captcha.setValue("");
                }
            } catch (error2) {
                console.error(error2);
            }
        },
        
        /***
         * Xử lý đăng nhập thành công
         * @param {*} tokenModel - chứa token lấy về từ api login
         * @returns none
         * @Created PhucTV (22/2/24)
         * @Modified None 
         */
        async resolveLoginSuccess(tokenModel){
            try {
                if (tokenModel == null){
                    let msg = "tokenModel is null";
                    this.getToastManager().error(msg);
                    throw new Error(msg);
                }
                await Request.setTokenToLocalStorage(tokenModel.Token, tokenModel.RefreshToken);

                let redirectTo = localStorage.getItem("redirect-to");
                localStorage.setItem("redirect-to", "");
                if (Validator.isEmpty(redirectTo)){
                    redirectTo = appConfig.homepage;
                }
                window.location.href = redirectTo;
            } catch (error){
                console.error(error);
            }
        },


        /**
         * Hàm thực hiện validate username và password trước khi đăng nhập
         * @param none
         * @returns true - cả username và password hợp lệ, false - không hợp lệ
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        async validateForm(){
            try {
                this.input.username.startDynamicValidate();
                this.input.password.startDynamicValidate();
                if (this.isLoginWithCaptcha) {
                    this.input.captcha.startDynamicValidate();
                }

                let validateUsername = await this.input.username.validate();
                if (!validateUsername) {
                    this.input.username.focus();
                    return false;
                }
                let validatePassword = await this.input.password.validate();
                if (!validatePassword){
                    this.input.password.focus();
                    return false;
                }
                if (this.isLoginWithCaptcha){
                    let validateCaptcha = await this.input.captcha.validate();
                    if (!validateCaptcha){
                        this.input.captcha.focus();
                        return false;
                    }
                }

                return true;
            } catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        inject: {},
        getPopupManager: {}, 
        getToastManager: {}
    }
}
</script>


<style scoped>
@import url(@/css/pages/login-page/login-page.css);
</style>

<style>
.pa-link, .pa-link *{
    text-decoration: none;
    cursor: pointer;
    color: #0073e6;
}
</style>


<style scoped>

</style>
