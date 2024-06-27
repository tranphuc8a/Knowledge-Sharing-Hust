

<template>
    <div class="p-post-tooltip">
        <div class="card">
            <div class="p-pt-thumbnail">
                <RouterLink :to="dDetailLink" class="router-link">
                    <div class="p-pt-thumbnail-image"
                        :style="{ backgroundImage: 'url(' + dThumbnail + ')' }"
                    >
                        <div class="p-pt-thumbnail-overlay"></div>
                    </div>
                </RouterLink>
            </div>
            <div class="p-pt-information">
                <div class="p-pt-title card-subheading">
                    <RouterLink :to="dDetailLink" class="router-link">
                        <EllipsisText :text="dPost?.Title" :titleStyle="titleStyle" :max-line="1" />
                    </RouterLink>
                </div>
                <div class="p-pt-privacy">
                    <VisualizedPrivacy :privacy="dPost?.Privacy" :icon-style="{fontSize: '16px'}" />
                    {{ privacyText }}
                </div>
                <div class="p-pt-abstract" v-show="dPost?.Abstract != null">
                    <EllipsisText :text="dPost?.Abstract" :max-line="3" />
                </div>
                <div class="p-devide">
                </div>
                <div class="p-pt-actions">
                    <div class="p-pt-stars">
                        <VisualizedTotalStar :average-star="dPost?.AverageStar" :total-star="dPost?.TotalStar" />
                    </div>
                    <div class="p-pt-savebutton">
                        <SaveButton :knowledgeId="dPost?.UserItemId" :initValue="dPost?.IsMarked" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import EllipsisText from '../text/EllipsisText.vue';
import VisualizedPrivacy from '../visualized-components/VisualizedPrivacy.vue';
import VisualizedTotalStar from '../others/VisualizedTotalStar.vue';
import SaveButton from '../others/SaveButton.vue';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'PostTooltip',
    components: {
        EllipsisText,
        VisualizedPrivacy,
        VisualizedTotalStar,
        SaveButton,
    },
    props: {
        post: {
            required: true,
        },
        detailLink: {
            default: null,
        }
    },
    watch: {
        post(){
            this.refreshPost();
        }
    },
    data(){
        return {
            dPost: this.post,
            isLesson: this.post?.PostType == myEnum.EPostType.Lesson,
            defaultLink: (this.isLesson ? '/lesson/' : '/question') + this.post?.UserItemId,
            dDetailLink: '',
            defaultThumbnail: this.isLesson ? require('@/assets/default-thumbnail/lesson-image-icon.png')
                    : require('@/assets/default-thumbnail/discussion-image-icon.jpg'),
            dThumbnail: null,
            titleStyle: {},
            privacyText: null,
        }
    },
    async mounted(){
        this.refreshPost();
    },
    methods: {
        async refreshPost(){
            try {
                this.dPost = this.post;
                this.isLesson = this.post?.PostType == myEnum.EPostType.Lesson;
                this.defaultLink = (this.isLesson ? '/lesson/' : '/question/') + this.post?.UserItemId;
                this.dDetailLink = this.detailLink ?? this.defaultLink;
                if (await Common.isValidImage(this.post?.Thumbnail)){
                    this.dThumbnail = this.post?.Thumbnail;
                } else {
                    this.dThumbnail = this.defaultThumbnail;
                }
                let privacy = this.dPost?.Privacy;
                this.privacyText = privacy == myEnum.EPrivacy.Public ? 'Công khai' : 'Riêng tư';
            } catch (e){
                console.error(e)
            }
        }
    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-post-tooltip{
    min-width: 250px;
    max-width: 450px;
    width: 100%;
    cursor: pointer;
    font-size: initial;
}

.p-post-tooltip .card{
    width: 100%;
    display: flex;
    flex-direction: column;
    border-radius: 8px;
}

.p-pt-thumbnail{
    width: 100%;
    aspect-ratio: 16/9;
    overflow: hidden;
    position: relative;
    border-top-left-radius: 8px;
    border-top-right-radius: 8px;
    cursor: pointer;
}

.p-pt-thumbnail > a{
    width: 100%;
    height: 100%;
    display: block;
}

.p-pt-thumbnail-image{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
}

.p-pt-thumbnail-overlay{
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    position: absolute;
    top: 0;
    left: 0;
    opacity: 0;
    transition: opacity 0.3s;
}

.p-pt-thumbnail:hover .p-pt-thumbnail-overlay{
    opacity: 1;
}

.p-pt-information{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 8px;
}

.p-pt-title{
    max-width: 100%;
    width: fit-content;
    cursor: pointer;
    text-align: left;
}

.p-pt-privacy{
    width: 100%;
    display: flex;
    gap: 8px;
    align-items: center;
    font-size: 14.5px;
    justify-content: flex-start;
    color: var(--grey-color-600);
}

.p-pt-abstract{
    max-width: 100%;
    width: fit-content;
    font-size: 14px;
    font-family: 'ks-font-regular';
}

.p-pt-actions{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
}

.p-pt-stars{
    width: fit-content;
    font-size: 14px;
}

.p-pt-savebutton{
    width: fit-content;
}

</style>

