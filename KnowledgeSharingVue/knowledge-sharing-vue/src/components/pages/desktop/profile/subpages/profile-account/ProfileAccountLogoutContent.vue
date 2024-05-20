

<template>
    <div class="p-profile-account-logout-content">
        <div class="p-logout-frame">
            <MButton 
                label="Đăng xuất phiên đăng nhập hiện tại" 
                :onclick="resolveLogout"
                />
        </div>
        <div class="p-logout-all-frame">
            <MButton 
                label="Đang xuất khỏi mọi phiên đăng nhập"
                :onclick="resolveLogoutAll"
                />
        </div>
    </div>
</template>



<script>
import MButton from './../../../../../base/buttons/MButton.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { PostRequest, Request } from '@/js/services/request';
import { useRouter } from 'vue-router';

export default {
    name: 'ProfileAccountLogoutContent',
    components: {
        MButton
    },
    props: {
    },
    data(){
        return {
            router: useRouter()
        }
    },
    async mounted(){
    },
    methods: {
        async resolveLogout(){
            try {
                await CurrentUser.deleteInstance();
                await Request.deleteLocalStorage();
                this.router.push('/login');
            } catch (e){
                console.error(e);
            }
        },
        
        
        async resolveLogoutAll(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn đăng xuất khỏi tất cả các thiết bị?";
                this.getPopupManager().inform(alertMsg, this.submitLogoutAll.bind(this));
            } catch (e){
                console.error(e);
            }
        },

        async submitLogoutAll(){
            try {
                await new PostRequest('Authentications/logout-all').execute();
                await CurrentUser.deleteInstance();
                await Request.deleteLocalStorage();
                this.router.push('/login');
            } catch (e){
                Request.resolveAxiosError(e);
            }
        }
    },
    inject: {
        getPopupManager: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-account-logout-content{
    width: 100%;
    padding: 100px 0px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-around;
}

</style>

