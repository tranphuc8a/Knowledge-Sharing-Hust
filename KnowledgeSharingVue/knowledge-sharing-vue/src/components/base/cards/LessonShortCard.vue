

<template>
    <LessonShortCardSkeleton v-if="!isLoaded" />

    <div class="p-lesson-short-card" v-if="isLoaded">
        <div class="card">
            <div class="p-lsc-thumbnail">
                <TooltipFrame :style="tooltipStyle">
                    <template #tooltipMask>
                        <router-link :to="lessonDetailLink" class="router-link">
                            <div class="p-lsc-thumbnail-image"
                                :style="{backgroundImage: `url(${lessonThumbnail})`}"
                            >
                                <div class="p-lsc-thumbnail-overlay"></div>
                            </div>
                        </router-link>
                    </template>
                    
                    <template #tooltipContent>
                        <LessonTooltip :lesson="dLesson" :detail-link="lessonDetailLink" />
                    </template>
                </TooltipFrame>
            </div>

            <div class="p-lsc-information">
                <div class="p-lsc-infor__left">
                    <div class="p-lsc-lesson-title">
                        <TooltipFrame :style="tooltipStyle">
                            <template #tooltipMask>
                                <router-link :to="lessonDetailLink">
                                    <EllipsisText :text="dLesson?.Title" :style="titleLessonStyle" :max-line="1"/>
                                </router-link>
                            </template>
                            
                            <template #tooltipContent>
                                <LessonTooltip :lesson="dLesson" :detail-link="lessonDetailLink" />
                            </template>
                        </TooltipFrame>
                    </div>
                    <div class="p-lsc-lesson-owner">
                        <TooltipUserAvatarAndUsername :user="lessonOwner" :size="32" />
                    </div>
                    <div class="p-lsc-lesson-stars">
                        <VisualizedTotalStar 
                            :average-star="dLesson?.AverageStar" 
                            :totalStar="dLesson?.TotalStar" 
                        />
                    </div>
                </div>
                <div class="p-lsc-infor__right">
                    <div class="p-lcs-menu-context">
                        <slot name="lessonMenuContext"></slot>
                    </div>
                    <SaveButton :knowledge-id="dLesson?.UserItemId" :init-value="dLesson?.IsMarked" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import TooltipFrame from '../tooltip/TooltipFrame.vue';
import LessonTooltip from './LessonTooltip.vue';
import Common from '@/js/utils/common';
import SaveButton from './../others/SaveButton.vue';
import VisualizedTotalStar from '../others/VisualizedTotalStar.vue';
import EllipsisText from '../text/EllipsisText.vue';
import TooltipUserAvatarAndUsername from '../avatar/TooltipUserAvatarAndUsername.vue';
import LessonShortCardSkeleton from './LessonShortCardSkeleton.vue';

export default {
    name: 'LessonShortCard',
    components: {
        SaveButton,
        VisualizedTotalStar,
        EllipsisText,
        TooltipUserAvatarAndUsername,
        TooltipFrame,
        LessonTooltip,
        LessonShortCardSkeleton,
    },
    props: {
        lesson: {
            required: true,
        },
        detailLink: {
            type: String,
            default: null
        }
    },
    watch: {
        lesson(){
            this.refreshNewlesson();
        }
    },
    data(){
        return {
            tooltipStyle: { boxShadow: '0px 0px 8px 4px rgba(var(--primary-color-rgb), .56)'},
            dLesson: this.lesson,
            defaultLessonThumbnail: require('@/assets/default-thumbnail/lesson-image-icon.png'),
            lessonThumbnail: null,
            lessonDetailLink: '',
            titleLessonStyle: {
            },
            lessonOwner: null,
            isLoaded: false,
        }
    },
    async created(){
        this.refreshNewlesson();
    },
    async mounted(){
    },
    methods: {
        async refreshNewlesson(){
            this.dLesson = this.lesson;
            if (this.dLesson == null){
                return;
            }
            let lessonLink = `/lesson/${this.dLesson.UserItemId}`;
            this.lessonDetailLink = this.detailLink ?? lessonLink;
            await this.updateThumbnail();
            this.lessonOwner = this.dLesson?.getUser();
            this.isLoaded = true;
        },

        async updateThumbnail(){
            try {
                let isThumbnailValid = await Common.isValidImage(this.dLesson.Thumbnail);
                if(isThumbnailValid){
                    this.lessonThumbnail = this.dLesson.Thumbnail;
                } else {
                    this.lessonThumbnail = this.defaultLessonThumbnail;
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
            getPost: () => this.dLesson,
        }
    }
}

</script>


<style scoped>


.p-lesson-short-card{
    width: fit-content;
    max-width: 100%;
}

.p-lesson-short-card > .card{
    height: 120px;
    width: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-lsc-thumbnail{
    width: 200px;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-lsc-thumbnail-image{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
    position: relative;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-lsc-thumbnail-overlay{
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

.p-lsc-thumbnail-image:hover .p-lsc-thumbnail-overlay{
    opacity: 1;
}

.p-lsc-information{
    min-width: 400px;
    width: fit-content;
    max-width: calc(100% - 200px);
    height: 100%;
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-lsc-infor__left{
    width: 0;
    flex-grow: 1;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: stretch;
}

.p-lsc-infor__left > *{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}


.p-lsc-lesson-title{
    max-width: 100%;
    width: fit-content;
    height: fit-content;
    text-align: left;
    font-size: 18px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-lsc-lesson-title a {
    max-width: 100%;
    max-height: 100%;
}

.p-lsc-lesson-owner{
    width: 100%;
    height: fit-content;
}

.p-lsc-lesson-stars{
    width: 100%;
    height: fit-content;
}

.p-lsc-infor__right{
    width: fit-content;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-end;
    align-items: flex-end;
    gap: 16px;
}

.p-lcs-menu-context{
    width: fit-content;
    height: fit-content;
}


</style>

