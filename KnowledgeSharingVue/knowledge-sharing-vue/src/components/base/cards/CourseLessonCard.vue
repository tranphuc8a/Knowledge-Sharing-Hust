

<template>
    <CourseLessonCardSkeleton v-if="!isLoaded" />
    
    <div class="p-course-lesson-card" v-if="isLoaded">
        <div class="p-clc-header">
            <div class="p-clc-offset card-subheading">
                <EllipsisText :text="offsetText" :max-line="1" />
            </div>
            <div class="p-clc-button">
                <router-link :to="detailLink" class="router-link">
                    <MEmbeddedButton 
                        label="Xem bài giảng"
                    />
                </router-link>
            </div>
        </div>
        <div class="p-clc-content">
            <LessonShortCard :lesson="dCourseLesson.Lesson" :detail-link="detailLink" > 
                <template #lessonMenuContext>
                    <CourseLessonMenuContext v-if="isShowMenuContext" />
                </template>
            </LessonShortCard>
        </div>
    </div>
</template>



<script>
import LessonShortCard from './LessonShortCard.vue'
import ResponseCourseLessonModel from '@/js/models/api-response-models/response-course-lesson-model';
import MEmbeddedButton from './../buttons/MEmbeddedButton.vue'
import CourseLessonMenuContext from './CourseLessonMenuContext.vue'
import CourseLessonCardSkeleton from './CourseLessonCardSkeleton.vue';
import EllipsisText from '../text/EllipsisText.vue';

export default {
    name: 'CourseLessonCard',
    components: {
        LessonShortCard,
        CourseLessonMenuContext,
        MEmbeddedButton,
        EllipsisText,
        CourseLessonCardSkeleton,
    },
    props: {
        responseCourseLessonModel: {
            required: true,
        },
        isShowMenuContext: {
            default: true,
        }
    },
    watch: {
        responseCourseLessonModel(){
            this.refreshCourseLesson();
        }
    },
    data(){
        return {
            dCourseLesson: null,
            offsetText: null,
            isLoaded: false,
            detailLink: '',
        }
    },
    async mounted(){
        this.refreshCourseLesson();
    },
    methods: {
        refreshCourseLesson(){
            try {
                if (this.responseCourseLessonModel == null) return;
                this.dCourseLesson = new ResponseCourseLessonModel().copy(this.responseCourseLessonModel);
                this.isLoaded = true;
                this.offsetText = `Bài ${this.dCourseLesson.Offset}. ${this.dCourseLesson.LessonTitle}`;
                let courseId = this.dCourseLesson.CourseId;
                this.detailLink = `/course-lesson/` + courseId + `/` + this.dCourseLesson.Offset;
            } catch (error) {
                console.error(error);
            }
        }
    },
    inject: {
    },
    provide(){
        return {
            getCourseLesson: () => this.dCourseLesson,
        }
    }
}

</script>


<style scoped>

.p-course-lesson-card{
    max-width: 100%;
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}

.p-clc-header{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-clc-header > * {
    width: fit-content;
    max-width: 100%;
    flex-shrink: 0;
    flex-grow: 0;
}

.p-clc-content{
    width: 100%;
    max-width: 100%;
}

.p-clc-offset{
    max-width: 100%;
}

.p-clc-button{
    width: fit-content;
    max-width: 100%;
    flex-shrink: 0;
    flex-grow: 0;
}

</style>

