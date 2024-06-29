

<template>
    <!-- <div class="p-profile-relation-content" v-if="!isLoaded">
        <div class="p-prc-list-user">
            <div class="p-prc-user-item"
                v-for="index in [1, 2, 3]"
                :key="index"
            >
                <ProfileFriendCardSkeleton />
            </div>
        </div>
    </div> -->

    <div class="p-profile-relation-content" v-if="true">
        <div class="p-empty-relation" v-if="isOutOfUser && !(listUser.length > 0)">
            <NotFoundPanel text="Không tìm thấy mục nào" />
        </div>
        <div class="p-prc-list-user">
            <div class="p-prc-user-item"
                v-for="(usercard, index) in listUser"
                :key="index + random()"
            >
                <ProfileFriendCard :user="usercard" />
            </div>
            
            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <ProfileFriendCardSkeleton />
            </div>

            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <ProfileFriendCardSkeleton />
            </div>

            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <ProfileFriendCardSkeleton />
            </div>

        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import ProfileFriendCardSkeleton from '../../components/profile-friend-sp/ProfileFriendCardSkeleton.vue';
import ProfileFriendCard from '../../components/profile-friend-sp/ProfileFriendCard.vue';
import ResponseFriendCardModel from '@/js/models/api-response-models/response-friend-card-model';
import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';
import { MyRandom } from '@/js/utils/myrandom';
// import { myEnum } from '@/js/resources/enum';
import { Request } from '@/js/services/request';

export default {
    name: 'ProfileRelationContent',
    components: {
        ProfileFriendCard, ProfileFriendCardSkeleton, NotFoundPanel
    },
    props: {
        getMoreUser: {
            required: true,
        },
        userRelationType: {}
    },
    watch: {
        getMoreUser: {
            handler: function() {
                this.refresh();
            },
            immediate: true,
        },
    },
    data(){
        return {
            listUser: [],
            isOutOfUser: false,
            isLoaded: false,
            isWorking: false,
        }
    },
    async mounted(){
        try {
            this.refresh();
            this.registerScrollHandler(this.resolveOnScroll.bind(this));
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        random(){
            return MyRandom.generateUUID();
        },

        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageItemHeight = 300;
                let leftItemNumber = 4;

                if (scrollHeight - scrollPosition < averageItemHeight * leftItemNumber){
                    // console.log("Load more post");
                    await this.loadMoreFriend();
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                this.isLoaded = false;
                this.isOutOfUser = false;
                this.listUser = [];
                await this.loadMoreFriend();
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreFriend(){
            if (this.isWorking || this.isOutOfUser) return;
            try {
                // update status
                this.isWorking = true;

                // prepare params and call api
                let limit = 15;
                let offset = this.listUser?.length;
                if (this.getMoreUser == null) return; 
                let response = await this.getMoreUser(limit, offset);
                let body = await Request.tryGetBody(response);

                // read response
                let listed = body;
                if (listed?.Results != null) listed = listed.Results;

                let tempResponseFriendModel = listed.map(function(item){
                    return new ResponseFriendCardModel().copy(item);
                });
                let userRelationType = this.userRelationType;
                let tempResponseUserModel = tempResponseFriendModel.map(function(item){
                    let userCard = new ResponseUserCardModel().copy(item);
                    userCard.UserRelationType = userCard.UserRelationType ?? userRelationType;
                    userCard.UserRelationId = item.FriendId;
                    return userCard;
                });

                // update status and data
                if (tempResponseUserModel.length < limit) {
                    this.isOutOfUser = true;
                }
                if (tempResponseUserModel.length > 0){
                    let listLoadedIds = this.listUser.map(function(item){
                        return item.UserId;
                    });
                    tempResponseUserModel = tempResponseUserModel.filter(function(item){
                        return !listLoadedIds.includes(item.UserId);
                    });
                    this.listUser = this.listUser.concat(tempResponseUserModel);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
        registerScrollHandler: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-relation-content{
    width: 100%;
    min-height: 150px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
}

.p-prc-list-user{
    width: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
    padding: 0px;
    margin: 0px;
}

.p-prc-user-item{
    flex-shrink: 0;
    flex-grow: 0;
    width: calc((100% - 16px)/2);
}

.p-empty-relation{
    width: 100%;
    flex-grow: 1;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

</style>

