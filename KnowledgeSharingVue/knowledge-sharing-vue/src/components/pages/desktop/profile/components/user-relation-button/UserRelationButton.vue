

<template>
    <div class="p-user-relation-button" style="display: flex; gap: 12px;" v-if="!isLoaded">
        <div class="skeleton" style="width: 100px; height: 36px; border-radius: 4px;">
        </div>
        <div class="skeleton" style="width: 100px; height: 36px; border-radius: 4px;">
        </div>
    </div>
    <div class="p-user-relation-button" v-if="isLoaded">
        <FriendButton v-if="userRelation === userRelationType.Friend" />
        <FolloweeButton v-else-if="userRelation === userRelationType.Followee" />
        <FollowerButton v-else-if="userRelation === userRelationType.Follower" />
        <RequesteeButton v-else-if="userRelation === userRelationType.Requestee" />
        <RequesterButton v-else-if="userRelation === userRelationType.Requester" />
        <NotRelationButton v-else-if="userRelation === userRelationType.NotInRelation" />
        <MButton 
            v-else-if="userRelation === userRelationType.IsMySelf"
            label="Chỉnh sửa trang cá nhân"
            :onclick="resolveClickButton"
            :buttonStyle="buttonStyle"
            fa="pencil" family="fas" :iconStyle="iconStyle"
        />
        <NotRelationButton v-else />
    </div>
</template>



<script>
import MButton from './../../../../../base/buttons/MButton.vue'
import NotRelationButton from './NotRelationButton.vue';
import FolloweeButton from './FolloweeButton.vue';
import FollowerButton from './FollowerButton.vue';
import RequesteeButton from './RequesteeButton.vue';
import RequesterButton from './RequesterButton.vue';
import FriendButton from './FriendButton.vue';

import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';
import CurrentUser from '@/js/models/entities/current-user';
import { myEnum } from '@/js/resources/enum';
import { GetRequest, Request } from '@/js/services/request';
import { useRouter } from 'vue-router';

export default {
    name: 'UserRelationButton',
    components: {
        NotRelationButton, FolloweeButton, FollowerButton,
        RequesteeButton, RequesterButton, FriendButton,
        MButton
    },
    props: {
        isCallApiWhenCreate: {
            type: Boolean,
            default: true,
        }
    },
    data(){
        return {
            user: null,
            isLoaded: true,
            currentUser: null,
            userRelation: null,
            userCard: null,
            userRelationType: myEnum.EUserRelationType,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            router: useRouter(),
        }
    },
    async created(){
        this.isLoaded = false;
        await this.refreshRelation(this.isCallApiWhenCreate);
        if (this.getUser() != null){
            this.isLoaded = true;
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
                this.userRelation = this.user?.UserRelationType;
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.userRelation = this.userRelationType.NotInRelation;
                    return;
                }
                if (isCallApi || (this.user != null && this.userRelation == null)){
                    if (this.user.UserId == this.currentUser.UserId){
                        this.userRelation = this.userRelationType.IsMySelf;
                        this.user.UserRelationType = this.userRelationType.IsMySelf;
                        return;
                    }
                    let res = await new GetRequest('UserRelations/relation-status/' + this.user.UserId)
                        .execute();
                    let body = await Request.tryGetBody(res);
                    this.userCard = new ResponseUserCardModel().copy(body);
                    this.userRelation = this.userCard.UserRelationType;
                    this.getUser().UserRelationId = this.userCard.UserRelationId;
                    this.getUser().UserRelationType = this.userCard.UserRelationType;
                }
            } catch (e) {
                console.error(e);
            }
        },

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
    },
    provide(){
        return {
            forceUpdateUserRelationButton: this.forceRender,
        }
    },
    inject:{
        getUser: {},
        getCurrentUser: {},
    }
}

</script>

<style scoped>

.p-user-relation-button{
    width: 100%;
}

</style>

