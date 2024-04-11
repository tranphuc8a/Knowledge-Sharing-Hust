

<template>
    <BaseAuthenticationPage>
        <div class="pr-header-and-description">
            <div class="pr-header"> 
                {{ getLabel()?.header }}
            </div>
            <div class="pr-description">
                {{ getLabel()?.description }}
            </div>
        </div>
        <form class="pr-input" v-on:keypress.enter.prevent="resolveEnterForm">
            <!-- input email -->
            <MSlotedTextfield :autocomplete="'email'" type="email" :placeholder=" getLabel()?.email " :isShowTitle="false" 
                ref="email" :validator="validator.email"
                :errorMessage="getLabel()?.invalidEmail"
                />
        </form>

        <div class="pr-links">
            <router-link class="pa-link" to="/login" >
                {{ getLabel()?.login }}
            </router-link>
        </div>
        
        <div class="pr-button">
            <!-- Thẻ button login -->
            <MButton :label=" getLabel()?.header" :onclick="resolveSubmit" ref="button" />
        </div>
    
        <OtherRegisterOptions :label="getLabel()?.others" />
    </BaseAuthenticationPage>
</template>


<script>
import OtherRegisterOptions from './OtherRegisterOptions.vue';
import BaseAuthenticationPage from '@/components/pages/authentication/base-page/BaseAuthenticationPage.vue';
import MSlotedTextfield from '@/components/base/inputs/MSlotedTextfield.vue';
import MButton from '@/components/base/buttons/MButton.vue';
import { EmailValidator, Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSForgotPasswordPage',
    data(){
        return {
            label: null,
            global: this.globalData,
            
            input: {},
            validator: {
                email: new EmailValidator(this.getLabel()?.invalidEmail)
                            .setIsAcceptEmpty(false)
            },
            button: null
        }
    },
    mounted(){
        this.getLabel();
        this.input = {
            email: this.$refs.email,
        }
        this.button = this.$refs.button;
    },
    components: {
        BaseAuthenticationPage, MSlotedTextfield, MButton, OtherRegisterOptions
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
            if (this.inject?.language != null){
                this.label = this.inject?.language?.pages?.register;
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
            let email = this.input?.email;
            if (!email) return;

            try {
                // Validate email:
                if (email.startDynamicValidate)
                    email.startDynamicValidate();
                if (! await email.validate())
                    return;

                // Validate success, call API:
                let emailValue = await email.getValue();
                let result = await new GetRequest('Authentications/send-register-account-verify-code/')
                        .setParams({ email: emailValue}).execute();
                
                // Call API success:
                let body = Request.tryGetBody(result);
                await this.resolveSendCodeSuccess(emailValue, body.AccessCode);

            } catch (error){
                Request.resolveAxiosError(error, {
                    [statusCodeEnum.SERVER_ERROR]: async function(){},
                    [statusCodeEnum.UNAUTHORIZED]: async function(){},
                    [statusCodeEnum.BAD_REQUEST]: async function(){},
                    [statusCodeEnum.FORBIDDEN]: async function(){},
                });

                console.error(error);
                let userMessage = Request.tryGetUserMessage(error);
                this.showErrorMsg(userMessage);
                if (email.stopDynamicValidate)
                    email.stopDynamicValidate();
            }
        },


        /**
         * Xử lý sự kiện gửi yêu cầu lấy email thành công
         * @param email - email nhận mã xác minh
         * @param accessCode - mã token truy nhập từ api trả về
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSendCodeSuccess(email, accessCode){
            try {
                // navigate to submit code page
                this.$router.push({ path: '/enter-register-verification-code', 
                    query: { email: email, accessCode: accessCode } });
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
        inject: {},
        getPopupManager: {}, 
        getToastManager: {}
    }
}
</script>


<style scoped>
@import url(@/css/pages/authentication/register-page/register-page.css);
</style>


