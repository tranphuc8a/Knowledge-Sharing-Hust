

<template>
    <div>
        <form class="pr-input" v-on:keypress.enter.prevent="resolveEnterForm">
            <MSlotedTextfield :autocomplete="'username'" type="text" :placeholder=" getLabel()?.username " :isShowTitle="false" 
                ref="username" :validator="validator.username" :title="getLabel()?.username"
                :errorMessage="getLabel()?.invalidUsername"
                />
    
            <MPasswordTextfield :autocomplete="'current-password'" :placeholder="getLabel()?.password"
                ref="password" :is-show-title="false" :title="getLabel()?.password"
                :onchange="resolveOnChangePassword"
                :validator="validator.password"
                :errorMessage="getLabel()?.invalidPassword"
                />
    
            <MPasswordTextfield :autocomplete="'current-password'" :placeholder="getLabel()?.repassword"
                ref="repassword" :validator="validator.repassword" :title="getLabel()?.repassword"
                :errorMessage="getLabel()?.invalidRepassword" :is-show-title="false"
                />
            
            
        </form>
    
        <slot />
    
        <div class="pr-button">
            <MButton :label=" getLabel()?.button" :onclick="resolveSubmit" ref="button" />
        </div>
    </div>

</template>


<script>
import MSlotedTextfield from '@/components/base/inputs/MSlotedTextfield.vue';
import MPasswordTextfield from '@/components/base/inputs/MPasswordTextfield.vue';
import MButton from '@/components/base/buttons/MButton.vue';
import { UsernameValidator, PasswordValidator, RepasswordValidator, Validator } from '@/js/utils/validator';
import { Request } from '@/js/services/request';
import statusCodeEnum from '@/js/resources/status-code-enum';

export default {
    name: 'CreateNewUserForm',
    data(){
        return {
            label: null,

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
    },
    components: {
        MSlotedTextfield, MButton, MPasswordTextfield
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
                this.label = this.getLanguage()?.pages?.createnewuser;
            }
            return this.label;
        },

        /**
         * Xử lý sự kiện thay đổi password
         * @param none
         * @return none
         * @Created PhucTV (4/3/24)
         * @Modified None
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
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmit(){
            try {
                // Validate verificationCode:
                if (! (await this.validateForm())){
                    return;
                }

                // Validate success, call API:
                let account = await this.getAccount();
                let result = await this.registerUserCallback(account.username, account.password);
                
                // success:
                await this.resolveSubmitSuccess(result);

            } catch (error){
                console.error(error);
                await this.resolveSubmitFailure(error);
            }
        },


        /**
         * Thực hiện validate Form trước khi submit
         * @param - none
         * @return - true: form hợp lệ, false - form không hợp lệ
         * @Created PhucTV (4/3/24)
         * @Modified None
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
         * @Created PhucTV (4/3/24)
         * @Modified None
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
         * @Created PhucTV (04/03/24)
         * @Modified None
        */
        async resolveSubmitSuccess(result){
            try {
                let userMessage = Request.tryGetUserMessage(result);
                await this.showSuccessMsg(userMessage);
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện submit that bai
         * @param error - Lỗi trả về của API
         * @returns none
         * @Created PhucTV (04/03/24)
         * @Modified None
         */
        async resolveSubmitFailure(error){
            try {
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
        getLanguage: {},
        getPopupManager: {}, 
        getToastManager: {}
    },
    props: {
        registerUserCallback: {
            type: Function,
            default: async function(username, password){
                console.log("Call registerUserCallback: ", username, password);
            }
        }
    }
}
</script>


<style scoped>
@import url(@/css/pages/authentication/register-page/register-page.css);
</style>


