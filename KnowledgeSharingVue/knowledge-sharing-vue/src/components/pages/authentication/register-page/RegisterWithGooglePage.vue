<template>
    <BaseAuthenticationPage>
        <div class="prg-header-and-description">
            <div class="prg-header"> 
                {{ getLabel()?.header }}
            </div>
            <div class="prg-description">
                {{ getLabel()?.description }}
            </div>
        </div>

        <div class="plg-content" v-show="!isShowInputForm">
            <div class="prg-loading" v-show="isLoading">
                <MSpinner :style="{fontSize: '24px'}" />
            </div>
            <div class="pa-error-message" v-show="isShowError">
                {{ errorMessage }}
            </div>
        </div>
        
        
        <CreateNewUserForm v-show="isShowInputForm" :registerUserCallback="registerUserCallback"> 
            <div class="prg-links">
                <router-link class="pa-link" to="/register" >
                    {{ getLabel()?.register }}
                </router-link>
    
                <router-link class="pa-link" to="/login" >
                    {{ getLabel()?.login }}
                </router-link>
            </div>
        </CreateNewUserForm>

        <div class="prg-links" v-show="!isShowInputForm">
            <router-link class="pa-link" to="/register" >
                {{ getLabel()?.register }}
            </router-link>

            <router-link class="pa-link" to="/login" >
                {{ getLabel()?.login }}
            </router-link>
        </div>
        
    </BaseAuthenticationPage>
</template>

<script>
import CreateNewUserForm from './CreateNewUserForm.vue';
import BaseAuthenticationPage from '@/components/pages/authentication/base-page/BaseAuthenticationPage.vue';
import { Request, PostRequest, GetRequest } from '@/js/services/request';
import { Validator } from '@/js/utils/validator';
import { useRoute } from 'vue-router';

export default {
    name: 'KSRegisterWithGoogle',
    components: {
        BaseAuthenticationPage, CreateNewUserForm
    },
    data() {
        return {
            label: null,
            isLoading: true,
            isShowInputForm: false,

            isShowError: false,
            errorMessage: "Failure register with Google",

            params: {
                state: null,
                access_token: null,
                token_type: null,
                expires_in: null,
                scope: null,
                authuser: null,
                prompt: null,
                email: null
            },

            route: useRoute()
        }
    },
    async mounted() {
        this.getLabel();

        let res = await this.getUrlParams();
        if (res == null){
            this.isLoading = false;
            return;
        }
        await this.submitRegisterWithGoogleToken();
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
                this.label = this.getLanguage()?.pages?.registerWithGoogle;
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

                let url = 'https://www.googleapis.com/oauth2/v2/userinfo';
                let res = await new GetRequest(url)
                    .setToken(this.params.access_token)
                    .setIsCallOnce(true)
                    .execute();
                this.email = res.data.email;
                return this.params;
            } catch (error) {
                console.error(error);
                await this.showErrorMsg(this.getLabel()?.errorMessage);
                return null;
            }
        },

        /**
         * Hàm xử lý dang ky với google token
         * @param none
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
        */
        async submitRegisterWithGoogleToken() {
            try {
                if (Validator.isEmpty(this.params.access_token)) {
                    let msg = "Đăng ký thất bại, vui lòng thử lại sau";
                    this.showErrorMsg(msg);
                    return;
                }
                let response = await new PostRequest('Authentications/register/google')
                    .setParams({token: this.params.access_token})
                    .setIsCallOnce(true)
                    .execute();

                // check token success
                // hide spinner and show input new account form
                this.isShowInputForm = true;
                let body = await Request.tryGetBody(response);
                this.activeCode = body?.ActiveCode;
            } catch (error) {
                // login error
                let userMessage = await Request.tryGetUserMessage(error);
                await this.showErrorMsg(userMessage);
            } finally {
                this.isLoading = false;
            }
        },

        /**
         * Hàm callback thuc hien dang ky tai khoan moi
         * @param {*} username - ten dang nhap
         * @param {*} password - mat khau
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
         */
        async registerUserCallback(username, password){
            let that = this;
            return await new PostRequest('Authentications/create-new-user')
                .setBody({
                    ActiveCode: that.activeCode,
                    Email: that.email,
                    Username: username,
                    Password: password
                }).execute();
        },

        /**
         * Xử lý yêu cầu hiển thị lỗi
         * @param errorMessage - lỗi cần hiển thị
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async showErrorMsg(errorMessage){
            if (Validator.isEmpty(errorMessage))
                return;
            this.isShowError = true;
            this.errorMessage = errorMessage;
            await this.getToastManager()?.error(errorMessage);
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
@import url(@/css/pages/authentication/register-page/register-with-google.css);
</style>