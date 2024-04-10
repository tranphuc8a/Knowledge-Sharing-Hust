

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
                <form class="pfp-input" v-on:keypress.enter.prevent="resolveEnterForm">
                    <!-- input verificationCode -->
                    <AmisTextField :autocomplete="'text'" type="text" :placeholder=" getLabel()?.verificationCode " :isShowTitle="false" 
                        ref="verificationCode" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.verificationCode"
                        :errorMessage="getLabel()?.invalidVerificationCode"
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
import AmisTextField from '@/components/base/authentication/MSlotedTextfield.vue';
import AmisSubmitButton from '@/components/base/authentication/MSubmitButton.vue';
import { NotEmptyValidator } from '@/js/utils/validator';
import { PostRequest, Request } from '@/js/services/request';
import { Validator } from '@/js/utils/validator';
// import { Request, PostRequest, GetRequest } from '@/js/services/request';
// import statusCodeEnum from '@/js/resources/status-code-enum';
import { useRoute } from 'vue-router';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSEnterForgotPasswordVerificationCode',
    data(){
        return {
            label: null,
            global: this.globalData,
            isShowError: false,
            errorMessage: 'Mã xác minh không hợp lệ',
            input: {},
            validator: {
                verificationCode: new NotEmptyValidator(this.getLabel()?.invalidVerificationCode)
            },
            button: null,
            email: null,
            accessCode: null
        }
    },
    setup() {
        const route = useRoute();
        return { route };
    },
    mounted(){
        this.getLabel();
        this.input = {
            verificationCode: this.$refs.verificationCode,
        }
        this.button = this.$refs.button;
        this.email = this.route.query.email;
        this.accessCode = this.route.query.accessCode;
        console.log(this.email);
        console.log(this.accessCode);
    },
    components: {
        ChangeLanguageButton, AmisTextField, AmisSubmitButton
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
                this.label = this.inject.language.pages.enterforgotpasswordverificationcode;
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
            let textField = this.input?.verificationCode;
            if (!textField) return;

            try {
                if (textField.startDynamicValidate)
                    textField.startDynamicValidate();
                if (! await textField.validate())
                    return;

                // validate success, call API:
                let code = await textField.getValue();
                await new PostRequest('Authenticate/VerifyForgotPasswordCode')
                    .setBody({
                        Email: this.email,
                        Code: code,
                        AccessCode: this.accessCode
                    }).execute();

                // call API success
                await this.resolveSubmitSuccess(this.email, this.accessCode, code);

            } catch (error){
                console.error(error);

                let userMessage = Request.tryGetUserMessage(error);
                if (userMessage)
                    this.showErrorMsg(userMessage);

                Request.resolveAxiosError(error, {
                    [statusCodeEnum.SERVER_ERROR]: async function(){},
                    [statusCodeEnum.UNAUTHORIZED]: async function(){},
                    [statusCodeEnum.BAD_REQUEST]: async function(){},
                    [statusCodeEnum.FORBIDDEN]: async function(){},
                });

                if (textField.stopDynamicValidate)
                    textField.stopDynamicValidate();
            }
        },


        /**
         * Xử lý sự kiện submit code thành công
         * @param email - email nhận mã xác minh
         * @param accessCode - mã token truy nhập từ api trả về
         * @param code - mã xác minh nhận được từ email
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmitSuccess(email, accessCode, code){
            try {
                // navigate to submit code page
                this.$router.push({ path: '/set-new-password', 
                    query: { email: email, accessCode: accessCode, code: code} });
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


