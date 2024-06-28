/* eslint-disable */
<template>
    <FeedCardFrame>
        <div class="p-feedcard-lestion">
            <div class="p-feedcard-lestion__header">
                <PostCardHeader />
            </div>
            <div class="p-feedcard-lestion__title">
                {{lesson?.Title ?? "Title of lesson"}}
            </div>
            <div class="p-feedcard-lestion__abstract" v-show="lesson?.Abstract != null">
                <!-- <textarea type="text" v-model="content"/> -->
                <LightLatexMarkdownRender :markdown-content="lesson?.Abstract" />
            </div>
            <div class="p-feedcard-lestion__categories" v-show="compiledCategories?.length > 0">
                <CategoriesList :categories="compiledCategories" />
            </div>
            <div class="p-feedcard-lestion__devide" 
                v-if="lesson?.Content != null"
                >
                <div></div>
            </div>
            <div class="p-feedcard-lestion__content" v-if="lesson?.Content != null">
                <LightLatexMarkdownRender :markdown-content="lesson?.Content" />
            </div>
            <div class="p-feedcard-lestion__thumbnail">
                <PostCardThumbnail />
            </div>
            <div class="p-feedcard-lestion__toolbar">
                <PostCardToolBar />
            </div>
            <div class="p-feedcard-lestion__devide">
                <div></div>
            </div>
            <div class="p-feedcard-lestion__comments" v-if="isShowComment">
                <PostCardCommentList ref="comment-list"/>
            </div>
        </div>
    </FeedCardFrame>
</template>

<script>
import PostCardCommentList from '../../home/components/feed-subpage/comment/PostCardCommentList.vue';
import LightLatexMarkdownRender from '@/components/base/markdown/LightLatexMarkdownRender.vue';
import PostCardToolBar from '../../home/components/feed-subpage/postcard/PostCardToolBar.vue';
import PostCardThumbnail from '../../home/components/feed-subpage/postcard/PostCardThumbnail.vue';
import CategoriesList from '@/components/base/category/CategoriesList.vue';
import PostCardHeader from '../../home/components/feed-subpage/postcard/PostCardHeader.vue';
import FeedCardFrame from '../../home/components/feed-subpage/postcard/FeedCardFrame.vue';
import postcard from '@/js/components/base/postcard';


export default {
    name: "LessonCard",
    data() {
        return {
            label: null,
            defaultCategoriesList: [],
            buttonStyle: {
                color: 'var(--blue-grey-color-800)',
            },
            iconStyle: {
                color: 'var(--grey-color)',
            },
            content: '',
            lesson: this.post,
            compiledCategories: [],
            isShowComment: true,
        }
    },
    mounted() {
        this.getLabel();
        this.compiledCategories = this.getCategories();
    },
    components: {
        LightLatexMarkdownRender, PostCardCommentList,
        PostCardThumbnail, PostCardToolBar,
        FeedCardFrame, PostCardHeader, CategoriesList
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

        ...postcard.methods,
    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    },
    provide(){
        return {
            getPost: () => this.lesson,
            resolveClickComment: this.resolveOnClickComment,
            forceUpdateCommentList: this.forceUpdateCommentList,
        }
    },
    watch: {
        post(newValue) {
            this.lesson = newValue;
            this.compiledCategories = this.getCategories();
        }
    },
    props: {
        post: {
            required: true
        }
    }
}
</script>

<style scoped>

@import url(@/css/pages/desktop/components/lestion.css);

.p-feedcard-lestion__content{
    font-size: 16px;
}

</style>