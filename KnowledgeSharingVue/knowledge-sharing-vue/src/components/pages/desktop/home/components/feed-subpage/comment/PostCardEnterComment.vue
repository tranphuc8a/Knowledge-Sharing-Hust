<template>

    <div class="p-enter-comment"  @keydown.shift.enter.prevent.stop="resolveSubmitComment">
        <div class="p-enter-comment-avatar">
            <UserAvatar :user="curentUser" :size="36" />
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
                :onclick="resolveSubmitComment" />
        </div>
    </div>

</template>


<script>
import UserAvatar from '@/components/base/avatar/UserAvatar.vue';
import MTextArea from '@/components/base/inputs/MTextArea';
import CurrentUser from '@/js/models/entities/current-user';
import { NotEmptyValidator } from '@/js/utils/validator';

export default {
    name: "p-enter-comment",
    data() {
        return {
            label: null,
            listComments: [null, null],
            curentUser: null,
            commentValidator: new NotEmptyValidator().setErrorMsg("Bình luận không được trống"),

            components: {
                textarea: null,
                submit: null
            }
        }
    },
    async mounted(){
        try {
            this.curentUser = await CurrentUser.getInstance();
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
            try {
                // validate form

                // get text
                let text = await this.components.textarea.getValue();

                // submit comment

                // update ui
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(text);
                }
            } catch (e){
                console.error(e);
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
        }
    },
    props: {
        useritem: {},
        onCommentSubmitted: {},
        isEditing: {
            default: false
        },
        value: {
            default: ''
        },
        placeholder: {
            default: "Thêm bình luận"
        }
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