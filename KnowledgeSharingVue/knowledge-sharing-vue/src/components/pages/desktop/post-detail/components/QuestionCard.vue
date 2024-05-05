/* eslint-disable */
<template>
    <FeedCardFrame>
        <div class="p-feedcard-question">
            <div class="p-feedcard-question__header">
                <PostCardHeader />
            </div>
            <div class="p-feedcard-question__title">
                {{question?.Title ?? "Title of question"}}
            </div>
            <div class="p-feedcard-question__categories">
                <CategoriesList :categories="compiledCategories" />
            </div>
            <div class="p-feedcard-question__content">
                <LatexMarkdownRender :markdown-content="question?.Content" />
            </div>
            <div class="p-feedcard-question__thumbnail">
                <PostCardThumbnail />
            </div>
            <div class="p-feedcard-question__toolbar">
                <PostCardToolBar />
            </div>
            <div class="p-feedcard-question__devide">
                <div></div>
            </div>
            <div class="p-feedcard-question__comments">
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
    name: "QuestionCard",
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
            question: this.post,
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
            if (this.question?.Categories != null
                && this.question.Categories.length > 0){
                return this.question.Categories
                    .map(cate => cate.CategoryName);
            }
            return this.defaultCategoriesList;
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
            getPost: () => this.question
        }
    },
    watch: {
        post(newValue) {
            this.question = newValue;
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

@import url(@/css/pages/desktop/components/question.css);

.p-feedcard-question__content{
    font-size: 16px;
}

</style>