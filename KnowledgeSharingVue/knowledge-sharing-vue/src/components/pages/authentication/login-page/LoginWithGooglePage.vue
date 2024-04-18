<template>
    <BaseAuthenticationPage>
        <div class="plg-header-and-description">
            <div class="plg-header"> 
                {{ getLabel()?.header }}
            </div>
            <div class="plg-description">
                {{ getLabel()?.description }}
            </div>
        </div>
        <div class="plg-content">
            <div class="plg-loading" v-show="isLoading">
                <MSpinner :style="{fontSize: '24px'}" />
            </div>
            <div class="pa-error-message" v-show="isShowError">
                {{ errorMessage }}
            </div>
        </div>
        
        <div class="plg-links">
            <router-link class="pa-link" to="/login" >
                {{ getLabel()?.login }}
            </router-link>

            <router-link class="pa-link" to="/register" >
                {{ getLabel()?.register }}
            </router-link>
        </div>
    </BaseAuthenticationPage>
</template>

<script>
import BaseAuthenticationPage from '@/components/pages/authentication/base-page/BaseAuthenticationPage.vue';
import { Request, PostRequest } from '@/js/services/request';
import appConfig from '@/app-config';
import statusCodeEnum from '@/js/resources/status-code-enum';
import { Validator } from '@/js/utils/validator';
import { useRoute } from 'vue-router';

export default {
    name: 'KSLoginWithGoogle',
    components: {
        BaseAuthenticationPage
    },
    data() {
        return {
            label: null,
            isLoading: true,

            isShowError: false,
            errorMessage: 'Login with Google failure',

            params: {
                state: null,
                access_token: null,
                token_type: null,
                expires_in: null,
                scope: null,
                authuser: null,
                prompt: null,
            },

            route: useRoute()
        }
    },
    async mounted() {
        this.getLabel();

        await this.getUrlParams();
        await this.submitLoginWithGoogle();
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
        */
        getLabel(){
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.pages?.loginWithGoogle;
            }
            return this.label;
        },

        /**
         * Hàm lấy tham số trên url
         * @param none
         * @returns params - tham số trên url
         * @Created PhucTV (11/04/24)
         * @Modified None
        */
        async getUrlParams() {
            try {
                const parsedParams = {};
                this.route.hash.split('&')
                .map(part => part.replace(/^#/, ''))
                .forEach(param => {
                    const parts = param.split('=');
                    parsedParams[parts[0]] = parts[1];
                });
                this.params = parsedParams;
                console.log(this.params);
                return this.params;
            } catch (error) {
                console.error(error);
                return null;
            }
        },

        /**
         * Hàm xử lý đăng nhập với google
         * @param none
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
        */
        async submitLoginWithGoogle() {
            try {
                if (Validator.isEmpty(this.params.access_token)) {
                    let msg = "Token is empty";
                    this.showErrorMsg(msg);
                    return;
                }
                let response = await new PostRequest('Authentications/login/google')
                    .setParams({token: this.params.access_token})
                    .execute();
                // login success
                this.isLoading = false;
                let tokenModel = await Request.tryGetBody(response);
                await this.resolveLoginSuccess(tokenModel);
            } catch (error) {
                // login error
                this.isLoading = false;
                await this.resolveLoginFailed(error);
            }
        },

        /**
         * Xử lý đăng nhập thành công
         * @param {*} tokenModel - chứa token lấy về từ api login
         * @returns none
         * @Created PhucTV (11/04/24)
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
         * Xử lý đăng nhập thất bại
         * @param {*} error - lỗi trả về từ api
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None 
        */
        async resolveLoginFailed(error){
            try {
                // Gọi hàm xử lý lỗi chung từ Request
                let doNothing = async function(){};
                Request.resolveAxiosError(error, {
                    // [statusCodeEnum.SERVER_ERROR]: doNothing,
                    [statusCodeEnum.UNAUTHORIZED]: doNothing,
                    [statusCodeEnum.BAD_REQUEST]: doNothing,
                    [statusCodeEnum.FORBIDDEN]: doNothing,
                });

                // Hiển thị message lỗi
                let userMessage = Request.tryGetUserMessage(error);
                this.showErrorMsg(userMessage);              
            } catch (error2) {
                console.error(error2);
            }
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
                await this.getToastManager().error(msg);
            } catch (error){
                console.error(error);
            }
        },
    },
    inject: {
        getLanguage: {},
        getPopupManager: {}, 
        getToastManager: {}
    }
}

</script>

<style scoped>
@import url(@/css/pages/authentication/login-page/login-with-google.css);
</style>