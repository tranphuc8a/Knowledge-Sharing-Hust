

<template>
    <div class="p-profile-panel-user-friend">
        <div class="p-number-friends">
            <span>{{ numberFriends }} bạn bè</span>
        </div>
        <div class="p-list-friends">
            <div class="p-list-friends__item" 
                v-for="(friend, index) in listFriends.slice().reverse()"
                :key="friend.UserId ?? index">
                <div class="p-list-friends__item_container">
                    <TooltipUserAvatar :user="friend" :size="userAvatarSize"/>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import ResponseFriendCardModel from '@/js/models/api-response-models/response-friend-card-model';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest, Request } from '@/js/services/request';

export default {
    name: 'ProfilePanelUserFriend',
    components: {
        TooltipUserAvatar
    },
    props: {
    },
    data(){
        return {
            userAvatarSize: 28,
            numberFriends: 0,
            listFriends: [],
            requiredFriendNumbers: 8,
            currentUser: null,
        }
    },
    async created(){
        await this.getFriends();
    },
    mounted(){
        
    },
    methods: {
        async getFriends(){
            try {
                if (this.getUser() == null) {
                    return;
                }
                this.currentUser = await CurrentUser.getInstance();
                let res = await new GetRequest('UserRelations/friends/' + this.getUser().UserId)
                    .setParams({
                        limit: this.requiredFriendNumbers,
                        offset: 0
                    }).execute();
                let body = await Request.tryGetBody(res);
                this.numberFriends = body?.Total ?? 0;
                this.listFriends = body?.Results?.filter(function(item){
                    return item != null;
                })?.map(function(item){
                    return new ResponseFriendCardModel().copy(item);
                }) ?? [];
            } catch (e) {
                Request.resolveAxiosError(e);
            }
        },

    },
    inject: {
        getUser: {},
    }
}

</script>

<style scoped>

.p-profile-panel-user-friend{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 8px;
    width: 100%;
}

.p-number-friends{
    font-size: 14.5px;
    color: var(--grey-color-600);
    font-family: 'ks-font-semibold';
    text-align: start;
}

.p-list-friends{
    display: flex;
    flex-flow: row-reverse nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 2px;
    height: 30px;
}

.p-list-friends__item{
    width: 20px;
    overflow: visible;
}

.p-list-friends__item_container{
    padding: 2px;
    border-radius: 100%;
    background-color: white;
    width: fit-content;
    height: fit-content;
    position: relative;
}

</style>

