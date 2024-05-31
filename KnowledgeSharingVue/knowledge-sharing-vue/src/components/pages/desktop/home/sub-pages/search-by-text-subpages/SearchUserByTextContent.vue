

<template>
    <div class="p-search-user-by-text-content">
        <div class="p-subt-result-card card">
            <div class="p-subt-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
                <!-- <div class="p">
                    <MButton label="Show query" :onclick="resolveClickShowQuery" />
                </div> -->
            </div>
            <div class="p-subt-result">
                <div class="p-subt-result-notfound" v-if="isOutOfUser && listUser.length == 0" >
                    <NotFoundPanel 
                        text="Không tìm thấy người dùng nào" 
                    />
                </div>
                <div class="p-subt-result-list">
                    <ProfileFriendCard v-for="user in listUser"
                        :key="user.UserId"
                        :user="user"
                    />
                    <ProfileFriendCardSkeleton v-if="!isOutOfUser" />
                    <ProfileFriendCardSkeleton v-if="!isOutOfUser" />
                    <ProfileFriendCardSkeleton v-if="!isOutOfUser" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import ProfileFriendCard from '../../../profile/components/profile-friend-sp/ProfileFriendCard.vue';
import ProfileFriendCardSkeleton from '../../../profile/components/profile-friend-sp/ProfileFriendCardSkeleton.vue';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';


export default {
    name: 'SearchUserByTextContent',
    components: {
        NotFoundPanel,
        ProfileFriendCard,
        ProfileFriendCardSkeleton
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            listUser: [],
            searchKey: '',
            isLoaded: false,
            isOutOfUser: false,
            isWorking: false,
        }
    },
    async created(){
    },
    async mounted(){
        this.createdPage();
        this.registerScrollHandler(this.resolveOnScroll.bind(this));
    },
    methods: {
        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;
                if (this.isOutOfUser || this.isWorking){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averagePostHeight = 200;
                let leftPostNumber = 6;

                if (scrollHeight - scrollPosition < averagePostHeight * leftPostNumber){
                    // console.log("Load more post");
                    this.loadMoreUser();
                }
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickShowQuery(){
            console.log(this.route.query);
            console.log(this.$route.query);
        },

        async createdPage(){
            try {
                this.isLoaded = false;
                this.searchKey = this.route.query['search'];
                this.listUser = [];
                this.isOutOfUser = false;
                this.loadMoreUser();
            } catch (e){
                console.error(e);
            } finally {
                this.isLoaded = true;
            }
        },

        async loadMoreUser(){
            if (this.isWorking || this.isOutOfUser) return;
            try {
                this.isWorking = true;

                // prepare request
                let url = 'Users/search';
                if (Validator.isEmpty(this.searchKey)){
                    this.listUser = [];
                    this.isOutOfUser = true;
                    return;
                }
                let limit = 20;
                let offset = this.listUser.length;

                // call request
                let res = await new GetRequest(url)
                    .setParams({searchKey: this.searchKey, limit: limit, offset: offset})
                    .execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // read data:
                if (body.length < limit){
                    this.isOutOfUser = true;
                }
                if (body.length > 0){
                    let tempUsers = body.map(function(user){
                        return new ResponseUserCardModel().copy(user);
                    });
                    this.listUser = this.listUser.concat(tempUsers);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        }
    },
    watch: {
        '$route.query.search': {
            handler(){
                this.createdPage();
            },
            deep: true,
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getCurrentUser: {},
        registerScrollHandler: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-search-user-by-text-content{
    max-width: 100%;
    width: 100%;
    padding-bottom: 32px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    position: relative;
    gap: 16px;
}

.p-subt-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-subt-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-subt-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-subt-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-subt-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

