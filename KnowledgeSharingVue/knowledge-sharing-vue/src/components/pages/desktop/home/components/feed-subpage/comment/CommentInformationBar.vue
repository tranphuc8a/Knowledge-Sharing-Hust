<template>
    <div class="p-comment-information-bar">
        <div class="p-comment-infor">
            <div class="p-comment-time">
                <VisualizedDatetime :datetime="commentProvider?.CreatedTime" />
            </div>
            <div class="p-comment-star">
                <VisualizedStar :star="commentProvider?.Star ?? 3.68" />
            </div>
            <MIcon fa="circle" :style="dotIconStyle" />
            <div class="p-comment-numstar">
                {{ getNumStar() }}
            </div>
        </div>
        <div class="p-comment-star-actions">
            <div class="p-comment-star-button">
                <CommentStarButton />
            </div>
            <div class="p-comment-reply-button" @:click="resolveReplyComment">
                Phản hồi
            </div>
        </div>
    </div>

</template>

<script>
import VisualizedDatetime from '@/components/base/visualized-components/VisualizedDatetime.vue';
import VisualizedStar from '@/components/base/visualized-components/VisualizedStar.vue';
import CommentStarButton from './CommentStarButton.vue';
import Common from '@/js/utils/common';

export default {
    name: 'CommentInformationBar',
    props: {
        onReply: {}
    },
    components: {
        VisualizedDatetime,
        VisualizedStar,
        CommentStarButton
    },
    data() {
        return {
            label: null,
            dotIconStyle: {fontSize: '3px', color: 'var(--grey-color-600)'},
            iconStyle: {color: 'var(--grey-color)'},
            buttonStyle: {color: 'var(--grey-color-800)'}
        }
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
            if (this.inject?.language != null){
                this.label = this.inject?.language?.subpages?.feedpage?.postcard;
            }
            return this.label;
        },


        getNumStar(){
            try {
                let numstar = Number(this.commentProvider?.NumberStar ?? 0);
                let beautyNumber = Common.formatNumber(numstar);
                return this.getLabel()?.numberStar(beautyNumber);
            } catch (e) {
                console.error(e);
            }
        },


        resolveReplyComment(){
            if (this.onReply){
                this.onReply();
            }
        }
    },
    inject: {
        inject: {},
        commentProvider: {}
    }
}

</script>

<style scoped>


.p-comment-numstar{
    cursor: pointer;
}


.p-comment-information-bar{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 0px;
    width: 100%;
}

.p-comment-infor{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: flex-start;
    gap: 8px;
    width: 100%;
}

.p-comment-reply-button{
    cursor: pointer;
    color: var(--grey-color);
    font-family: 'ks-font-semibold';
    font-size: 12px;
}
.p-comment-reply-button:hover,
.p-comment-numstar:hover{
    color: var(--grey-color-700);
}
.p-comment-reply-button:active,
.p-comment-numstar:active{
    color: var(--grey-color-800);
}


.p-comment-star-actions{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 8px;
    width: 100%;
}

.p-comment-numstar{
    color: var(--grey-color);
    font-family: 'ks-font-semibold';
    font-size: 12px;
}



</style>