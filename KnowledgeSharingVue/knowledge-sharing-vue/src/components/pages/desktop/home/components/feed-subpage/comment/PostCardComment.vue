<template>
    <div class="p-comment">
        <div class="p-comment-left">
            <div :class="{'p-comment-avatar': true, 'p-comment-owner': isOwner()}">
                <TooltipUserAvatar :user="comment?.User"/>
            </div>
        </div>
        

        <div class="p-comment-content">
            <div class="p-my-comment">
                <div class="p-comment-card">
                    <div class="p-comment-frame">
                        <div class="p-comment-username">
                            <TooltipUsername :user="comment?.User"/>
                        </div>
                        <div class="p-comment-text">
                            <span v-show="!isCollapsing">
                                {{comment?.Content ?? "Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luậnĐây là bình luậnĐây là bình luận Đây là bình luận"}}
                            </span> 
                            <span v-show="isCollapsing">
                                {{ collapseText(
                                    comment?.Content ?? "Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luận Đây là bình luậnĐây là bình luậnĐây là bình luận Đây là bình luận"
                                ) }}
                            </span>
                            <span> &nbsp; </span>
                            <span class="p-more-button" 
                                v-show="!isCollapsing && true"
                                @:click="toggleCollapse">Ẩn bớt</span>
                            <span class="p-more-button" 
                                v-show="isCollapsing"
                                @:click="toggleCollapse">Xem thêm</span>
                        </div>
                    </div>
                    <div class="p-comment-menu-context">
                        <CommentMenuContext 
                            :on-edit="resolveEditComment"
                            :on-delete="resolveDeleteComment"
                            :on-reply="resolveReplyComment"
                        />
                    </div>
                </div>
                <div class="p-comment-info">
                    <CommentInformationBar :on-reply="resolveReplyComment" />
                </div>
            </div>
            <div class="p-comment-replies" v-if="comment?.NumberReplies > 0">
                <PostCardComment v-for="(comment) in comment?.Replies ?? []" :key="comment?.UserItemId" :comment="comment"/>
            </div>
            <div class="p-reply-enter" v-if="isShowEnterComment && comment?.ReplyId == null">
                <PostCardEnterComment :useritem="comment"/>
            </div>
        </div>
    </div>

</template>

<script>
import CommentInformationBar from './CommentInformationBar.vue';
import CommentMenuContext from './CommentMenuContext.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import PostCardEnterComment from './PostCardEnterComment.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import { Validator } from '@/js/utils/validator';
export default {
    name: "p-comment",
    data() {
        return {
            isShowEnterComment: false,
            isShowReplies: false,
            isCollapsing: true,
            maxCommentLength: 50,
        }
    },
    components: {
        CommentInformationBar,
        CommentMenuContext,
        TooltipUserAvatar,
        PostCardEnterComment,
        TooltipUsername
    },
    methods: {
        isOwner(){
            let postOwner = this.post?.UserId;
            let commentOwner = this.comment?.UserId;
            return Validator.isNotEmpty(commentOwner) && postOwner === commentOwner;
        },

        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.inject?.language != null){
                this.label = this.inject?.language?.subpages?.feedpage?.postcard;
            }
            console.log(this.inject);
            return this.label;
        },


        collapseText(text){
            try {
                text = String(text);
                if (text.length <= this.maxCommentLength) {
                    return text;
                }
                return text.slice(0, this.maxCommentLength) + '...';
            } catch (error){
                console.error(error);
                return "Error";
            }
        },

        toggleCollapse(){
            this.isCollapsing = !this.isCollapsing;
        },

        async resolveReplyComment(){
            this.isShowEnterComment = true;
        }
    },
    props: {
        comment: {}
    },
    mounted() {
        this.listComments = this.comments;
    },
    inject: {
        inject: {},
        post: {}
    },
    provider(){
        return {
            comment: this.comment
        }
    }
}

</script>

<style scoped>

.p-comment,
.p-comment-card{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-comment-left{
    flex-shrink: 0;
    flex-grow: 0;
    width: 44px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: flex-start;
    box-sizing: border-box;
}

.p-comment-avatar{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    border-radius: 100%;
    box-sizing: border-box;
}
.p-comment-owner{
    width: 44px;
    height: 44px;
    border: 3px solid var(--primary-color);
}

.p-comment-content{
    flex: 1;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-comment-card{
    gap: 4px;
}

.p-comment-frame{
    width: fit-content;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    
    background-color: var(--primary-color-100);
    border-radius: 22px;
    padding: 8px 12px;;
}

.p-comment-text{
    text-align: justify;
}

.p-more-button{
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-more-button:hover{
    text-decoration: underline;
}

.p-my-comment{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-reply-enter{
    margin-top: 8px;
    width: 100%;
}

</style>