<template>
    <div class="p-postcard-comment-list">
        <div class="p-pcl-filter-button">
            <CommentFilterButton :on-change="resolveOnchangeFilter" />
        </div>
        <div class="p-pcl-comment-list">
            <PostCardComment v-for="comment in listComments" :key="comment?.UserItemId" :comment="comment"/>
        </div>
        <div class="p-pcl-enter-comment">
            <PostCardEnterComment :useritem="post"/>
        </div>
    </div>
</template>

<script>
import PostCardComment from './PostCardComment.vue';
import PostCardEnterComment from './PostCardEnterComment.vue';
import CommentFilterButton from './CommentFilterButton.vue';
import { myEnum } from '@/js/resources/enum';


export default {
    name: "PostCardcomment-list",
    data(){
        return {
            label: null,
            listComments: [null, null],
        }
    },
    components: {
        CommentFilterButton, PostCardEnterComment, PostCardComment
    },
    mounted(){
        this.getLabel();
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
        }
        
    },
    inject: {
        getLanguage: {},
        post: {
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
    gap: 18px;
}

</style>
