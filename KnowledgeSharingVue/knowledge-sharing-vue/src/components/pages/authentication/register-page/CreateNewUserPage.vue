

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

        <CreateNewUserForm :registerUserCallback="registerUserCallback" > 
            <div class="pr-links">
                <router-link class="pa-link" to="/login" >
                    {{ getLabel()?.login }}
                </router-link>
            </div>
        </CreateNewUserForm>
        
    </BaseAuthenticationPage>
</template>


<script>
import CreateNewUserForm from './CreateNewUserForm.vue';
import BaseAuthenticationPage from '@/components/pages/authentication/base-page/BaseAuthenticationPage.vue';
import { PostRequest } from '@/js/services/request';
import { useRoute } from 'vue-router';

export default {
    name: 'KSCreateNewUserPage',
    data(){
        return {
            label: null,
            email: null,
            activeCode: null,
        }
    },
    mounted(){
        this.route = useRoute();
        this.email = this.route.query.email;
        this.activeCode = this.route.query.activeCode;
    },
    components: {
        BaseAuthenticationPage, CreateNewUserForm
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
         * Hàm callback thuc hien dang ky tai khoan moi
         * @param {*} username - ten dang nhap
         * @param {*} password - mat khau
         * @returns none
         * @Created PhucTV (11/04/24)
         * @Modified None
         */
        async registerUserCallback(username, password){
            let that = this;
            return await new PostRequest('Authentications/register-account/')
                .setBody({
                    ActiveCode: that.activeCode,
                    Email: that.email,
                    Username: username,
                    Password: password
                }).execute();
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
@import url(@/css/pages/authentication/register-page/register-page.css);
</style>


