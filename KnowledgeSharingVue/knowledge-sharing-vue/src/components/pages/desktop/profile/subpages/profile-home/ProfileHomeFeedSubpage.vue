

<template>
    <div class="p-profile-subpage p-feed-subpage" v-if="!isLoaded">
        <AddPostFeedCard v-if="isMySelf"/>
        <div v-for="item in [1, 2, 3]" :key="item" style="width: 100%">
            <div style="padding: 16px; display: index; flex-flow: column nowrap; width: 100%"
                class="card"
            >
                <div class="skeleton" style="width: 30%; height: 20px; margin-bottom: 18px;"></div>
                <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
                <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
                <div class="skeleton" style="width: 50%; height: 20px; margin-bottom: 10px;"></div>
            </div>
        </div>
    </div>


    <div class="p-profile-subpage p-feed-subpage" v-if="isLoaded"
        ref="list"
        >
        <AddPostFeedCard v-if="isMySelf" />

        <component v-for="item in listPosts" 
            :key="item?.UserItemId"
            :is="getComponent(item)"
            v-bind="{post: item}">
            Hello
        </component>

        <div style="padding: 16px; display: index; flex-flow: column nowrap; width: 100%"
            class="card" v-if="!isOutOfPost">
            <div class="skeleton" style="width: 30%; height: 20px; margin-bottom: 18px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 50%; height: 20px; margin-bottom: 10px;"></div>
        </div>
    </div>

    
</template>



<script>
import AddPostFeedCard from '../../../home/components/feed-subpage/postcard/AddPostFeedCard.vue';
import LessonFeedCard from '../../../home/components/feed-subpage/postcard/LessonFeedCard.vue';
import QuestionFeedCard from '../../../home/components/feed-subpage/postcard/QuestionFeedCard.vue';

import { myEnum } from '@/js/resources/enum';
import CurrentUser from '@/js/models/entities/current-user';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';
import { GetRequest, Request } from '@/js/services/request';
import ResponseQuestionModel from '@/js/models/api-response-models/response-question-model';

export default {
    name: 'ProfileHomeFeedSubpage',
    components: {
        AddPostFeedCard,
        LessonFeedCard,
        QuestionFeedCard,
    },
    props: {
    },
    data(){
        return {
            isLoaded: false,
            isOutOfPost: false,
            isLoadingMore: false,
            isMySelf: false,
            listPosts: [],
            currentUser: null,
        }
    },
    created(){
        this.refresh();
    },
    mounted(){
        this.registerScrollHandler(this.resolveOnScroll);
    },
    methods: {
        getComponent(item){
            if (item?.PostType ==  myEnum.EPostType.Question){
                return QuestionFeedCard;
            }
            return LessonFeedCard;
        },

        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;
                if (this.isOutOfPost || this.isLoadingMore){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averagePostHeight = 800;
                let leftPostNumber = 2;

                if (scrollHeight - scrollPosition < averagePostHeight * leftPostNumber){
                    console.log("Load more post");
                    await this.loadMorePost();
                }
            } catch (e){
                console.error(e);
            }
        },

        async loadMorePost(){
            if (this.isOutOfPost || this.isLoadingMore){
                return;
            }
            try {
                this.isLoadingMore = true;
                let userId = this.getUser()?.UserId;
                if (userId == null){
                    return;
                }
                
                // get more post
                let offset = this.listPosts?.length ?? 0;
                let url = 'Users/posts/' + userId;
                if (this.currentUser == null){
                    url = 'Users/anonymous/posts/' + userId;
                } else if (await this.getIsMySelf()){
                    url = 'Posts/my';
                }
                let res = await new GetRequest(url)
                    .setParams({
                        offset: offset,
                        limit: 10,
                    })
                    .execute();
                let body = await Request.tryGetBody(res);
                let tempListPosts = body.map(function(post){
                    if (post.PostType == myEnum.EPostType.Lesson){
                        return new ResponseLessonModel().copy(post);
                    }
                    return new ResponseQuestionModel().copy(post);
                });
                if (tempListPosts.length <= 0){
                    this.isOutOfPost = true;
                    console.log("Out of posts");
                } else {
                    this.listPosts = this.listPosts.concat(tempListPosts);
                }

            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isLoadingMore = false;
            }
        },

        async refresh(){
            try {
                console.log("Refresh");
                this.isLoaded = false;
                this.isMySelf = await this.getIsMySelf();
                this.currentUser = await CurrentUser.getInstance();
                this.listPosts = [];
                await this.loadMorePost();
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
        getIsMySelf: {},
        registerScrollHandler: {},
    }
}

</script>

<style scoped>

.p-feed-subpage{
    width: 100%;
    gap: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
}



</style>

