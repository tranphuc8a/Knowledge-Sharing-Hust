<template>

    <div class="p-enter-comment"  @keydown.shift.enter.prevent.stop="resolvePressShiftEnter">
        <div class="p-enter-comment-avatar">
            <UserAvatar :user="currentUser" :size="36" />
        </div>
        <div class="p-enter-comment-textarea">
            <MTextArea
                ref="textarea"
                :placeholder="placeholder"
                :is-show-title="false" :is-show-error="true" 
                :validator="commentValidator"
                max-height="150px" rows="1"
                />
        </div>
        <div class="p-enter-comment-submit" ref="submit">
            <MActionIcon fa="paper-plane" 
                :containerStyle="{width: '36px', height: '36px'}"
                :onclick="resolveSubmitComment" 
                ref="actionicon"/>
        </div>
    </div>

</template>


<script>
import UserAvatar from '@/components/base/avatar/UserAvatar.vue';
import MTextArea from '@/components/base/inputs/MTextArea';
import CurrentUser from '@/js/models/entities/current-user';
import { PatchRequest, PostRequest, Request } from '@/js/services/request';
import { NotEmptyValidator } from '@/js/utils/validator';
import { myEnum } from '@/js/resources/enum';
import ResponseCommentModel from '@/js/models/api-response-models/response-comment-model';

export default {
    name: "p-enter-comment",
    data() {
        return {
            isSubmiting: false,
            label: null,
            listComments: [null, null],
            currentUser: null,
            commentValidator: new NotEmptyValidator().setErrorMsg("Bình luận không được trống"),

            components: {
                textarea: null,
                submit: null
            }
        }
    },
    async mounted(){
        try {
            this.currentUser = await CurrentUser.getInstance();
            this.components = {
                textarea: this.$refs.textarea,
                submit: this.$refs.submit
            }
            this.components.textarea.setValue(this.value);
        }
        catch (error) {
            console.error(error);
        }
    },
    components: {
        MTextArea,
        UserAvatar,
    },
    methods: {

        async resolveSubmitComment(){
            if (this.isSubmiting) return;
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.isSubmiting = true;

                // validate form
                if(! await this.$refs.textarea.validate()){
                    this.$refs['textarea'].startDynamicValidate();
                    this.$refs['textarea'].focus();
                    return;
                }

                // get text
                let text = await this.components.textarea.getValue();
                
                if (this.isEditing){
                    await this.editComment(text);
                } else if (this.useritem.UserItemType == myEnum.EUserItemType.Comment){
                    await this.replyComment(text);
                } else {
                    await this.addComment(text);
                }

                this.components.textarea.setValue("");
            } catch (e){
                console.error(e);
            } finally {
                this.isSubmiting = false;
            }
        },

        async resolvePressShiftEnter(){
            try {
                let actionIcon = this.$refs.actionicon;
                if (actionIcon?.resolveOnClick){
                    await actionIcon.resolveOnClick();
                }
            } catch (error){
                console.error(error);
            }
        },

        async editComment(text){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                await new PatchRequest('Comments/' + this.editFor)
                    .setBody({ Content: text })
                    .execute();
                // update ui
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(text);
                }
            } catch (e){
                Request.resolveAxiosError(e);
                // update ui
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(null);
                }
            }
        },
        
        async replyComment(text){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                let res = await new PostRequest('Comments/reply')
                    .setBody({
                        ReplyId: this.useritem.UserItemId,
                        Content: text
                    }).execute();
                let body = await Request.tryGetBody(res);
                let comment = new ResponseCommentModel();
                comment.copy(body);
                comment.User = this.currentUser;

                // update ui
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(comment);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            }
        },

        async addComment(text){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                let res = await new PostRequest('Comments')
                    .setBody({
                        KnowledgeId: this.useritem.UserItemId,
                        Content: text
                    }).execute();
                let body = await Request.tryGetBody(res);
                let comment = new ResponseCommentModel();
                comment.copy(body);
                comment.User = this.currentUser;

                // update ui
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(comment);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            }
        },

        /**
         * Focus on textarea
         * @param none
         * @returns none
         * @Created PhucTV (18/04/24)
         * @Modified None
         */
        async focus(){
            try {
                this.$refs.textarea?.focus?.();
            } catch (e){
                console.error(e);
            }
        },

        /**
         * Set value to textarea
         * @param none
         * @returns none
         * @Created PhucTV (13/05/24)
         * @Modified None
         */
        async setValue(value){
            try {
                this.$refs.textarea?.setValue?.(value);
            } catch (e){
                console.error(e);
            }
        }
    },
    props: {
        useritem: {},
        onCommentSubmitted: {},
        isEditing: {
            default: false
        },
        editFor: {},
        value: {
            default: ''
        },
        placeholder: {
            default: "Thêm bình luận"
        }
    },
    inject: {
        getPopupManager: {}
    }
}

</script>

<style scoped> 
.p-enter-comment{
    width: 100%;
    height: auto;
    display: flex;
    flex-flow: row nowrap;
    justify-self: space-between;
    align-items: flex-start;
    gap: 4px;
}

.p-enter-comment > :first-child,
.p-enter-comment > :last-child{
    align-self: stretch;
}

.p-enter-comment > :last-child{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: flex-end;
}

.p-enter-comment-textarea{
    width: 0;
    flex: 1;
}


</style>