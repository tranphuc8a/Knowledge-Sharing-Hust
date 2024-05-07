

<template>

    <div class="p-profile-panel-button">
        <div class="p-ppb-button" v-if="isNotMySelf">
            <UserRelationButton
                :user="currentUser"
            />
        </div>
        <div class="p-ppb-button" v-if="isNotMySelf">
            <MessageButton
                :user="currentUser"
            />
        </div>
        <div class="p-ppb-button" v-if="!isNotMySelf">
            <MButton 
                label="Chỉnh sửa trang cá nhân"
                :onclick="resolveClickButton"
                :buttonStyle="buttonStyle"
                fa="pencil" family="fas" :iconStyle="iconStyle"
            />
        </div>
    </div>
</template>



<script>
import MButton from '@/components/base/buttons/MButton';
import CurrentUser from '@/js/models/entities/current-user';
import UserRelationButton from '../user-relation-button/UserRelationButton.vue';
import MessageButton from '../user-relation-button/MessageButton.vue';

export default {
    name: 'ProfilePanelButton',
    components: {
        MButton, UserRelationButton, MessageButton
    },
    props: {
    },
    data(){
        return {
            iconStyle: {
                fontSize: '14px'
            },
            buttonStyle: {
            
            },
            currentUser: CurrentUser.getInstance(),
            isNotMySelf: true,
        }
    },
    mounted(){
        try {
            this.refresh();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async resolveClickButton(){
            try {
                console.log("click edit profile button");
            } catch (e) {
                console.error(e);
            }
        },

        async refresh(){
            try {
                this.isNotMySelf = true;
                this.currentUser = CurrentUser.getInstance();
                if (this.currentUser != null && this.getUser() != null){
                    if (this.currentUser.UserId == this.getUser().UserId){
                        this.isNotMySelf = false;
                    }
                }
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {}
    }
}

</script>

<style scoped>

.p-profile-panel-button{
    height: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: flex-end;
    padding-bottom: 12px;
    gap: 12px;
}

.p-ppb-button{
    width: auto;
}

</style>

