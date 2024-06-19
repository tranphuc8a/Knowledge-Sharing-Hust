

<template>
    <div class="p-feed-subpage" v-if="!isLoaded">
        <!-- <AddPostFeedCard v-if="isShowAddPost"/> -->

        <slot name="addpost"></slot>

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


    <div class="p-feed-subpage" v-if="isLoaded"
        ref="list"
        >
        <slot name="addpost"></slot>

        <component v-for="item in listPosts" 
            :key="item?.UserItemId"
            :is="getComponent(item)"
            v-bind="{post: item, isViewComment: isViewComment}">
            Hello
        </component>

        <div class="p-feed-notfound" v-show="isOutOfPost && !(listPosts.length > 0)">
            <NotFoundPanel text="Không tìm thấy mục nào" />
        </div>

        <div style="padding: 16px; display: index; flex-flow: column nowrap; width: 100%"
            class="card" v-if="!isOutOfPost">
            <div class="skeleton" style="width: 30%; height: 20px; margin-bottom: 18px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 50%; height: 20px; margin-bottom: 10px;"></div>
        </div>

        <div style="padding: 16px; display: index; flex-flow: column nowrap; width: 100%"
            class="card" v-if="!isOutOfPost">
            <div class="skeleton" style="width: 30%; height: 20px; margin-bottom: 18px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
            <div class="skeleton" style="width: 50%; height: 20px; margin-bottom: 10px;"></div>
        </div>

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
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
// import AddPostFeedCard from '../components/feed-subpage/postcard/AddPostFeedCard.vue';
import LessonFeedCard from '../components/feed-subpage/postcard/LessonFeedCard.vue';
import QuestionFeedCard from '../components/feed-subpage/postcard/QuestionFeedCard.vue';

import { myEnum } from '@/js/resources/enum';
import CurrentUser from '@/js/models/entities/current-user';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';
import { Request } from '@/js/services/request';
import ResponseQuestionModel from '@/js/models/api-response-models/response-question-model';

export default {
    name: 'PostSubpage',
    components: {
        // AddPostFeedCard,
        LessonFeedCard,
        QuestionFeedCard,
        NotFoundPanel,
    },
    props: {
        getPost: {
            required: async function(limit, offset){
                console.log("Get post at " + limit + " " + offset);
                return [];
            }
        },
        // prop xac dinh context cua postsubpage hien tai (null, user, course)
        owner: {
            default: null,
        },
        // isShowAddPost: {
        //     type: Boolean,
        //     default: true,
        // },
        isViewComment: {
            default: true,
        }
    },
    data(){
        return {
            isLoaded: false,
            isOutOfPost: false,
            isLoadingMore: false,
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
                let averagePostHeight = 700;
                let leftPostNumber = 5;

                if (scrollHeight - scrollPosition < averagePostHeight * leftPostNumber){
                    // console.log("Load more post");
                    this.loadMorePost();
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
                
                // get more post
                let offset = this.listPosts?.length ?? 0;
                let limit = 10;
                if (this.getPost == null) return;
                let res = await this.getPost(limit, offset);
                
                let body = await Request.tryGetBody(res);
                if (!(body?.length > 0) && (body?.Results?.length > 0)){
                    body = body.Results;
                }
                let tempListPosts = body.map(function(post){
                    if (post.PostType == myEnum.EPostType.Lesson){
                        return new ResponseLessonModel().copy(post);
                    }
                    return new ResponseQuestionModel().copy(post);
                });
                if (tempListPosts.length < limit){
                    this.isOutOfPost = true;
                    console.log("Out of posts");
                } 
                
                if (tempListPosts.length > 0){
                    this.listPosts = this.listPosts.concat(tempListPosts);
                }

            } catch (e){
                Request.resolveAxiosError(e);
                this.isOutOfPost = true;
            } finally {
                this.isLoadingMore = false;
            }
        },

        async refresh(){
            try {
                this.isLoaded = false;
                this.isOutOfPost = false;
                this.currentUser = await CurrentUser.getInstance();
                this.listPosts = [];
                if (this.getPost == null) return;
                await this.loadMorePost();
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        registerScrollHandler: {},
    },
    watch: {
        getPost(){
            this.refresh();
        }
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

.p-feed-notfound{
    width: 100%;
    height: 200px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}



</style>

