

<template>
    <BaseAuthenticationPage>
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
            <MSlotedTextfield :autocomplete="'text'" type="text" :placeholder=" getLabel()?.verificationCode " :isShowTitle="false" 
                ref="verificationCode" :validator="validator.verificationCode"
                :errorMessage="getLabel()?.invalidVerificationCode"
                />
        </form>
        <div class="pfp-links">
            <router-link class="pa-link" to="/login" >
                {{ getLabel()?.login }}
            </router-link>
        </div>
        
        <div class="pfp-button">
            <!-- Thẻ button login -->
            <MButton :label=" getLabel()?.header" :onclick="resolveSubmit" ref="button" />
        </div>
        
    </BaseAuthenticationPage>
</template>


<script>
import BaseAuthenticationPage from '../base-page/BaseAuthenticationPage.vue';
import MSlotedTextfield from '@/components/base/inputs/MSlotedTextfield.vue';
import MButton from '@/components/base/buttons/MButton.vue';
import { NotEmptyValidator } from '@/js/utils/validator';
import { PostRequest, Request } from '@/js/services/request';
import { Validator } from '@/js/utils/validator';
import { useRoute } from 'vue-router';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSEnterForgotPasswordVerificationCode',
    data(){
        return {
            label: null,
            global: this.globalData,
            
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
        BaseAuthenticationPage, MSlotedTextfield, MButton
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
                this.label = this.getLanguage()?.pages?.enterforgotpasswordverificationcode;
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
                let response = await new PostRequest('/Authentications/check-reset-password-verify-code')
                    .setBody({
                        Email: this.email,
                        Code: code,
                        AccessCode: this.accessCode
                    }).execute();

                // call API success
                let body = await Request.tryGetBody(response);
                let activeCode = body?.ActiveCode;
                await this.resolveSubmitSuccess(this.email, activeCode);

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
         * @param activeCode - mã token truy nhập từ api trả về
         * @param code - mã xác minh nhận được từ email
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmitSuccess(email, activeCode){
            try {
                // navigate to submit code page
                this.$router.push({ path: '/set-new-password', 
                    query: { email: email, activeCode: activeCode} });
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
         * Xử lý yêu cầu hiển thị lỗi
         * @param errorMessage - lỗi cần hiển thị
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async showErrorMsg(errorMessage){
            if (Validator.isEmpty(errorMessage))
                return;
            await this.getToastManager()?.error(errorMessage);
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
@import url(@/css/pages/authentication/forgot-password-page/forgot-password-page.css);
</style>


