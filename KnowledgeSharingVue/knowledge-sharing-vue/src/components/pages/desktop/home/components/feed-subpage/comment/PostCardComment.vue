<template>
    <div class="p-comment" v-if="isShowComment">
        <div class="p-comment-line" v-show="!isEditing">
            <div class="p-comment-left">
                <div :class="{'p-comment-avatar': true, 'p-comment-owner': isOwner()}">
                    <TooltipUserAvatar :user="dComment?.User"/>
                </div>
            </div>

            <div class="p-comment-right">
                <div class="p-my-comment">
                    <div class="p-comment-card" ref="comment-card">
                        <div class="p-comment-frame">
                            <div class="p-comment-username">
                                <TooltipUsername :user="dComment?.User"/>
                            </div>
                            <div class="p-comment-text">
                                <LimitLengthText :text="dComment?.Content"
                                    :length="50" />
                            </div>
                        </div>
                        <div class="p-comment-menu-context" ref="comment-menu-context">
                            <CommentMenuContext 
                                :on-edit="resolveEditComment"
                                :on-delete="resolveDeleteComment"
                                :on-reply="resolveReplyComment"
                                :on-toggle-information="resolveToggleInformation"
                            />
                        </div>
                    </div>
                    <div class="p-comment-info" v-if="dComment?.isHideCommentInformation === false">
                        <CommentInformationBar :on-reply="resolveReplyComment" />
                    </div>
                </div>
            </div>
        </div>

        <div class="p-comment-line p-edit-comment" v-if="isEditing">
            <PostCardEnterComment ref="edit-comment" 
                :is-editing="true"
                :edit-for="this.comment.UserItemId"
                :useritem="comment" 
                :value="dComment?.Content ?? 'Chinh sua binh luan'"
                :on-comment-submitted="resolveSubmittedComment"
                />

            <div class="p-cancel-edit-comment">
                <span @:click="resolveCancelEditComment">
                    Hủy chỉnh sửa
                </span>
            </div>
        </div>

        <div class="p-comment-line">
            <div class="p-comment-left">
                
            </div>

            <div class="p-comment-right">
                <div class="p-comment-replies" v-if="dComment?.TotalReplies > 0">
                    <PostCardComment 
                        v-for="(cmt, index) in listReplies" 
                        :key="cmt?.UserItemId ?? index" 
                        :comment="cmt"
                    />
                </div>
                <div class="p-reply-enter" v-if="isShowEnterComment && dComment?.ReplyId == null">
                    <PostCardEnterComment ref="reply-enter" :useritem="comment" placeholder="Phản hồi bình luận"/>
                </div>
                <MLinkButton v-show="!isOutOfReplies && (listReplies?.length <= 0)" 
                    :onclick="getMoreReplies" 
                    :label="`Có ${dComment.TotalReplies ?? 0} phản hồi`" :href="null"
                    :buttonStyle="buttonStyle"
                />
                <MLinkButton v-show="!isOutOfReplies && (listReplies?.length > 0)" :onclick="getMoreReplies" 
                    label="Tải thêm phản hồi" :href="null"
                    :buttonStyle="buttonStyle"
                />
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
import ResponseCommentModel from '@/js/models/api-response-models/response-comment-model';
import { GetRequest, Request } from '@/js/services/request';
import MLinkButton from './../../../../../../base/buttons/MLinkButton.vue'


export default {
    name: "p-comment",
    data() {
        return {
            buttonStyle: { padding: '0px', fontSize: '13px', height: '24px' },
            isShowComment: true,
            isOutOfReplies: false,
            dComment: this.comment,
            isShowEnterComment: false,
            isShowReplies: false,
            isCollapsing: true,
            isEditing: false,
            components: {
                commentCard: null,
                commentMenuContext: null,
                replyEnter: null
            },
            listReplies: [],
            isLoadingMoreReplies: false,
        }
    },
    components: {
        LimitLengthText,
        CommentInformationBar,
        CommentMenuContext,
        TooltipUserAvatar,
        PostCardEnterComment,
        TooltipUsername, MLinkButton
    },
    mounted(){
        this.refreshComment();
    },
    methods: {
        isOwner(){
            let postOwner = this.getPost()?.UserId;
            let commentOwner = this.dComment?.UserId;
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
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.subpages?.feedpage?.postcard;
            }
            return this.label;
        },

        async forceRender(){
            let that = this;
            that.isShowComment = false;
            that.$nextTick(() => {
                that.isShowComment = true;
            });
        },

        async refreshComment(){
            try {
                let tempComment = this.comment;
                if (tempComment == null) tempComment = {};
                tempComment.isHideCommentInformation = false;
                tempComment.ForceUpdate = this.forceRender.bind(this);

                this.components = {
                    commentCard: this.$refs['comment-card'],
                    commentMenuContext: this.$refs['comment-menu-context'],
                    replyEnter: this.$refs['reply-enter']
                };

                this.dComment = tempComment;
                this.isOutOfReplies = ! (this.dComment?.TotalReplies > 0); 
                this.getLabel();
            } catch (error) {
                console.error(error);
            }
        },

        async resolveReplyComment(){
            try {
                this.isShowEnterComment = !this.isShowEnterComment;
                let that = this;
                this.$nextTick(() => {
                    try {
                        that.$refs['reply-enter']?.focus?.();
                    } catch (e) {
                        console.error(e);
                    }
                });
            } catch (error) {
                console.error(error);
            }
        },

        async resolveEditComment(){
            this.isEditing = true;
        },

        async resolveCancelEditComment(){
            this.isEditing = false;
        },

        async resolveSubmittedComment(text){
            try {
                this.isEditing = false;
                this.dComment.Content = text;
            }
            catch (error) {
                console.error(error);
            }
        },

        async resolveDeleteComment(){
            this.$emit('on-delete', this.dComment);
        },

        async resolveToggleInformation(){
            try {
                this.dComment.isHideCommentInformation = !this.dComment.isHideCommentInformation;
            } catch (error){
                console.error(error);
            }
        },

        async getMoreReplies(){
            if (this.isOutOfReplies || this.isLoadingMoreReplies)
                return;
            try {
                this.isLoadingMoreReplies = true;
                let offset = this.listReplies.length;
                let limit = 10;
                let res = await new GetRequest('Comments/replies/' + this.comment.UserItemId)
                    .setParams({
                        offset: offset,
                        limit: limit
                    })
                    .execute();
                let body = await Request.tryGetBody(res);
                if (body.Count < limit || body.Count <= 0){
                    this.isOutOfReplies = true;
                }
                let results = body.Results;
                let tempComment = results.map(function(com){
                    return new ResponseCommentModel().copy(com);
                });
                
                this.listReplies = this.listReplies.concat(tempComment);
            } catch (error){
                Request.resolveAxiosError(error);
            } finally {
                this.isLoadingMoreReplies = false;
            }
        }

    },
    props: {
        comment: {
            required: true
        },

        onPostedComment: {
            default: null
        }
    },
    watch: {
        comment(){
            this.refreshComment();
        }
    },
    inject: {
        getLanguage: {},
        getPost: {}
    },
    provide(){
        return {
            getComment: () => this.dComment,
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
    flex-flow: column nowrap;
    gap: 8px;
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

.p-comment-right{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
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

.p-comment-line{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-comment-line.p-edit-comment{
    flex-flow: column nowrap;
}
.p-cancel-edit-comment{
    width: 100%;
    text-align: left;
    font-size: 12px;
    color: var(--primary-color);
    padding-left: 40px;
}
.p-cancel-edit-comment span{
    cursor: pointer;
}
.p-cancel-edit-comment span:hover{
    text-decoration: underline;
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
    visibility: hidden;

    align-self: stretch;
}

.p-comment-text{
    height: fit-content;
    margin: 0;
    padding: 0;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    font-size: 14px;
}


.p-my-comment{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-my-comment:hover .p-comment-menu-context{
    visibility: visible;
}

.p-reply-enter{
    margin-top: 8px;
    width: 100%;
}

</style>