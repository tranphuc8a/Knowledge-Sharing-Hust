




<template>
    
    <MButton :onclick="resolveUnblockUser" fa="unlock" label="Mở khóa" />
    
</template>



<script>
import MButton from '@/components/base/buttons/MButton.vue'
import { PostRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'UserAdministratorUnblockButton',
    components: {
        MButton
    },
    data(){
        return {
        }
    },
    async mounted(){
    },
    methods: {
        async resolveUnblockUser(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn mở khóa người dùng này không?";
                this.getPopupManager().inform(alertMsg, this.submitUnblockUser.bind(this));
            } catch (e){
                console.error(e);
            }
        },

        async submitUnblockUser(){
            try {
                let user = await this.getUser();
                let userId = user?.UserId;
                if (userId == null) {
                    this.getToastManager().error("Không tìm thấy người dùng");
                    return;
                }
                await new PostRequest("Users/admin/unblock-user/" + userId).execute();
                // success:
                this.getToastManager().success("Đã mở khóa người dùng");
                if (this.onUnblockedUser != null){
                    this.onUnblockedUser(userId);
                }
                user.Role = myEnum.EUserRole.User;
                this.forceUpdateUserAdministratorButton();
            } catch (e){
                Request.resolveAxiosError(e);
            }
        }
    },
    inject: {
        getUser: {},
        getToastManager: {},
        getPopupManager: {},
        onUnblockedUser: {default: null},
        forceUpdateUserAdministratorButton: {}
    },
    provide(){
        return {}
    }
}

</script>

<style scoped>

</style>