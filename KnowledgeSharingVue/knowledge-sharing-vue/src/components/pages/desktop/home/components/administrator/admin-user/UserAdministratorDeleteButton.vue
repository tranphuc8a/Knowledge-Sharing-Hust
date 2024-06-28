




<template>
    
    <MActionIcon :onclick="resolveDeleteButton" fa="trash-can" :iconStyle="iconStyle"/>
    
</template>



<script>
// import MDeleteButton from '@/components/base/buttons/MDeleteButton.vue';
import { DeleteRequest, Request } from '@/js/services/request';

export default {
    name: 'UserAdministratorDeleteButton',
    components: {
        // MDeleteButton
    },
    data(){
        return {
            iconStyle: {
                color: "red"
            }
        }
    },
    async mounted(){
    },
    methods: {
        async resolveDeleteButton(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn xóa người dùng này không?";
                this.getPopupManager().warning(alertMsg, this.submitDeleteUser.bind(this));
            } catch (e){
                console.error(e);
            }
        },

        async submitDeleteUser(){
            try {
                let user = await this.getUser();
                let userId = user?.UserId;
                if (userId == null) {
                    this.getToastManager().error("Không tìm thấy người dùng");
                    return;
                }
                await new DeleteRequest("Users/admin/delete-user/" + userId).execute();
                // success:
                this.getToastManager().success("Xóa người dùng thành công");
                if (this.onUserDeleted != null){
                    this.onUserDeleted(userId);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            }
        }
    },
    inject: {
        getUser: {},
        getToastManager: {},
        getPopupManager: {},
        onUserDeleted: { default: null },
    },
    provide(){
        return {}
    }
}

</script>

<style scoped>

</style>