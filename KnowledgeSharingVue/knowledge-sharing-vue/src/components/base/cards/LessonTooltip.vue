

<template>
    <div class="p-lesson-tooltip">
        <div class="card">
            <div class="p-lt-thumbnail">
                <RouterLink :to="dDetailLink" class="router-link">
                    <div class="p-lt-thumbnail-image"
                        :style="{ backgroundImage: 'url(' + dThumbnail + ')' }"
                    >
                        <div class="p-lt-thumbnail-overlay"></div>
                    </div>
                </RouterLink>
            </div>
            <div class="p-lt-information">
                <div class="p-lt-title">
                    <RouterLink :to="dDetailLink">
                        <EllipsisText :text="dLesson?.Title" :titleStyle="titleStyle" :max-line="1" />
                    </RouterLink>
                </div>
                <div class="p-lt-privacy">
                    <VisualizedPrivacy :privacy="dLesson?.Privacy" />
                    {{ privacyText }}
                </div>
                <div class="p-lt-abstract">
                    <EllipsisText :text="dLesson?.Abstract" :max-line="3" />
                </div>
                <div class="p-devide">
                </div>
                <div class="p-lt-actions">
                    <div class="p-lt-stars">
                        <VisualizedTotalStar :average-star="dLesson?.AverageStar" :total-star="dLesson?.TotalStar" />
                    </div>
                    <div class="p-lt-savebutton">
                        <SaveButton :knowledgeId="dLesson?.UserItemId" :initValue="dLesson?.IsMarked" />
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
    name: 'LessonTooltip',
    components: {
        EllipsisText,
        VisualizedPrivacy,
        VisualizedTotalStar,
        SaveButton,
    },
    props: {
        lesson: {
            required: true,
        },
        detailLink: {
            default: null,
        }
    },
    watch: {
        lesson(){
            this.refreshLesson();
        }
    },
    data(){
        return {
            dLesson: this.lesson,
            defaultLink: '/lesson/' + this.lesson?.UserItemId,
            dDetailLink: '',
            defaultThumbnail: require('@/assets/default-thumbnail/lesson-image-icon.png'),
            dThumbnail: null,
            titleStyle: {
                fontSize: '1.2rem',
                fontWeight: 'bold',
                color: 'var(--primary-color)',
            },
            privacyText: null,
        }
    },
    async mounted(){
    },
    methods: {
        async refreshLesson(){
            try {
                this.dLesson = this.lesson;
                this.defaultLink = '/lesson/' + this.lesson?.UserItemId;
                this.dDetailLink = this.detailLink ?? this.defaultLink;
                if (await Common.isValidImage(this.lesson?.Thumbnail)){
                    this.dThumbnail = this.lesson?.Thumbnail;
                } else {
                    this.dThumbnail = this.defaultThumbnail;
                }
                let privacy = this.dLesson?.Privacy;
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

.p-lesson-tooltip{
    width: 100%;
}

.p-lesson-tooltip .card{
    width: 100%;
    display: flex;
    flex-direction: column;
    border-radius: 8px;
}

.p-lt-thumbnail{
    width: 100%;
    aspect-ratio: 16/9;
    overflow: hidden;
    position: relative;
    border-top-left-radius: 8px;
    border-top-right-radius: 8px;
    cursor: pointer;
}

.p-lt-thumbnail > a{
    width: 100%;
    height: 100%;
    display: block;
}

.p-lt-thumbnail-image{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
}

.p-lt-thumbnail-overlay{
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    position: absolute;
    top: 0;
    left: 0;
    opacity: 0;
    transition: opacity 0.3s;
}

.p-lt-thumbnail:hover .p-lt-thumbnail-overlay{
    opacity: 1;
}

.p-lt-information{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-lt-title{
    width: 100%;
    cursor: pointer;
}

.p-lt-privacy{
    width: 100%;
    display: flex;
    gap: 8px;
    align-items: center;
    justify-content: flex-start;
    color: var(--grey-color-600);
    font-family: 'ks-font-semibold';
}

.p-lt-abstract{
    width: 100%;
}

.p-lt-actions{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
}

.p-lt-stars{
    width: fit-content;
}

.p-lt-savebutton{
    width: fit-content;
}

</style>

