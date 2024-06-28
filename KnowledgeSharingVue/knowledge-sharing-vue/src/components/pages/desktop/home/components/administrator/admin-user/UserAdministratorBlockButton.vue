




<template>

    <MCancelButton :onclick="resolveBlockUser" fa="lock" label="Khóa" />
    
</template>



<script>
import MCancelButton from '@/components/base/buttons/MCancelButton.vue'
import { PostRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'UserAdministratorBlockButton',
    components: {
        MCancelButton
    },
    data(){
        return {
        }
    },
    async mounted(){
    },
    methods: {
        async resolveBlockUser(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn khóa người dùng này không?";
                this.getPopupManager().warning(alertMsg, this.submitBlockUser.bind(this));
            } catch (e){
                console.error(e);
            }
        },

        async submitBlockUser(){
            try {
                let user = await this.getUser();
                let userId = user?.UserId;
                if (userId == null) {
                    this.getToastManager().error("Không tìm thấy người dùng");
                    return;
                }
                await new PostRequest("Users/admin/block-user/" + userId).execute();
                // success:
                this.getToastManager().success("Đã khóa người dùng");
                if (this.onBlockedUser != null){
                    this.onBlockedUser(userId);
                }
                user.Role = myEnum.EUserRole.Banned;
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
        onBlockedUser: {default: null},
        forceUpdateUserAdministratorButton: {}
    },
    provide(){
        return{}
    }
}

</script>

<style scoped>

</style>