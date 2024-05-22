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
                                    :length="250" />
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

        <div class="p-comment-line p-edit-comment" v-show="isEditing">
            <PostCardEnterComment ref="edit-comment" 
                :is-editing="true"
                :edit-for="dComment?.UserItemId"
                :useritem="dComment" 
                :value="dComment?.Content ?? 'Chinh sua binh luan'"
                :on-comment-submitted="resolveCommentEdited"
                />

            <div class="p-cancel-edit-comment">
                <span @:click="resolveCancelEditComment">
                    Hủy chỉnh sửa
                </span>
            </div>
        </div>

        <div class="p-comment-line" v-show="listReplies.length > 0">
            <div class="p-comment-left"></div>
            <div class="p-comment-right">
                <div class="p-comment-replies">
                    <PostCardComment 
                        v-for="(cmt, index) in listReplies" 
                        :key="cmt?.UserItemId" 
                        :comment="cmt"
                        :on-deleted-comment="resolveDeletedReply(index)"
                    />
                </div>
            </div>
        </div>

        <div class="p-comment-line" v-show="!isOutOfReplies">
            <div class="p-comment-left"></div>
            <div class="p-comment-right">
                <MLinkButton v-show="listReplies?.length <= 0" 
                    :onclick="getMoreReplies" 
                    :label="`Có ${dComment?.TotalComment ?? 0} phản hồi`" :href="null"
                    :buttonStyle="buttonStyle"
                />
                <MLinkButton v-show="listReplies?.length > 0" :onclick="getMoreReplies" 
                    label="Tải thêm phản hồi" :href="null"
                    :buttonStyle="buttonStyle"
                />
            </div>
        </div>

        <div class="p-comment-line" v-show="isShowEnterComment && dComment?.ReplyId == null">
            <div class="p-comment-left"></div>
            <div class="p-comment-right">
                <div class="p-reply-enter">
                    <PostCardEnterComment ref="reply-enter" 
                        :useritem="comment" 
                        :on-comment-submitted="resolveSubmittedReplies"
                        placeholder="Phản hồi bình luận"/>
                </div>
            </div>
        </div>

    </div>

</template>

<script>
import PostCardComment from './PostCardComment.vue';
import LimitLengthText from '@/components/base/text/LimitLengthText.vue';
import CommentInformationBar from './CommentInformationBar.vue';
import CommentMenuContext from './CommentMenuContext.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import PostCardEnterComment from './PostCardEnterComment.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import { Validator } from '@/js/utils/validator';
import ResponseCommentModel from '@/js/models/api-response-models/response-comment-model';
import { GetRequest, DeleteRequest, Request } from '@/js/services/request';
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
        PostCardComment,
        LimitLengthText,
        CommentInformationBar,
        CommentMenuContext,
        TooltipUserAvatar,
        PostCardEnterComment,
        TooltipUsername, MLinkButton
    },
    mounted(){
        try {
            this.refreshComment();
        } catch (error){
            console.error(error);
        }
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
                let tempComment = new ResponseCommentModel().copy(this.comment);
                if (tempComment?.registerObserver){
                    tempComment.registerObserver(this.refreshComment.bind(this));
                }
                
                if (tempComment == null) tempComment = {};
                tempComment.isHideCommentInformation = false;
                tempComment.ForceUpdate = this.forceRender.bind(this);

                this.components = {
                    commentCard: this.$refs['comment-card'],
                    commentMenuContext: this.$refs['comment-menu-context'],
                    replyEnter: this.$refs['reply-enter']
                };

                this.dComment = tempComment;
                this.isOutOfReplies = ! (this.dComment?.TotalComment > 0); 
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
            try {
                this.isEditing = true;
                await this.$refs['edit-comment'].setValue(this.dComment.Content);
                this.$refs['edit-comment'].focus();
            } catch (er){
                console.error(er);
            }
        },

        async resolveCancelEditComment(){
            this.isEditing = false;
        },

        async resolveCommentEdited(text){
            try {
                this.isEditing = false;
                this.dComment.Content = text;
            }
            catch (error) {
                console.error(error);
            }
        },

        async resolveDeleteComment(){
            try {
                let that = this;
                let submitDeleteComment = async function(){
                    try {
                        await new DeleteRequest('Comments/' + that.dComment.UserItemId)
                            .execute();
                        that.getToastManager()?.success('Xóa bình luận thành công');
                        that.onDeletedComment?.();
                    } catch (error) {
                        Request.resolveAxiosError(error);
                    }
                }
                this.getPopupManager()?.inform('Bạn có chắc chắn muốn xóa bình luận này?', submitDeleteComment);
            } catch (error) {
                console.error(error);
            }
        },

        resolveDeletedReply(index){
            let that = this;
            return async function(){    
                try {
                    that.listReplies.splice(index, 1);
                } catch (error){
                    console.error(error);
                }
            }
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
                    let comm = new ResponseCommentModel();
                    comm.copy(com);
                    return comm;
                });
                this.listReplies = this.listReplies.concat(tempComment);
            } catch (error){
                Request.resolveAxiosError(error);
            } finally {
                this.isLoadingMoreReplies = false;
            }
        },

        async resolveSubmittedReplies(comment){
            try {
                if (comment == null) return;
                this.listReplies.push(comment);
            } catch (error){
                console.error(error);
            }
        },
    },
    props: {
        comment: {
            required: true
        },

        onPostedComment: {
            default: null
        },

        onDeletedComment: {
            default: null
        }
    },
    watch: {
        comment(){
            try {
                // if (this.comment?.registerObserver){
                //     this.comment.registerObserver(this.refreshComment.bind(this));
                // }
                this.refreshComment();
            } catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        getLanguage: {},
        getPost: {},
        getPopupManager: {},
        getToastManager: {}
    },
    provide(){
        return {
            getComment: () => this.dComment,
        }
    }
}

</script>

<style scoped>

@import url(@/css/pages/desktop/components/comment.css);

</style>