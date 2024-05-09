

<template>
    <div class="p-profile-subpage">
        <div class="pps-friend-card card">
            <div class="pps-friend-card__header">
                <div class="pps-header__left">
                    <span>Bạn bè</span>
                    <span> {{ numFriends }} người bạn </span>
                </div>
                <div class="pps-header__right">
                    <MSecondaryButton 
                        label="Xem tất cả bạn bè"
                        :onclick="resolveClickViewAllFriends"
                        :buttonStyle="buttonStyle"
                    />
                </div>
            </div>
            <div class="pps-list-friends">
                <div class="pps-list-friends-container">
                    <div class="pps-friend-item"
                        v-for="(friend, index) in filteredFriends"
                        :key="friend.FriendId ?? index"
                    >
                        <SquareUserItem
                            :user="friend"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { useRouter } from 'vue-router';
import SquareUserItem from '../components/profile-home-friend-subpage/SquareUserItem.vue';
import { GetRequest, Request } from '@/js/services/request';
import ResponseFriendCardModel from '@/js/models/api-response-models/response-friend-card-model';
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';

export default {
    name: 'ProfileHomeFriendSubpage',
    components: {
        SquareUserItem,
        MSecondaryButton
    },
    props: {
    },
    data(){
        return {
            router: useRouter(),
            buttonStyle: {

            },
            numFriends: 0,
            filteredFriends: [],
        }
    },
    mounted(){
        this.refreshListFriend();
    },
    methods: {
        async refreshListFriend(){
            try {
                let userId = this.getUser()?.UserId;
                if (userId == null) return;
                let res = await new GetRequest('UserRelations/friends/' + userId)
                    .setParams({
                        limit: 9,
                        offset: 0
                    }).execute();
                let body = await Request.tryGetBody(res);
                this.numFriends = body?.Total ?? 0;
                let listFriends = body?.Results?.map(function(friend){
                    return new ResponseFriendCardModel().copy(friend);
                });
                this.filteredFriends = listFriends;
            } catch (error){
                Request.resolveAxiosError(error);
            }
        },


        async resolveClickViewAllFriends(){
            try {
                let username = this.getUser()?.Username;
                if (username == null) return;
                this.router.push(`/profile/${username}/friend`);
            } catch (error){
                console.error(error);
            }
        },
    },
    inject: {
        getUser: {},
        getIsMySelf: {}
    }
}

</script>

<style scoped>

.pps-friend-card{
    padding: 16px;
    gap: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    width: 100%;
}

.pps-friend-card__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-start;
    width: 100%;
}

.pps-header__left{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
}
.pps-header__left :first-child{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}
.pps-header__left :last-child{
    font-family: 'ks-font-semibold';
    font-size: 14px;
    color: var(--grey-color-500);
}

.pps-list-friends{
    width: 100%;
}
.pps-list-friends-container{
    width: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 12px;
}
.pps-friend-item{
    width: calc((100% - 24px)/3);
}

</style>

