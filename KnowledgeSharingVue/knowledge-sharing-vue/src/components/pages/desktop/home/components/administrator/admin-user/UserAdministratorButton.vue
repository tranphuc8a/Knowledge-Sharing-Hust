

<template>
    <div class="p-user-administrator-button" style="display: flex; gap: 12px;" v-if="!isLoaded">
        <div class="skeleton" style="width: 100px; height: 36px; border-radius: 4px;">
        </div>
        <div class="skeleton" style="width: 100px; height: 36px; border-radius: 4px;">
        </div>
    </div>
    <div class="p-user-administrator-button" v-if="isLoaded">
        <span v-if="user?.Role == userRoleEnum.Admin" >
            <MSecondaryButton 
                :iconStyle="iconStyle" :buttonStyle="buttonStyle"
                fa="user-shield" label="Quản trị viên"
                :onclick="null"
            />
        </span>
        
        <span v-else class="p-uab-two-buttons">
            <span>
                <span v-if="user?.Role == userRoleEnum.Banned">
                    <UserAdministratorUnblockButton />
                </span>
                <span v-else>
                    <UserAdministratorBlockButton />
                </span>
            </span>
            <span>
                <UserAdministratorDeleteButton />
            </span>
        </span>
    </div>
</template>



<script>
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton.vue';
import UserAdministratorDeleteButton from './UserAdministratorDeleteButton.vue';
import UserAdministratorUnblockButton from './UserAdministratorUnblockButton.vue';
import UserAdministratorBlockButton from './UserAdministratorBlockButton.vue'

import CurrentUser from '@/js/models/entities/current-user';
import { myEnum } from '@/js/resources/enum';
import { GetRequest, Request } from '@/js/services/request';

export default {
    name: 'UserAdministratorButton',
    components: {
        MSecondaryButton,
        UserAdministratorDeleteButton,
        UserAdministratorUnblockButton,
        UserAdministratorBlockButton
    },
    props: {
        isCallApiWhenCreate: {
            type: Boolean,
            default: true
        }
    },
    data(){
        return {
            user: null,
            isLoaded: true,
            currentUser: null,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            userRoleEnum: myEnum.EUserRole,
        }
    },
    async created(){
        try {
            this.isLoaded = false;
            await this.refreshRelation(this.isCallApiWhenCreate);
            if (this.getUser?.() != null){
                this.isLoaded = true;
            }
        } catch (e){
            console.error(e);
        }
    },
    mounted(){
    },
    methods: {
        async forceRender(){
            try {
                this.isLoaded = false;
                this.refreshRelation();
                let that = this;
                this.$nextTick(() => {
                    that.isLoaded = true;
                });
            } catch (e) {
                console.error(e);
            }
        },

        async refreshRelation(isCallApi = true){
            try {
                this.user = this.getUser();
                this.userRole = this.user?.Role;
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.userRole = this.userRoleEnum.User;
                    return;
                }
                if (isCallApi || (this.user != null && this.userRole == null)){
                    if (this.user.UserId == this.currentUser.UserId){
                        this.userRole = this.currentUser.Role;
                        return;
                    }
                    let res = await new GetRequest('Users/admin/user-profile/' + this.user.UserId).execute();
                    let body = await Request.tryGetBody(res);
                    this.userRole = body.Role;
                    this.getUser().Role = body.Role;
                }
            } catch (e) {
                console.error(e);
            }
        },
    },
    provide(){
        return {
            forceUpdateUserAdministratorButton: this.forceRender,
        }
    },
    inject:{
        getUser: {},
        getCurrentUser: {},
    }
}

</script>

<style scoped>

.p-user-administrator-button{
    width: 100%;
}

.p-uab-two-buttons{
    display: flex;
    gap: 8px;
    justify-content: center;
    align-items: center;
}

</style>

