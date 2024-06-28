

<template>
    <div class="p-administrator-list-question-content">
        <div class="p-alqc-result-card card">
            <div class="p-alqc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-alqc-result">
                <div class="p-alqc-result-notfound" v-if="isOutOfPost && listPost.length == 0" >
                    <NotFoundPanel 
                        text="Không tìm thấy bài thảo luận nào" 
                    />
                </div>
                <div class="p-alqc-result-list">
                    <PostAdministatorShortCard v-for="post in listPost"
                        :key="post.UserItemId"
                        :post="post"
                    /> 

                    <PostShortCardSkeleton v-if="!isOutOfPost" />
                    <PostShortCardSkeleton v-if="!isOutOfPost" />
                    <PostShortCardSkeleton v-if="!isOutOfPost" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import PostAdministatorShortCard from '../../components/administrator/admin-useritem/PostAdministatorShortCard.vue';
import PostShortCardSkeleton from '@/components/base/cards/PostShortCardSkeleton.vue';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import ViewQuestion from '@/js/models/views/view-question';


export default {
    name: 'AdministratorListQuestionContent',
    components: {
        NotFoundPanel,
        PostAdministatorShortCard,
        PostShortCardSkeleton,
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            listPost: [],
            searchKey: '',
            isLoaded: false,
            isOutOfPost: false,
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
                if (this.isOutOfPost || this.isWorking){
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
                    this.loadMorePost();
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
                this.listPost = [];
                this.isOutOfPost = false;
                this.loadMorePost();
            } catch (e){
                console.error(e);
            } finally {
                this.isLoaded = true;
            }
        },

        async loadMorePost(){
            if (this.isWorking || this.isOutOfPost) return;
            try {
                this.isWorking = true;

                // prepare request
                let url = 'Questions/search';
                if (Validator.isEmpty(this.searchKey)){
                    this.listPost = [];
                    this.isOutOfPost = true;
                    return;
                }
                let limit = 20;
                let offset = this.listPost.length;

                // call request
                let res = await new GetRequest(url)
                    .setParams({search: this.searchKey, limit: limit, offset: offset})
                    .execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // read data:
                if (body.length < limit){
                    this.isOutOfPost = true;
                }
                if (body.length > 0){
                    let tempPosts = body.map(function(post){
                        return new ViewQuestion().copy(post);
                    });
                    this.listPost = this.listPost.concat(tempPosts);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveOnDeletedQuestion(postId){
            try {
                let index = this.listPost.findIndex(function(post){
                    return post.UserItemId == postId;
                });
                if (index >= 0){
                    this.listPost.splice(index, 1);
                }
            } catch (e){
                console.error(e);
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
        return {
            onPostDeleted: this.resolveOnDeletedQuestion
        }
    }
}

</script>


<style scoped>

.p-administrator-list-question-content{
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

.p-alqc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-alqc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-alqc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-alqc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-alqc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

