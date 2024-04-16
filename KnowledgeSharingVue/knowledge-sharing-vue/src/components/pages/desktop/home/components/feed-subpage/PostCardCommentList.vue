<template>
    <div class="p-postcard-comment-list">
        <div class="p-pcl-filter-button">
            <CommentFilterButton :on-change="resolveOnchangeFilter" />
        </div>
        <div class="p-pcl-comment-list">

        </div>
        <div class="p-pcl-enter-comment">
            <PostCardEnterComment :useritem="post"/>
        </div>
    </div>
</template>

<script>
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
        CommentFilterButton, PostCardEnterComment
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
            if (this.inject?.language != null){
                this.label = this.inject?.language?.subpages?.feedpage?.postcard;
            }
            console.log(this.inject);
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
        inject: {},
        post: {
            default: null
        }
    }
}

</script>

<style scoped>

</style>
