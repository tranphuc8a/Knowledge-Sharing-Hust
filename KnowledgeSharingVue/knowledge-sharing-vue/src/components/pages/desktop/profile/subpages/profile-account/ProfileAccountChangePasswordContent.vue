

<template>
    <div class="p-profile-change-password-content">
        <div class="p-pcpc-form">
            <div class="p-pcpc-textfield">
                <div class="p-password-frame">
                    <MPasswordTextfield 
                        label="Nhập mật khấu " placeholder="Mật khẩu hiện tại của bạn" autocomplete="password"
                        :is-show-icon="true" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="validators.password"
                        state="normal" ref="oldpassword"/>
                </div>

                <div class="p-password-frame">
                    <MPasswordTextfield 
                        label="Nhập mật khấu mới" placeholder="Mật khẩu mới" autocomplete="password"
                        :is-show-icon="true" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="validators.password"
                        :oninput="resolveInputPassword"
                        state="normal" ref="password"/>
                </div>

                <div class="p-password-frame">
                    <MPasswordTextfield 
                        label="Nhập lại mật khẩu" placeholder="Nhập lại mật khẩu mới" autocomplete="password"
                        :is-show-icon="true" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="validators.repassword"
                        state="normal" ref="repassword"/>
                </div>
            </div>

            <div class="p-pcpc-button">
                <div clas="p-button-frame">
                    <MButton 
                        label="Đổi mật khẩu" 
                        :onclick="resolveClickChangePassword"
                        />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import MButton from './../../../../../base/buttons/MButton.vue';
import MPasswordTextfield from '@/components/base/inputs/MPasswordTextfield.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { PostRequest, Request } from '@/js/services/request';
import { PasswordValidator, RepasswordValidator } from '@/js/utils/validator';


export default {
    name: 'ProfileAccountChangePasswordContent',
    components: {
        MButton,
        MPasswordTextfield,
    },
    props: {
    },
    data(){
        return {
            validators: {
                password: new PasswordValidator("Mật khẩu không hợp lệ"),
                repassword: new RepasswordValidator("Mật khẩu không trùng khớp")
            },
            isWorking: false
        }
    },
    async mounted(){
    },

    methods: {
        async validate(){
            try {
                let oldpassword = this.$refs.oldpassword;
                let password = this.$refs.password;
                let repassword = this.$refs.repassword;

                let startDynamicValidate = async function(){
                    await oldpassword.startDynamicValidate();
                    await password.startDynamicValidate();
                    await repassword.startDynamicValidate();
                }

                if (!await oldpassword.validate()){
                    await startDynamicValidate();
                    await oldpassword.focus();
                    return false;
                }

                if (!await password.validate()){
                    await startDynamicValidate();
                    await password.focus();
                    return false;
                }

                if (!await repassword.validate()){
                    await startDynamicValidate();
                    await repassword.focus();
                    return false;
                }

                return true;
            } catch (e){
                console.error(e);
                return false;
            }
        },


        async resolveClickChangePassword(){
            if (this.isWorking) return;
            try {
                // Validate data
                this.isWorking = true;
                if (!await this.validate()){
                    return;
                }
                // Get value
                let currentUser = await CurrentUser.getInstance();
                let username = currentUser.Username;
                let oldpassword = await this.$refs.oldpassword.getValue();
                let password = await this.$refs.password.getValue();

                // call api
                await new PostRequest('Authentications/change-password')
                    .setBody({
                        Username: username,
                        Password: oldpassword,
                        NewPassword: password
                    }).execute();
                
                // Success: Reset input and toast success:
                this.$refs.oldpassword.setValue("");
                this.$refs.password.setValue("");
                this.$refs.repassword.setValue("");
                this.$refs.oldpassword.stopDynamicValidate();
                this.$refs.password.stopDynamicValidate();
                this.$refs.repassword.stopDynamicValidate();

                this.getToastManager().success("Đổi mật khẩu thành công");
                this.isWorking = false;
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveInputPassword(text){
            try {
                this.validators.repassword.setOriginPassword(text);
                this.validators.repassword.validate();
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-change-password-content{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-pcpc-form{
    width: 50%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    gap: 48px;
    padding: 20px;
}

.p-pcpc-textfield{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
}

.p-pcpc-textfield > * {
    width: 100%;
}

.p-pcpc-textfield > :first-child{
    padding-bottom: 32px;
}

.p-pcpc-button{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-button-frame{
    width: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
}

</style>

