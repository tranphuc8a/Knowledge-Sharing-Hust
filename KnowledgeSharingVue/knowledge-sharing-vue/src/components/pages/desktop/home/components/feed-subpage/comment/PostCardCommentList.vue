<template>
    <div class="p-postcard-comment-list">
        <div class="p-pcl-filter-button">
            <CommentFilterButton :on-change="resolveOnchangeFilter" />
        </div>
        <div class="p-pcl-comment-list" v-if="listComments?.length > 0">
            <PostCardComment v-for="comment in listComments" :key="comment?.UserItemId" :comment="comment"/>
            <div class="p-pcl-comment-load-more">
                Tải thêm bình luận
            </div>
        </div>
        <div class="p-pcl-empty-comment" v-if="listComments?.length <= 0">
            Hiện không có bình luận nào
        </div>
        <div class="p-pcl-enter-comment">
            <PostCardEnterComment :useritem="getPost()" :on-comment-submitted="resolvePostedComment"/>
        </div>
    </div>
</template>

<script>
import PostCardComment from './PostCardComment.vue';
import PostCardEnterComment from './PostCardEnterComment.vue';
import CommentFilterButton from './CommentFilterButton.vue';
import { myEnum } from '@/js/resources/enum';
import CurrentUser from '@/js/models/entities/current-user';
import { Request, GetRequest } from '@/js/services/request';
import ResponseCommentModel from '@/js/models/api-response-models/response-comment-model';


export default {
    name: "PostCardcomment-list",
    data(){
        return {
            label: null,
            listComments: [],
            isOutOfComments: false,
            pageSize: 10,
            currentUser: null,
        }
    },
    components: {
        CommentFilterButton, PostCardEnterComment, PostCardComment
    },
    async mounted(){
        if (this.getPost()?.TopComments?.length > 0){
            this.listComments = this.getPost().TopComments.map(function(comment){
                let com = new ResponseCommentModel();
                com.copy(comment);
                return com;
            });
        }
        console.log(this.listComments);
        this.currentUser = await CurrentUser.getInstance();
        this.getLabel();
        this.getMoreComments();
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.subpages?.feedpage?.postcard;
            }
            return this.label;
        },


        async resolveOnchangeFilter(commentFilterType){
            try {
                switch (commentFilterType) {
                    case myEnum.commentFilterType.Best: 

                        break;
                    case myEnum.commentFilterType.Recent: 

                        break;
                    case myEnum.commentFilterType.All: 

                        break;
                    default:
                        break;
                }
            } catch (e) {
                console.error(e);
            }
        },


        async getMoreComments(){
            try {
                let numComs = this.listComments.length;
                let url = null;
                let postId = this.getPost()?.UserItemId;
                if (postId == null){
                    return;
                }
                if (this.currentUser == null){
                    url = 'Comments/anonymous/' + postId;
                } else {
                    url = 'Comments/' + postId;
                }
                let res = await new GetRequest(url)
                    .setParams({
                        limit: this.pageSize,
                        offset: numComs
                    }).execute();
                let body = await Request.tryGetBody(res);
                let readComments = body?.Results ?? [];
                if (readComments < this.pageSize)
                    this.isOutOfComments = true;
                let mappedComments = readComments.map(function(comment){
                    let com = new ResponseCommentModel();
                    com.copy(comment);
                    return com;
                });
                this.listComments = this.listComments.concat(mappedComments);
            } catch (e) {
                console.error(e);
            }
        },


        async resolvePostedComment(comment){
            try {
                this.listComments.push(comment);
            } catch (e) {
                console.error(e);
            }
        }

        
    },
    inject: {
        getLanguage: {},
        getPost: {
            default: null
        }
    }
}

</script>

<style scoped>

.p-pcl-comment-list{
    padding: 18px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 18px;
}

.p-pcl-empty-comment{
    width: 100%;
    text-align: center;
    padding: 12px 0px 18px 0px;
}

</style>
