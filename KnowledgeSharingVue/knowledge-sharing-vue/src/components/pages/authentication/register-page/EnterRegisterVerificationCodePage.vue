

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
            <!-- input verificationCode -->
            <MSlotedTextfield :autocomplete="'text'" type="text" :placeholder=" getLabel()?.verificationCode " :isShowTitle="false" 
                ref="verificationCode" :validator="validator.verificationCode"
                :errorMessage="getLabel()?.invalidVerificationCode"
                />
        </form>
       
        <div class="pr-links">
            <router-link class="pa-link" to="/login" >
                {{ getLabel()?.login }}
            </router-link>
        </div>

        <div class="pr-button">
            <!-- Thẻ button login -->
            <MButton :label=" getLabel()?.button" :onclick="resolveSubmit" ref="button" />
        </div>
        
    </BaseAuthenticationPage>
</template>


<script>
import BaseAuthenticationPage from '../base-page/BaseAuthenticationPage.vue';
import MSlotedTextfield from '@/components/base/inputs/MSlotedTextfield.vue';
import MButton from '@/components/base/buttons/MButton.vue';
import { NotEmptyValidator } from '@/js/utils/validator';
import { PostRequest, Request } from '@/js/services/request';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSForgotPasswordPage',
    data(){
        return {
            label: null,
            global: this.globalData,
            
            input: {},
            validator: {
                verificationCode: new NotEmptyValidator(this.getLabel()?.invalidVerificationCode)
            },
            button: null
        }
    },
    mounted(){
        this.getLabel();
        this.input = {
            verificationCode: this.$refs.verificationCode,
        }
        this.button = this.$refs.button;
        this.route = useRoute();
        this.email = this.route.query.email;
        this.accessCode = this.route.query.accessCode;
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
                this.label = this.getLanguage()?.pages?.enterregisterverificationcode;
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
            let verificationCode = this.input?.verificationCode;
            if (!verificationCode) return;

            try {
                // Validate verificationCode:
                if (verificationCode.startDynamicValidate)
                    verificationCode.startDynamicValidate();
                if (! await verificationCode.validate())
                    return;

                // Validate success, call API:
                let verificationCodeValue = await verificationCode.getValue();
                let response = await new PostRequest('Authentications/check-register-account-verify-code/')
                    .setBody({
                        Email: this.email,
                        AccessCode: this.accessCode,
                        Code: verificationCodeValue
                    })
                    .execute();
                
                // Call API success:
                let body = await Request.tryGetBody(response);
                let activeCode = body?.ActiveCode;
                await this.resolveSubmitSuccess(this.email, activeCode);

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
                if (this.input?.verificationCode.stopDynamicValidate)
                    this.input.verificationCode.stopDynamicValidate();
            }
        },


        /**
         * Xử lý sự kiện xác minh code thành công
         * @param email - email nhận mã xác minh
         * @param activeCode - mã token truy nhập từ api trả về
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmitSuccess(email, activeCode){
            try {
                // navigate to submit code page
                this.$router.push({ path: '/create-new-user', 
                    query: { email: email, activeCode: activeCode } });
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
@import url(@/css/pages/authentication/register-page/register-page.css);
</style>


