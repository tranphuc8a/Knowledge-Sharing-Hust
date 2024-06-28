<template>
    <div class="p-postcard-comment-list" v-if="isLoaded && !isBlockComment">
        <div class="p-pcl-filter-button" v-show="true">
            <CommentFilterButton :on-change="resolveOnchangeFilter" :value="orderValue" />
        </div>
        <div class="p-pcl-comment-list" v-show="listCombinedComment?.length > 0">
            <PostCardComment 
                v-for="comment in listCombinedComment" 
                :key="comment?.UserItemId" 
                :comment="comment"
                :on-deleted-comment="resolveDeletedComment(comment)"
            />
            
            <MLinkButton v-show="!isOutOfComments" :onclick="getMoreComments" 
                label="Tải thêm bình luận" :href="null" :buttonStyle="buttonStyle"
            />

        </div>
        <div class="p-pcl-empty-comment" v-show="listCombinedComment?.length <= 0">
            Hiện không có bình luận nào
        </div>
        <div class="p-pcl-enter-comment">
            <PostCardEnterComment :useritem="getPost()" :on-comment-submitted="resolvePostedComment" ref="enter-comment"/>
        </div>
    </div>
    <div class="p-postcard-comment-list" v-if="isLoaded && isBlockComment">
        <div class="p-pcl-empty-comment">
            Bài viết đã bị khóa bình luận
        </div>
    </div>
    <div class="p-postcard-comment-list" v-if="!isLoaded">
        <div class="p-pcl-loading-comment">
            <MSpinner />
        </div>
    </div>
</template>

<script>
import MLinkButton from '@/components/base/buttons/MLinkButton';
import PostCardComment from './PostCardComment.vue';
import PostCardEnterComment from './PostCardEnterComment.vue';
import CommentFilterButton from './CommentFilterButton.vue';
import { myEnum } from '@/js/resources/enum';
import CurrentUser from '@/js/models/entities/current-user';
import { Request, GetRequest } from '@/js/services/request';
import ResponseCommentModel from '@/js/models/api-response-models/response-comment-model';
import { MyRandom } from '@/js/utils/myrandom';

export default {
    name: "PostCardcomment-list",
    data(){
        return {
            isLoaded: true,
            isBlockComment: false,
            buttonStyle: { padding: '0px', fontSize: '13px', height: '24px' },
            label: null,
            listComments: [],
            listPostedComment: [],
            listCombinedComment: [],
            isOutOfComments: false,
            pageSize: 6,
            currentUser: null,
            defaultOrder: '',
            order: '',
            orderValue: myEnum.commentFilterType.Best
        }
    },
    components: {
        MLinkButton,
        CommentFilterButton, PostCardEnterComment, PostCardComment
    },
    async mounted(){
        if (this.getPost()?.TopComments?.length > 0){
            this.listComments = this.getPost().TopComments.map(function(comment){
                let com = new ResponseCommentModel();
                com.copy(comment);
                return com;
            });
            this.listPostedComment = [];
            await this.updateListCombinedComments();
        }
        this.isBlockComment = this.getPost()?.IsBlockComment ?? false;
        // console.log(this.listComments);
        this.currentUser = await CurrentUser.getInstance();
        this.getLabel();
        this.getTopComments();
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


        randomize(){
            return MyRandom.generateUUID();
        },


        async resolveOnchangeFilter(commentFilterType){
            try {
                this.order = commentFilterType;
                this.orderValue = commentFilterType;
                this.listComments = [];
                this.listPostedComment = [];
                this.listCombinedComment = [];
                this.isOutOfComments = false;
                this.getTopComments();
            } catch (e) {
                console.error(e);
            }
        },


        async getTopComments(){
            try {
                this.listComments = [];
                this.listPostedComment = [];

                this.isLoaded = false;
                let url = null;
                let postId = this.getPost()?.UserItemId;
                if (postId == null){
                    return;
                }
                let orderString = "";
                if (this.order == myEnum.commentFilterType.Best){
                    // api for best comments
                    if (this.currentUser == null){
                        url = 'Comments/anonymous/' + postId;
                    } else {
                        url = 'Comments/' + postId;
                    }
                    orderString = 'TotalStar:desc,AverageStar:desc,TotalComment:desc';
                } else {
                    // default api for top comments
                    if (this.currentUser == null){
                        url = 'Comments/anonymous/' + postId;
                    } else {
                        url = 'Comments/' + postId;
                    }
                    orderString = '';
                }

                let pageSize = 3;
                let res = await new GetRequest(url)
                    .setParams({
                        limit: pageSize,
                        offset: 0,
                        order: orderString
                    }).execute();
                let body = await Request.tryGetBody(res);
                let readComments = body?.Results ?? [];
                if (readComments < pageSize)
                    this.isOutOfComments = true;
                let mappedComments = readComments.map(function(comment){
                    let com = new ResponseCommentModel();
                    com.copy(comment);
                    return com;
                });
                this.listComments = mappedComments;
                await this.updateListCombinedComments();
            } catch (e) {
                console.error(e);
            } finally {
                this.isLoaded = true;
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
                let orderString = "";
                if (this.order == myEnum.commentFilterType.Best){
                    // api for best comments
                    if (this.currentUser == null){
                        url = 'Comments/anonymous/' + postId;
                    } else {
                        url = 'Comments/' + postId;
                    }
                    orderString = 'TotalStar:desc,AverageStar:desc,TotalComment:desc';
                } else {
                    // default api for top comments
                    if (this.currentUser == null){
                        url = 'Comments/anonymous/' + postId;
                    } else {
                        url = 'Comments/' + postId;
                    }
                    orderString = '';
                }
                
                let res = await new GetRequest(url)
                    .setParams({
                        limit: this.pageSize,
                        offset: numComs,
                        order: orderString
                    }).execute();
                let body = await Request.tryGetBody(res);
                let readComments = body?.Results ?? [];
                if (readComments.length < this.pageSize)
                    this.isOutOfComments = true;
                let mappedComments = readComments.map(function(comment){
                    let com = new ResponseCommentModel();
                    com.copy(comment);
                    return com;
                });
                let listCommentIds = this.listComments.map(function(comment){
                    return comment.UserItemId;
                });
                let newComments = mappedComments.filter(function(comment){
                    return listCommentIds.indexOf(comment.UserItemId) < 0;
                });
                this.listComments = this.listComments.concat(newComments);
                await this.updateListCombinedComments();
            } catch (e) {
                console.error(e);
                Request.resolveAxiosError(e);
            }
        },


        async resolvePostedComment(comment){
            try {
                if (comment == null){
                    return;
                }
                this.listPostedComment.push(comment);
                await this.updateListCombinedComments();
            } catch (e) {
                console.error(e);
            }
        },

        resolveDeletedComment(comment){
            let that = this;
            return async function(){
                try {
                    that.listComments = that.listComments.filter(function(com){
                        return com.UserItemId != comment.UserItemId;
                    });
                    that.listPostedComment = that.listPostedComment.filter(function(com){
                        return com.UserItemId != comment.UserItemId;
                    });
                    await that.updateListCombinedComments();
                } catch (e) {
                    console.error(e);
                }
            }
        },

        async focus(){
            try {
                await this.$refs['enter-comment']?.focus?.();
            } catch (e) {
                console.error(e);
            }
        },


        async updateListCombinedComments(){
            try {
                let listCommendIds = this.listComments.map(function(comment){
                    return comment.UserItemId;
                });
                this.listPostedComment = this.listPostedComment.filter(function(comment){
                    return listCommendIds.indexOf(comment.UserItemId) < 0;
                });
                this.listCombinedComment = this.listComments.concat(this.listPostedComment);
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
    gap: 12px;
}

.p-pcl-empty-comment{
    width: 100%;
    text-align: center;
    padding: 12px 0px 18px 0px;
    color: var(--grey-color);
    font-family: 'ks-font-regular';
}

.p-pcl-loading-comment{
    width: 100%;
    text-align: center;
    padding: 12px 0px 18px 0px;
}

.p-pcl-comment-load-more{
    cursor: pointer;
}

.p-pcl-comment-load-more:hover{
    text-decoration: underline;
}

</style>
