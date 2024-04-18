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
                this.label = this.getLanguage()?.subpages?.feedpage?.lessoncard;
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
    props: {
        post: {
            required: true
        }
    }
}
</script>

<style scoped>
.p-feedcard-lesson * {
    text-align: start;
}

.p-feedcard-lesson > div {
    width: 100%;
}

.p-feedcard-lesson{
    padding: 16px 0px 8px 0px;
    box-sizing: border-box;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-feedcard-lesson__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.p-feedcard-lesson__reminder{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;

    font-weight: 500;
    flex: 1;
    text-align: justify;
    font-size: 16px;
    min-height: 42px;
    border-radius: 32px;
    background-color: var(--grey-color-200);
    padding: 4px 12px;
    box-sizing: border-box;
    cursor: pointer;
    color: var(--grey-color-600);
}
.p-feedcard-lesson__reminder:hover{
    background-color: var(--grey-color-300);
    color: var(--blue-grey-color-800);
}


.p-feedcard-lesson__title{
    padding: 4px 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    font-weight: 600;
    font-size: 18px;
    width: 100%;
}

.p-feedcard-lesson__categories,
.p-feedcard-lesson__content,
.p-feedcard-lesson__devide,
.p-feedcard-lesson__comments{
    width: 100%;
    padding: 0px 16px;
}

.p-feedcard-lesson__devide > div{
    height: 1px;
    margin: 4px 0px;
    width: 100%;
    background-color: var(--grey-color-300);
}

</style>