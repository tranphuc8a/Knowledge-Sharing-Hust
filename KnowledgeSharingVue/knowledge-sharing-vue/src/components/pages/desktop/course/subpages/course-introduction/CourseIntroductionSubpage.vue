

<template>
    <div class="p-course-introduction-subpage p-course-subpage">
        <div class="p-cis-content" v-if="!isLoaded">
            <div class="p-cis-item">
                <div style="padding: 16px; display: index; flex-flow: column nowrap; width: 100%"
                    class="card"
                >
                    <div class="skeleton" style="width: 30%; height: 20px; margin-bottom: 18px;"></div>
                    <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
                    <div class="skeleton" style="width: 100%; height: 20px; margin-bottom: 10px;"></div>
                    <div class="skeleton" style="width: 50%; height: 20px; margin-bottom: 10px;"></div>
                </div>
            </div>
        </div>

        <div class="p-cis-content" v-if="isLoaded">
            <div class="p-cis-item">
                <CourseIntroductionCard :course="dCourse" />
            </div>
        </div>
    </div>
</template>



<script>
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';
import CourseIntroductionCard from '../../components/course-card/CourseIntroductionCard.vue';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'CourseIntroductionSubpage',
    components: {
        CourseIntroductionCard,
    },
    props: {
    },
    data(){
        return {
            dCourse: null,
            isLoaded: false,
        }
    },
    async mounted(){
        await this.initPost();
    },
    methods: {
        async initPost(){
            try {
                this.dCourse = this.getCourse();
                // this.dLesson = await this.convertToLesson();
                // if (this.dLesson == null){
                //     return;
                // }
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        },


        async convertToLesson(){
            try {
                if (this.getCourse() == null) {
                    return null;
                }
                this.dCourse = new ResponseCourseModel().copy(this.getCourse());
                let lesson = new ResponseLessonModel().copy(this.dCourse);
                lesson.Content = this.dCourse.Introduction;
                lesson.PostType = myEnum.EPostType.Lesson;
                return lesson;
            } catch (e){
                console.error(e);
                return null;
            }
        }
    },
    inject: {
        getIsMyCourse: {},
        getCourse: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-cis-content{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-cis-item{
    max-width: 100%;
    width: fit-content;
    min-width: 600px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

</style>

