

<template>
    <div class="profile-lesson-subpage p-profile-main-subpage">
        <div class="profile-lesson-subpage__left"
            v-show="isMySelf"
        >
            <!-- ProfileLessonToolbar -->
            <ProfileLessonToolbar 
                :on-click-search="resolveOnChangeConfig"
            />
        </div>

        <div class="profile-lesson-subpage__right">
            <!-- ProfileLessonFeedSubpage -->
            <PostSubpage 
                :get-post="getPostCallback"
            >
                <template #addpost >
                    <AddPostFeedCard v-show="isMySelf" />
                </template>
            </PostSubpage>
        </div>
    </div>
</template>



<script>
import ProfileLessonToolbar from './ProfileLessonToolbar.vue';
import PostSubpage from '../../../home/sub-pages/PostSubpage.vue';
import { Validator } from '@/js/utils/validator';
import { myEnum } from '@/js/resources/enum';
import { GetRequest } from '@/js/services/request';
import CurrentUser from '@/js/models/entities/current-user';
import AddPostFeedCard from '../../../home/components/feed-subpage/postcard/AddPostFeedCard.vue';

export default {
    name: 'ProfileLessonSubpage',
    components: {
        ProfileLessonToolbar,
        PostSubpage,
        AddPostFeedCard,
    },
    props: {
    },
    data(){
        return {
            isMySelf: false,
            getPostCallback: null,
        }
    },
    async mounted(){
        try {
            this.isMySelf = await this.getIsMySelf();
            await this.resolveOnChangeConfig({});

        } catch (error){
            console.error(error);
        }
    },
    methods: {

        getOrderString(config){
            let { isLatest, isMostStar, isMostComment } = config;

            let orderCreatedTime, orderStar, orderComment;
            if (isLatest === true || isLatest === false){
                orderCreatedTime = "CreatedTime:" + (isLatest ? "desc" : "asc");
            }
            if (isMostStar === true || isMostStar === false){
                orderStar = "TotalStar:" + (isMostStar ? "desc" : "asc");
            }
            if (isMostComment === true || isMostComment === false){
                orderComment = "TotalComment:" + (isMostComment ? "desc" : "asc");
            }
            let orders = [orderCreatedTime, orderStar, orderComment].filter(x => x?.length > 0).join(",");

            return orders;
        },

        getFilterString(config){
            let {  isPrivate } = config;

            let filterPrivate = "";
            if (isPrivate === true || isPrivate === false){
                filterPrivate = "Privacy:" + (isPrivate ? myEnum.EPrivacy.Private : myEnum.EPrivacy.Public);
            }
            let filters = [filterPrivate].filter(x => x?.length > 0).join(",");

            return filters;
        },


        async resolveOnChangeConfig(config){
            try {
                if (! this.isMySelf){
                    return await this.resolveOnChangeConfigOnOtherUser();
                }

                // mySelf
                if (config === null || config === undefined){
                    config = {};
                }
                let { text } = config;
                
                // Get url
                let url = "Lessons/my";
                if (Validator.isNotEmpty(text)){
                    url = "Lessons/search/my";
                }
                
                // Get orderString
                let orders = this.getOrderString(config);
                // Get filterString
                let filters = this.getFilterString(config);
                let params = {
                    search: text,
                    order: orders,
                    filter: filters,
                }

                // create async callback function:
                let callback = async function(limit, offset){
                    let param = {
                        ...params,
                        limit: limit,
                        offset: offset,
                    };
                    let res = await new GetRequest(url).setParams(param).execute();
                    return res;
                }.bind(this);

                this.getPostCallback = callback;
            } catch (error){
                console.error(error);
            }
        },


        async resolveOnChangeConfigOnOtherUser(){
            try {
                let userId = this.getUser()?.UserId;
                if (userId == null) return;
                // let that = this;
                let url = "Users/lessons/" + userId;
                let currentUser = await CurrentUser.getInstance();
                if (currentUser == null) {
                    url = "User/anonymous/lessons/" + userId;
                }
                this.getPostCallback = async function(limit, offset){
                    let param = {
                        limit: limit,
                        offset: offset,
                    };
                    let res = await new GetRequest(url).setParams(param).execute();
                    return res;
                }
            }
            catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    }
}

</script>

<style>
.p-profile-main-subpage{
    max-width: var(--profile-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.profile-lesson-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.profile-lesson-subpage__left{
    width: 0;
    flex-shrink: 1;
    flex-grow: 2;

    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.profile-lesson-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 3;
}

</style>

