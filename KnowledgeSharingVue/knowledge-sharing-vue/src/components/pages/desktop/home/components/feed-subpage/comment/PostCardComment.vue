<template>
    <div class="p-comment">
        <div class="p-comment-left">
            <div :class="{'p-comment-avatar': true, 'p-comment-owner': isOwner()}">
                <TooltipUserAvatar :user="comment?.User"/>
            </div>
        </div>
        

        <div class="p-comment-content">
            <div class="p-my-comment">
                <div class="p-comment-card" ref="comment-card">
                    <div class="p-comment-frame">
                        <div class="p-comment-username">
                            <TooltipUsername :user="comment?.User"/>
                        </div>
                        <div class="p-comment-text">
                            <LimitLengthText :text="comment?.Content" 
                                :lenth="50" :on-collapse="adjustMenuContextHeight"/>
                        </div>
                    </div>
                    <div class="p-comment-menu-context" ref="comment-menu-context">
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
                <PostCardEnterComment :useritem="comment" placeholder="Phản hồi bình luận"/>
            </div>
        </div>
    </div>

</template>

<script>
import LimitLengthText from '@/components/base/text/LimitLengthText.vue';
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
            components: {
                commentCard: null,
                commentMenuContext: null
            }
        }
    },
    components: {
        LimitLengthText,
        CommentInformationBar,
        CommentMenuContext,
        TooltipUserAvatar,
        PostCardEnterComment,
        TooltipUsername
    },
    mounted(){
        this.components = {
            commentCard: this.$refs['comment-card'],
            commentMenuContext: this.$refs['comment-menu-context']
        };
        this.listComments = this.comments;
        this.getLabel();
        this.adjustMenuContextHeight();
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
            return this.label;
        },


        async adjustMenuContextHeight(){
            let that = this;
            this.$nextTick(async function(){
                try {
                    that.components.commentMenuContext.style.height = 'auto';
                    that.components.commentMenuContext.style.height 
                        = that.components.commentCard.clientHeight + 'px';
                } catch (error) {
                    console.error(error);
                }
            })
        },

        async resolveReplyComment(){
            this.isShowEnterComment = !this.isShowEnterComment;
        },

        async resolveEditComment(){
            this.$emit('on-edit', this.comment);
        },

        async resolveDeleteComment(){
            this.$emit('on-delete', this.comment);
        }
    },
    props: {
        comment: {
            required: true
        }
    },
    inject: {
        inject: {},
        post: {}
    },
    provide(){
        return {
            commentProvider: this.comment
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

.p-comment{
    width: 100%;
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

.p-comment-info{
    width: calc(100% - 40px);
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

.p-comment-menu-context{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-comment-text{
    height: fit-content;
    margin: 0;
    padding: 0;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
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