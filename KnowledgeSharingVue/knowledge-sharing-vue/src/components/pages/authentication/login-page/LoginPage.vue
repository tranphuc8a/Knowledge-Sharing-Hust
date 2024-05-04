
<template>
    <BaseAuthenticationPage>
        <div class="pl-header-and-description">
            <div class="pl-header"> 
                {{ getLabel()?.header }}
            </div>
            <!-- <div class="pl-description">
                {{ getLabel()?.description }}
            </div> -->
        </div>

        <form class="pl-input" v-on:keypress.enter="resolveEnterForm">
            <!-- 2 thẻ input -->
            <SlotedTextfield :autocomplete="'username'" type="text" :placeholder=" getLabel()?.username " :isShowTitle="false" 
                ref="username" :validator="validator.username"
                :errorMessage="getLabel()?.invalidUsername"
                />
            <PasswordTextfield :autocomplete="'current-password'" :placeholder="getLabel()?.password" :is-show-title="false"
                ref="password" :validator="validator.password"
                :errorMessage="getLabel()?.invalidPassword"
                />
            <div class="pl-input-captcha" v-show="isLoginWithCaptcha">
                <div class="pl-input-captcha-textfield">
                    <SlotedTextfield type="text" :placeholder=" getLabel()?.captcha " :isShowTitle="false" 
                        ref="captcha" :validator="validator.captcha"
                        :errorMessage="getLabel()?.invalidCaptcha">
                        <MActionIcon fa="rotate-right" :onclick="refreshCaptcha" 
                            :iconStyle="{width: '18px', height: '18px'}"
                            :containerStyle="{width: '24px', height: '24px'}"/>
                    </SlotedTextfield>
                </div>
                <div class="pl-captcha-image-frame">
                    <img v-show="captchaImageData != null" class="pl-captcha-image" :src="`data:image/png;base64,${captchaImageData}`" alt="Captcha">
                </div>
            </div>
        </form>

        <div class="pl-links">
            <router-link class="pa-link" to="/forgotpassword" >
                {{ getLabel()?.forgotpassword }}
            </router-link>

            <router-link class="pa-link" to="/register" >
                {{ getLabel()?.register }}
            </router-link>
        </div>

        <div class="pl-button">
            <!-- Thẻ button login -->
            <MButton :label=" getLabel()?.login" :onclick="resolveSubmitLogin" ref="button" />
        </div>
        
        <OtherLoginOptions :label="getLabel()?.others" />
        
    </BaseAuthenticationPage>
</template>


<script>
import OtherLoginOptions from '@/components/pages/authentication/login-page/OtherLoginOptions';
import BaseAuthenticationPage from '@/components/pages/authentication/base-page/BaseAuthenticationPage.vue';
import SlotedTextfield from '@/components/base/inputs/MSlotedTextfield.vue';
import PasswordTextfield from '@/components/base/inputs/MPasswordTextfield.vue';
import MButton from '@/components/base/buttons/MButton.vue';
import { UsernameValidator, PasswordValidator, Validator, NotEmptyValidator } from '@/js/utils/validator';
import { Request, PostRequest, GetRequest } from '@/js/services/request';
import statusCodeEnum from '@/js/resources/status-code-enum';
import appConfig from '@/app-config';

export default {
    name: 'KSLoginPage',
    data(){
        return {
            global: this.globalData,

            label: null,
            isLoginWithCaptcha: false,
            captchaImageData: null,
            captchaToken: null,

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
        // Check if loged in
        let userLogin = await new GetRequest().checkLogedIn();
        if (userLogin != null){
            let lastUrl = localStorage.getItem("redirect-to");
            if (Validator.isEmpty(lastUrl)){
                lastUrl = '/'; // to home page
            }
            this.$router.push(lastUrl);
        }

        // prepare data
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
        BaseAuthenticationPage, SlotedTextfield, PasswordTextfield,
        MButton, OtherLoginOptions
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
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.pages?.login;
            }
            return this.label;
        },

        /**
         * Hai hàm thực hiện show lỗi đăng nhập từ api
         * @param {*} msg - lỗi hiển thị
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None 
        */
        async showErrorMsg(msg){
            try {
                if (Validator.isEmpty(msg)) return;
                this.getToastManager().error(msg);
            } catch (error){
                console.error(error);
            }
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
                let result = await new GetRequest('/Authentications/refresh-captcha').execute();
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
                let result = await new PostRequest('/Authentications/login').setBody({
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
                let result = await new PostRequest('/Authentications/login-with-captcha').setBody({
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
                if (this.isLoginWithCaptcha || statusCode == statusCodeEnum.BAD_REQUEST){
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
        
        /**
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
                    redirectTo = appConfig.getHomePageUrl();
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
        getLanguage: {},
        getPopupManager: {}, 
        getToastManager: {}
    }
}
</script>


<style scoped>
@import url(@/css/pages/authentication/login-page/login-page.css);
</style>

