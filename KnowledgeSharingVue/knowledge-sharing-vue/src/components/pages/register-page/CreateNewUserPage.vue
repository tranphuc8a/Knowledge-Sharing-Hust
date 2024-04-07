

<template>
    <div class="pr-background">
        <div class="pr-form-container">
            <div class="pr-form">
                <div class="pr-logo-frame">
                    <div class="pr-logo"></div>
                </div>
                <div class="pr-header-and-description">
                    <div class="pr-header"> 
                        {{ getLabel()?.header }}
                    </div>
                    <div class="pr-description">
                        {{ getLabel()?.description }}
                    </div>
                </div>
                <form class="pr-input" v-on:keypress.enter.prevent="resolveEnterForm">
                    <AmisTextField :autocomplete="'username'" type="text" :placeholder=" getLabel()?.username " :isShowTitle="false" 
                        ref="username" :onfocus="hideErrorMsg" :oninput="hideErrorMsg" :validator="validator.username"
                        :errorMessage="getLabel()?.invalidUsername"
                        />

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
                <div class="pr-error-message" v-show="isShowError">
                    {{ errorMessage }}
                </div>
                
                <div class="pr-button">
                    <!-- Thẻ button login -->
                    <AmisSubmitButton :label=" getLabel()?.button" :onclick="resolveSubmit" ref="button" />
                </div>
                <div class="pr-links pa-link">
                    <router-link to="/login" >
                        {{ getLabel()?.login }}
                    </router-link>
                </div>
                <div class="pr-divide">
                    <div class="pr-segment-frame">
                        <div class="pr-segment"></div>
                    </div>
                    <div class="pr-text-divide">
                        {{ getLabel()?.others }}
                    </div>
                </div>
                <div class="pr-login-options">
                    <div class="pr-option pr-login-google">
                    </div>
                    <div class="pr-option pr-login-apple">
                    </div>
                    <div class="pr-option pr-login-microsoft">
                    </div>
                </div>
            </div>
            <div class="pr-copyright">
                Copyright © 2012 - 2024 KS JSC
            </div>
        </div>
        <div class="pr-change-language-button">
            <ChangeLanguageButton />
        </div>
    </div>
</template>


<script>
import ChangeLanguageButton from '@/components/base/authentication/MChangeLanguageButton.vue';
import AmisTextField from '@/components/base/authentication/MTextField.vue';
import AmisPasswordTextField from '@/components/base/authentication/MPasswordTextField.vue';
import AmisSubmitButton from '@/components/base/authentication/MSubmitButton.vue';
import { UsernameValidator, PasswordValidator, RepasswordValidator, Validator } from '@/js/utils/validator';
import { PostRequest, Request } from '@/js/services/request';
import { useRoute } from 'vue-router';
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
            errorMessage: 'Verification code is invalid',
            input: {
                username: null,
                password: null,
                repassword: null
            },
            validator: {
                username: new UsernameValidator( this.getLabel()?.invalidUsername),
                password: new PasswordValidator( this.getLabel()?.invalidPassword),
                repassword: new RepasswordValidator( this.getLabel()?.invalidRepassword)
            },
            button: null
        }
    },
    mounted(){
        this.getLabel();
        this.input = {
            username: this.$refs.username,
            password: this.$refs.password,
            repassword: this.$refs.repassword,
        }
        this.button = this.$refs.button;
        this.route = useRoute();
        this.email = this.route.query.email;
        this.accessCode = this.route.query.accessCode;
        this.code = this.route.query.code;
    },
    components: {
        ChangeLanguageButton, AmisTextField, AmisSubmitButton, AmisPasswordTextField
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * Created: PhucTV (20/2/24)
         * Modified: None
        */
        getLabel(){
            if (this.inject.language != null){
                this.label = this.inject.language.pages.createnewuser;
            }
            return this.label;
        },

        /**
         * Xử lý sự kiện thay đổi password
         * @param none
         * @return none
         * Created: PhucTV (4/3/24)
         * Modified: None
        */
        async resolveOnChangePassword(){
            try {
                let passwordTextfield = this.input?.password;
                let repasswordTextfield = this.input?.repassword;
                let repasswordValidator = this.validator?.repassword;
                if (!passwordTextfield || !repasswordTextfield || !repasswordValidator){
                    return;
                }

                let passwordValue = await passwordTextfield.getValue();
                repasswordValidator.setOriginPassword(passwordValue);
                await repasswordTextfield.validate();
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện click submit button
         * @param none
         * @returns none
         * Created: PhucTV (04/03/24)
         * Modified: None
        */
        async resolveSubmit(){
            try {
                // Validate verificationCode:
                if (! (await this.validateForm())){
                    return;
                }

                // Validate success, call API:
                let account = await this.getAccount();
                let result = await new PostRequest('Authenticate/AddNewUser/')
                        .setBody({
                            AccessCode: this.accessCode,
                            Code: this.code,
                            Email: this.email,
                            Username: account.username,
                            Password: account.password
                        }).execute();
                
                // Call API success:
                await this.resolveSubmitSuccess(result);

            } catch (error){
                console.error(error);

                Request.resolveAxiosError(error, {
                    [statusCodeEnum.SERVER_ERROR]: async function(){},
                    [statusCodeEnum.UNAUTHORIZED]: async function(){},
                    [statusCodeEnum.BAD_REQUEST]: async function(){},
                    [statusCodeEnum.FORBIDDEN]: async function(){},
                });

                let userMessage = Request.tryGetUserMessage(error);
                this.showErrorMsg(userMessage);
                for (let textfield in this.input){
                    let tf = this.input[textfield];
                    if (tf.stopDynamicValidate)
                        tf.stopDynamicValidate();
                }
            }
        },


        /**
         * Thực hiện validate Form trước khi submit
         * @param - none
         * @return - true: form hợp lệ, false - form không hợp lệ
         * Created: PhucTV (4/3/24)
         * Modified: None
        */
        async validateForm(){
            try {
                for (let textfield in this.input){
                    await this.input[textfield].startDynamicValidate();
                }
                for (let textfield in this.input){
                    if (! await this.input[textfield].validate()){
                        this.input[textfield].focus();
                        return false;
                    }
                }
                return true;
            } catch (error){
                console.error(error);
                return false;
            }
        },


        /**
         * Thực hiện lấy về thông tin username và password nhập trên form
         * @params none
         * @return - account thu thập được từ form (username, password)
         * Created: PhucTV (4/3/24)
         * Modified: None
        */
        async getAccount(){
            try {
                let account = {
                    username: null,
                    password: null
                }
                account.username = await this.input.username.getValue();
                account.password = await this.input.password.getValue();
                return account;
            } catch (error){
                console.error(error);
                return null;
            }
        },

        /**
         * Xử lý sự kiện xác minh code thành công
         * @param result - Kết quả trả về của API
         * @returns none
         * Created: PhucTV (04/03/24)
         * Modified: None
        */
        async resolveSubmitSuccess(result){
            try {
                let userMessage = Request.tryGetUserMessage(result);
                await this.showErrorMsg(userMessage);
            } catch (error){
                console.error(error);
            }
        },


        /**
         * Xử lý sự kiện nhấn enter
         * @param none
         * @returns none
         * Created: PhucTV (04/03/24)
         * Modified: None
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
         * Created: PhucTV (04/03/24)
         * Modified: None
        */
        async hideErrorMsg(){
            this.isShowError = false;
        },

        /**
         * Xử lý yêu cầu hiển thị lỗi
         * @param errorMessage - lỗi cần hiển thị
         * @returns none
         * Created: PhucTV (04/03/24)
         * Modified: None
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
@import url(@/css/pages/register-page/register-page.css);
</style>


