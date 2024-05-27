<template>
    <div class="p-postcard-comment-list" v-if="isLoaded">
        <div class="p-pcl-filter-button" v-show="false">
            <CommentFilterButton :on-change="resolveOnchangeFilter" />
        </div>
        <div class="p-pcl-comment-list" v-show="listComments?.length > 0">
            <PostCardComment 
                v-for="comment in listComments" 
                :key="comment?.UserItemId" 
                :comment="comment"
                :on-deleted-comment="resolveDeletedComment(comment)"
            />
            
            <MLinkButton v-show="!isOutOfComments" :onclick="getMoreComments" 
                label="Tải thêm bình luận" :href="null" :buttonStyle="buttonStyle"
            />

        </div>
        <div class="p-pcl-empty-comment" v-show="listComments?.length <= 0">
            Hiện không có bình luận nào
        </div>
        <div class="p-pcl-enter-comment">
            <PostCardEnterComment :useritem="getPost()" :on-comment-submitted="resolvePostedComment" ref="enter-comment"/>
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
            buttonStyle: { padding: '0px', fontSize: '13px', height: '24px' },
            label: null,
            listComments: [],
            isOutOfComments: false,
            pageSize: 10,
            currentUser: null,
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
        }
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


        async getTopComments(){
            try {
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
                        offset: 0
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
                this.listComments = mappedComments;
            } catch (e) {
                console.error(e);
            } finally {
                // let that = this;
                // this.isLoaded = false;
                // this.$nextTick(() => {
                //     that.isLoaded = true;
                // });
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
                // this.listComments = this.listComments.concat(mappedComments);
                // this.$forceUpdate();
            } catch (e) {
                console.error(e);
                Request.resolveAxiosError(e);
            }
        },


        async resolvePostedComment(comment){
            try {
                this.listComments.push(comment);
            } catch (e) {
                console.error(e);
            }
        },

        resolveDeletedComment(comment){
            let that = this;
            return async function(){
                try {
                    let index = that.listComments.indexOf(comment);
                    if (index >= 0){
                        that.listComments.splice(index, 1);
                    }
                } catch (e) {
                    console.error(e);
                }
            }
        },

        async focus(){
            try {
                await this.$refs['enter-comment'].focus();
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

.p-pcl-comment-load-more{
    cursor: pointer;
}

.p-pcl-comment-load-more:hover{
    text-decoration: underline;
}

</style>
