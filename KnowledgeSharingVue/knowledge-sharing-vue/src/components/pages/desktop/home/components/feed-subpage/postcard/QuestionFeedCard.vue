<!-- /* eslint-disable */ -->
<template>
    <FeedCardFrame>
        <div class="p-feedcard-lestion">
            <div class="p-feedcard-lestion__header">
                <PostCardHeader />
            </div>
            <div class="p-feedcard-lestion__title">
                {{question?.Title ?? "Title of question"}}
            </div>
            <div class="p-feedcard-lestion__abstract" v-show="question?.Abstract != null">
                <!-- <textarea type="text" v-model="content"/> -->
                <LatexMarkdownRender :markdown-content="question?.Abstract" />
            </div>
            <div class="p-feedcard-lestion__categories" v-show="compiledCategories?.length > 0">
                <CategoriesList :categories="compiledCategories" />
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
            <div class="p-feedcard-lestion__comments">
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
    name: "questionFeedCard",
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

        async resolveClickAddquestion(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-lestion'});
            } catch (error) {
                console.log(error);
            }
        },

        async resolveClickAddQuestion(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-lestion'});
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

@import url(@/css/pages/desktop/components/lestion.css);

</style>