/* eslint-disable */
<template>
    <FeedCardFrame>
        <div class="p-feedcard-lesson">
            <div class="p-feedcard-lesson__header">
                <PostCardHeader />
            </div>
            <div class="p-feedcard-lesson__title">
                {{lesson?.Title ?? "Title of lesson"}}
            </div>
            <div class="p-feedcard-lesson__categories">
                <CategoriesList :categories="compiledCategories" />
            </div>
            <div class="p-feedcard-lesson__content">
                <LatexMarkdownRender :markdown-content="lesson?.Content" />
            </div>
            <div class="p-feedcard-lesson__thumbnail">
                <PostCardThumbnail />
            </div>
            <div class="p-feedcard-lesson__toolbar">
                <PostCardToolBar />
            </div>
            <div class="p-feedcard-lesson__devide">
                <div></div>
            </div>
            <div class="p-feedcard-lesson__comments">
                <PostCardCommentList />
            </div>
        </div>
    </FeedCardFrame>
</template>

<script>
import PostCardCommentList from '../../home/components/feed-subpage/comment/PostCardCommentList.vue';
import LatexMarkdownRender from '@/components/base/markdown/LatexMarkdownRender.vue';
import PostCardToolBar from '../../home/components/feed-subpage/postcard/PostCardToolBar.vue';
import PostCardThumbnail from '../../home/components/feed-subpage/postcard/PostCardThumbnail.vue';
import CategoriesList from '@/components/base/category/CategoriesList.vue';
import PostCardHeader from '../../home/components/feed-subpage/postcard/PostCardHeader.vue';
import FeedCardFrame from '../../home/components/feed-subpage/postcard/FeedCardFrame.vue';

export default {
    name: "lessonCard",
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
        }
    },
    mounted() {
        this.getLabel();
        this.compiledCategories = this.getCategories();
    },
    components: {
        LatexMarkdownRender, PostCardCommentList,
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

        getCategories() {
            if (this.lesson?.Categories != null
                && this.lesson.Categories.length > 0){
                return this.lesson.Categories
                    .map(cate => cate.CategoryName);
            }
            return this.defaultCategoriesList;
        },

        async resolveClickAddLesson(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-lesson'});
            } catch (error) {
                console.log(error);
            }
        },

        async resolveClickAddQuestion(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-question'});
            } catch (error) {
                console.log(error);
            }
        }
        
    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    },
    provide(){
        return {
            getPost: () => this.lesson
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

@import url(@/css/pages/desktop/components/lesson.css);

.p-feedcard-lesson__content{
    font-size: 16px;
}

</style>