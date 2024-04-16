<template>

    <div class="p-enter-comment"  @keydown.shift.enter.prevent.stop="resolveSubmitComment">
        <div class="p-enter-comment-avatar">
            <UserAvatar :user="curentUser" :size="36" />
        </div>
        <div class="p-enter-comment-textarea" ref="textarea">
            <MTextArea
                ref="textarea"
                placeholder="Thêm bình luận"
                :is-show-title="false" :is-show-error="true" 
                :validator="commentValidator"
                :oninput="adjustHeight"
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
        this.curentUser = await CurrentUser.getInstance();
        this.components = {
            textarea: this.$refs.textarea,
            submit: this.$refs.submit
        }
        this.adjustHeight();
    },
    components: {
        MTextArea,
        UserAvatar,
    },
    methods: {
        async adjustHeight(){
            try {
                let textarea = this.components.textarea;
                let submit = this.components.submit;

                submit.style.height = textarea.clientHeight + "px";
            } catch (e){
                console.error(e);
            }
        },


        async resolveSubmitComment(){
            try {
                // validate form

                // get text

                // submit comment

                // update
                let comment = {};
                if (this.onCommentSubmitted) {
                    await this.onCommentSubmitted(comment);
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    props: {
        useritem: {},
        onCommentSubmitted: {}
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
    height: calc(100%);
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