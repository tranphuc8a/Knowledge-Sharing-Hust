

<template>
    <div class="p-profile-panel-button" v-if="!isLoaded">
        <a-skeleton-button active size="default" shape="default" 
            :block="true" style="width: 200px" />
    </div>

    <div class="p-profile-panel-button" v-if="isLoaded">
        <div class="p-ppb-button" v-if="isNotMySelf">
            <UserRelationButton />
        </div>
        <div class="p-ppb-button" v-if="isNotMySelf">
            <MessageButton />
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
import { useRouter } from 'vue-router';

export default {
    name: 'ProfilePanelButton',
    components: {
        MButton, UserRelationButton, MessageButton
    },
    props: {
    },
    data(){
        return {
            isLoaded: false,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            currentUser: null,
            isNotMySelf: true,
            router: useRouter(),
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
                let currentUser = await CurrentUser.getInstance();
                let username = currentUser?.Username ?? currentUser?.UserId;
                if (currentUser == null) return;
                let path = '/profile/' + username + '/profile-edit';
                this.router.push(path);
            } catch (e) {
                console.error(e);
            }
        },

        async refresh(){
            try {
                this.isLoaded = false;
                if (this.getUser()?.UserId == null) return;
                let tempIsNotMySelf = true;
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser != null){
                    if (this.currentUser.UserId == this.getUser().UserId){
                        tempIsNotMySelf = false;
                    }
                }
                this.isNotMySelf = tempIsNotMySelf;
                this.isLoaded = true;
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

