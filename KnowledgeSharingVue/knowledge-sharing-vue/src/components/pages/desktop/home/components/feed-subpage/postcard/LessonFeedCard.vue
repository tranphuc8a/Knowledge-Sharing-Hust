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
                <CategoriesList :categories="lesson?.Categories ?? defaultCategoriesList" />
            </div>
            <div class="p-feedcard-lesson__content">
                <textarea type="text" v-model="content"/>
                <LatexMarkdownRender :markdown-content="content" />
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
import PostCardCommentList from '../comment/PostCardCommentList.vue';
import LatexMarkdownRender from '@/components/base/markdown/LatexMarkdownRender.vue';
import PostCardToolBar from './PostCardToolBar.vue';
import PostCardThumbnail from './PostCardThumbnail.vue';
import CategoriesList from '@/components/base/category/CategoriesList.vue';
import PostCardHeader from './PostCardHeader.vue';
import FeedCardFrame from './FeedCardFrame.vue';

export default {
    name: "lessonFeedCard",
    data() {
        return {
            label: null,
            defaultCategoriesList: ["Hello", "Goodbye asjdg q22222222222222222222222222222222222222222"],
            buttonStyle: {
                color: 'var(--blue-grey-color-800)',
            },
            iconStyle: {
                color: 'var(--grey-color)',
            },
            content: '',
            lesson: this.post
        }
    },
    mounted() {
        this.getLabel();
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
            this.lesson = newValue
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

</style>