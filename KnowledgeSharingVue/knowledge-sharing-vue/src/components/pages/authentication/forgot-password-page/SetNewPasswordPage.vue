
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
        <form class="pfp-input" v-on:keypress.enter="resolveEnterForm">
            <PasswordTextField :autocomplete="'current-password'" :placeholder="getLabel()?.password"
                ref="password" :is-show-title="false"
                :onchange="resolveOnChangePassword"
                :validator="validator.password"
                :errorMessage="getLabel()?.invalidPassword"
                />

            <PasswordTextField :autocomplete="'current-password'" :placeholder="getLabel()?.repassword" :is-show-title="false"
                ref="repassword" :validator="validator.repassword"
                :errorMessage="getLabel()?.invalidRepassword"
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
import PasswordTextField from '@/components/base/inputs/MPasswordTextfield.vue'
import MButton from '@/components/base/buttons/MButton.vue';
import { PostRequest, Request } from '@/js/services/request';
import { PasswordValidator, RepasswordValidator } from '@/js/utils/validator';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'KSForgotPasswordPage',
    data(){
        return {
            label: null,
            global: this.globalData,
            
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
            activeCode: null,
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
        this.activeCode = this.route.query.activeCode;
    },
    components: {
        BaseAuthenticationPage, PasswordTextField, MButton
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
                this.label = this.inject?.language?.pages?.setnewpassword;
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
                let result = await new PostRequest('Authentications/reset-password')
                    .setBody({
                        Email: this.email,
                        ActiveCode: this.activeCode,
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
                this.showSuccessMsg(userMessage);
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
        },

        /**
         * Xử lý yêu cầu hiển thị thông báo thành công
         * @param successMessage - thông báo thành công cần hiển thị
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
        */
        async showSuccessMsg(successMessage){
            if (Validator.isEmpty(successMessage))
                return;
            await this.getToastManager()?.success(successMessage);
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
@import url(@/css/pages/authentication/forgot-password-page/forgot-password-page.css);
</style>


