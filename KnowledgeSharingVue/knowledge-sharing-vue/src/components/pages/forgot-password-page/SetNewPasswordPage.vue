

<template>
    <div class="pfp-background">
        <div class="pfp-form-container">
            <div class="pfp-form">
                <div class="pfp-logo-frame">
                    <div class="pfp-logo"></div>
                </div>
                <div class="pfp-header-and-description">
                    <div class="pfp-header"> 
                        {{ getLabel()?.header }}
                    </div>
                    <div class="pfp-description">
                        {{ getLabel()?.description }}
                    </div>
                </div>
                <form class="pfp-input" v-on:keypress.enter="resolveEnterForm">
                    <AmisPasswordTextField :autocomplete="'current-password'" :placeholder="getLabel()?.password"
                        ref="password" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" 
                        :onchange="resolveOnChangePassword"
                        :validator="validator.password"
                        :errorMessage="getLabel()?.invalidPassword"
                        />

                    <AmisPasswordTextField :autocomplete="'current-password'" :placeholder="getLabel()?.repassword"
                        ref="repassword" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.repassword"
                        :errorMessage="getLabel()?.invalidRepassword"
                        />
                </form>
                <div class="pfp-error-message" v-show="isShowError">
                    {{ errorMessage }}
                </div>
                
                <div class="pfp-button">
                    <!-- Thẻ button login -->
                    <AmisSubmitButton :label=" getLabel()?.header" :onclick="resolveSubmit" ref="button" />
                </div>
                <div class="pfp-links pa-link">
                    <router-link to="/login" >
                        {{ getLabel()?.login }}
                    </router-link>
                </div>
            </div>
            <div class="pfp-copyright">
                Copyright © 2012 - 2024 KS JSC
            </div>
        </div>
        <div class="pfp-change-language-button">
            <ChangeLanguageButton />
        </div>
    </div>
</template>


<script>
import ChangeLanguageButton from '@/components/base/authentication/MChangeLanguageButton.vue';
// import AmisTextField from '@/components/base/authentication/MTextField.vue';
import AmisPasswordTextField from '@/components/base/authentication/MPasswordTextField.vue'
import AmisSubmitButton from '@/components/base/authentication/MSubmitButton.vue';
import { PostRequest, Request } from '@/js/services/request';
import { PasswordValidator, RepasswordValidator } from '@/js/utils/validator';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import statusCodeEnum from '@/js/resources/status-code-enum';
// import { Request, PostRequest, GetRequest } from '@/js/services/request';
// import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSForgotPasswordPage',
    data(){
        return {
            label: null,
            global: this.globalData,
            isShowError: false,
            errorMessage: 'Lỗi xảy ra',
            input: {
                password: null,
                repassword: null
            },
            validator: {
                password: new PasswordValidator(this.getLabel()?.invalidPassword),
                repassword: new RepasswordValidator(this.getLabel()?.invalidRepassword)
            },
            button: null,
            email: null,
            accessCode: null,
            code: null,
        }
    },
    mounted(){
        this.getLabel();
        this.input = {
            password: this.$refs.password,
            repassword: this.$refs.repassword
        }
        this.button = this.$refs.button;
        this.route = useRoute();
        this.email = this.route.query.email;
        this.accessCode = this.route.query.accessCode;
        this.code = this.route.query.code;
    },
    components: {
        ChangeLanguageButton, AmisPasswordTextField, AmisSubmitButton
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
                this.label = this.inject.language.pages.setnewpassword;
            }
            return this.label;
        },


        /**
         * Xử lý sự kiện click submit button
         * @param none
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmit(){
            let password = this.input?.password;
            let repassword = this.input?.repassword;
            if (!password || !repassword) return;

            try {
                if (password.startDynamicValidate)
                    password.startDynamicValidate();
                if (repassword.startDynamicValidate)
                    repassword.startDynamicValidate();

                let isOkay = (await password.validate()) && (await repassword.validate());
                if (!isOkay) return;

                // validate success, call API:
                let passwordValue = await password.getValue();
                let result = await new PostRequest('Authenticate/ResetPassword')
                    .setBody({
                        Email: this.email,
                        AccessCode: this.accessCode,
                        Code: this.code,
                        Password: passwordValue
                    }).execute();
                
                // success:
                await this.resolveSubmitSuccess(result);

            } catch (error){
                console.error(error);
                let userMessage = Request.tryGetUserMessage(error);
                if (userMessage) this.showErrorMsg(userMessage);

                Request.resolveAxiosError(error, {
                    [statusCodeEnum.SERVER_ERROR]: async function(){},
                    [statusCodeEnum.UNAUTHORIZED]: async function(){},
                    [statusCodeEnum.BAD_REQUEST]: async function(){},
                    [statusCodeEnum.FORBIDDEN]: async function(){},
                });

                if (password.stopDynamicValidate)
                    password.stopDynamicValidate();
                if (repassword.stopDynamicValidate)
                    repassword.stopDynamicValidate();
            }
        },


        /**
         * Xử lý sự kiện submit code thành công
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmitSuccess(response){
            try {
                // navigate to success page
                let userMessage = Request.tryGetUserMessage(response);
                this.showErrorMsg(userMessage);
            } catch (error){
                console.error(error);
            }
        },


        /**
         * Xử lý sự kiện nhấn enter
         * @param none
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveEnterForm(){
            try {
                if (this.button?.resolveOnclick){
                    await this.button?.resolveOnclick();
                }
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện thay đổi mật khẩu
         * @param none
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveOnChangePassword(){
            try {
                let repasswordValidator = this.validator?.repassword;
                let passwordTextfield = this.input?.password;
                let repasswordTextfield = this.input?.repassword;
                if (!repasswordValidator?.setOriginPassword 
                    || !passwordTextfield?.getValue 
                    || !repasswordTextfield?.validate)
                    return;
                let password = await passwordTextfield.getValue();
                repasswordValidator.setOriginPassword(password);
                repasswordTextfield.validate();
            } catch (error){
                console.error(error);
            }
        },


        /**
         * Xử lý yêu cầu ẩn lỗi
         * @param none
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async hideErrorMsg(){
            this.isShowError = false;
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
            this.errorMessage = errorMessage;
            this.isShowError = true;
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
@import url(@/css/pages/forgot-password-page/forgot-password-page.css);
</style>


