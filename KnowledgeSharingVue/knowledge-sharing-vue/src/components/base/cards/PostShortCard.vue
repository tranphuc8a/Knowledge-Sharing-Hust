

<template>
    <PostShortCardSkeleton v-if="!isLoaded" />

    <div class="p-post-short-card" v-if="isLoaded">
        <div class="card">
            <div class="p-psc-thumbnail">
                <TooltipFrame :style="tooltipStyle">
                    <template #tooltipMask>
                        <router-link :to="postDetailLink" class="router-link">
                            <div class="p-psc-thumbnail-image"
                                :style="{backgroundImage: `url(${postThumbnail})`}"
                            >
                                <div class="p-psc-thumbnail-overlay"></div>
                            </div>
                        </router-link>
                    </template>
                    
                    <template #tooltipContent>
                        <PostTooltip :post="dPost" :detail-link="postDetailLink" />
                    </template>
                </TooltipFrame>
            </div>

            <div class="p-psc-information">
                <div class="p-psc-infor__left">
                    <div class="p-psc-lesson-title">
                        <TooltipFrame :style="tooltipStyle">
                            <template #tooltipMask>
                                <router-link :to="postDetailLink">
                                    <EllipsisText :text="dPost?.Title" :style="postTitleStyle" :max-line="1"/>
                                </router-link>
                            </template>
                            
                            <template #tooltipContent>
                                <PostTooltip :post="dPost" :detail-link="postDetailLink" />
                            </template>
                        </TooltipFrame>
                    </div>
                    <div class="p-psc-post-owner">
                        <TooltipUserAvatarAndUsername :user="postOwner" :size="32" />
                    </div>
                    <div class="p-psc-post-stars">
                        <VisualizedTotalStar 
                            :average-star="dPost?.AverageStar" 
                            :totalStar="dPost?.TotalStar" 
                        />
                    </div>
                </div>
                <div class="p-psc-infor__right">
                    <div class="p-psc-menu-context">
                        <slot name="postMenuContext"></slot>
                    </div>
                    <SaveButton :knowledge-id="dPost?.UserItemId" :init-value="dPost?.IsMarked" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import TooltipFrame from '../tooltip/TooltipFrame.vue';
import PostTooltip from './PostTooltip.vue';
import Common from '@/js/utils/common';
import SaveButton from './../others/SaveButton.vue';
import VisualizedTotalStar from '../others/VisualizedTotalStar.vue';
import EllipsisText from '../text/EllipsisText.vue';
import TooltipUserAvatarAndUsername from '../avatar/TooltipUserAvatarAndUsername.vue';
import PostShortCardSkeleton from './PostShortCardSkeleton.vue';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'PostShortCard',
    components: {
        SaveButton,
        VisualizedTotalStar,
        EllipsisText,
        TooltipUserAvatarAndUsername,
        TooltipFrame,
        PostTooltip,
        PostShortCardSkeleton,
    },
    props: {
        post: {
            required: true,
        },
        detailLink: {
            type: String,
            default: null
        }
    },
    watch: {
        post(){
            this.refreshNewPost();
        }
    },
    data(){
        return {
            tooltipStyle: { boxShadow: '0px 0px 8px 4px rgba(var(--primary-color-rgb), .56)'},
            dPost: this.post,
            defaultpostThumbnail: this.post.PostType == myEnum.EPostType.Lesson ? 
                require('@/assets/default-thumbnail/lesson-image-icon.png') :
                require('@/assets/default-thumbnail/discussion-image-icon.jpg'),
            postThumbnail: null,
            postDetailLink: '',
            postTitleStyle: {
            },
            postOwner: null,
            isLoaded: false,
        }
    },
    async created(){
        this.refreshNewPost();
    },
    async mounted(){
    },
    methods: {
        async refreshNewPost(){
            this.dPost = this.post;
            if (this.dPost == null){
                return;
            }
            let isLesson = this.dPost.PostType == myEnum.EPostType.Lesson;
            let postLink = isLesson ? `/lesson/${this.dPost.UserItemId}` : `/question/${this.dPost.UserItemId}`;
            this.postDetailLink = this.detailLink ?? postLink;
            await this.updateThumbnail();
            this.postOwner = this.dPost?.getUser();
            this.isLoaded = true;
        },

        async updateThumbnail(){
            try {
                let isThumbnailValid = await Common.isValidImage(this.dPost.Thumbnail);
                if(isThumbnailValid){
                    this.postThumbnail = this.dPost.Thumbnail;
                } else {
                    this.postThumbnail = this.defaultpostThumbnail;
                }
            } catch (error) {
                console.error(error);
            }
        },
    },
    inject: {
    },
    provide(){
        return {
            getPost: () => this.dPost,
        }
    }
}

</script>


<style scoped>


.p-post-short-card{
    width: 100%;
    max-width: 100%;
}

.p-post-short-card > .card{
    height: 120px;
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-psc-thumbnail{
    width: 200px;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-image{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
    position: relative;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-overlay{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    opacity: 0;
    transition: opacity 0.3s;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-image:hover .p-psc-thumbnail-overlay{
    opacity: 1;
}

.p-psc-information{
    min-width: 300px;
    width: 100%;
    max-width: calc(100% - 200px);
    height: 100%;
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-psc-infor__left{
    width: 0;
    flex-grow: 1;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: stretch;
}

.p-psc-infor__left > *{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}


.p-psc-lesson-title{
    max-width: 100%;
    width: fit-content;
    height: fit-content;
    text-align: left;
    font-size: 18px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-psc-lesson-title a {
    max-width: 100%;
    max-height: 100%;
}

.p-psc-post-owner{
    width: 100%;
    height: fit-content;
}

.p-psc-post-stars{
    width: 100%;
    height: fit-content;
}

.p-psc-infor__right{
    width: fit-content;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 4px;
}

.p-psc-menu-context{
    width: fit-content;
    height: fit-content;
}


</style>

