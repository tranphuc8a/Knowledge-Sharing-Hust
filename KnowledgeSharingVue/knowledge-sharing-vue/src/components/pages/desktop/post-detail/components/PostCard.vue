/* eslint-disable */
<template>
    <FeedCardFrame>
        <div class="p-feedcard-lestion">
            <div class="p-feedcard-lestion__header">
                <PostCardHeader />
            </div>
            <div class="p-feedcard-lestion__title">
                {{dPost?.Title ?? "Title of post"}}
            </div>
            <div class="p-feedcard-lestion__abstract" v-show="dPost?.Abstract != null">
                <!-- <textarea type="text" v-model="content"/> -->
                <LatexMarkdownRender :markdown-content="dPost?.Abstract" />
            </div>
            <div class="p-feedcard-lestion__categories" v-show="compiledCategories?.length > 0">
                <CategoriesList :categories="compiledCategories" />
            </div>
            <div class="p-feedcard-lestion__devide" 
                v-if="dPost?.Content != null"
                >
                <div></div>
            </div>
            <div class="p-feedcard-lestion__content" v-if="dPost?.Content != null">
                <LatexMarkdownRender :markdown-content="dPost?.Content" />
            </div>
            <div class="p-feedcard-lestion__thumbnail">
                <PostCardThumbnail />
            </div>
            <div class="p-feedcard-lestion__toolbar">
                <PostCardToolBar />
            </div>
            <div class="p-feedcard-lestion__devide" v-if="isShowComment">
                <div></div>
            </div>
            <div class="p-feedcard-lestion__comments" v-if="isShowComment">
                <PostCardCommentList ref="comment-list" />
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
    name: "PostCard",
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
            dPost: this.post,
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
            if (this.dPost?.Categories != null
                && this.dPost.Categories.length > 0){
                return this.dPost.Categories
                    .map(cate => cate.CategoryName);
            }
            return this.defaultCategoriesList;
        },
        
        async resolveOnClickComment(){
            try {
                if (this.$refs['comment-list'] != null){
                    await this.$refs['comment-list'].focus();
                }
            } catch (e){
                console.error(e);
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
            getPost: () => this.dPost,
            resolveClickComment: this.resolveOnClickComment,
        }
    },
    watch: {
        post(newValue) {
            this.dPost = newValue;
            this.compiledCategories = this.getCategories();
        }
    },
    props: {
        post: {
            required: true
        },

        isShowComment: {
            default: true
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