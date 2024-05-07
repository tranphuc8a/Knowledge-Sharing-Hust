

<template>
    <div class="p-user-relation-button" v-if="isShow">
        <FriendButton v-if="userRelation === userRelationType.Friend" />
        <FolloweeButton v-else-if="userRelation === userRelationType.Followee" />
        <FollowerButton v-else-if="userRelation === userRelationType.Follower" />
        <RequesteeButton v-else-if="userRelation === userRelationType.Requestee" />
        <RequesterButton v-else-if="userRelation === userRelationType.Requester" />
        <NotRelationButton v-else-if="userRelation === userRelationType.NotInRelation" />
        <NotRelationButton v-else />
    </div>
</template>



<script>
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

export default {
    name: 'UserRelationButton',
    components: {
        NotRelationButton, FolloweeButton, FollowerButton,
        RequesteeButton, RequesterButton, FriendButton,
    },
    props: {
    },
    data(){
        return {
            user: null,
            isShow: true,
            currentUser: null,
            userRelation: null,
            userCard: null,
            userRelationType: myEnum.EUserRelationType,
        }
    },
    mounted(){
        this.refreshRelation();
    },
    methods: {
        async forceRender(){
            try {
                this.refreshRelation();
                this.isShow = false;
                this.$nextTick(() => {
                    this.isShow = true;
                });
            } catch (e) {
                console.error(e);
            }
        },

        async refreshRelation(){
            try {
                this.user = this.getUser();
                this.userRelation = this.user?.UserRelationType ?? this.userRelationType.NotInRelationType;
                this.currentUser = CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.userRelation = this.userRelationType.NotInRelation;
                    return;
                }
                if (this.getUser() == null) return;
                let res = await new GetRequest('UserRelations/relation-status/' + this.getUser().UserId)
                    .execute();
                let body = await Request.tryGetBody(res);
                this.userCard = new ResponseUserCardModel().copy(body);
                this.userRelation = this.userCard.UserRelationType;
            } catch (e) {
                console.error(e);
            }
        }
    },
    provide(){
        return {
            forceUpdateUserRelationButton: this.forceRender,
        }
    },
    inject:{
        getUser: {}
    }
}

</script>

<style scoped>

.p-user-relation-button{
    width: 100%;
}

</style>

