

<template>
    <div class="p-course-lesson-card card">
        
    </div>
</template>



<script>
import ResponseCourseLessonModel from '@/js/models/api-response-models/response-course-lesson-model';




export default {
    name: 'CourseLessonCard',
    components: {
    },
    props: {
        responseCourseLessonModel: {}
    },
    data(){
        return {
            courseLesson: null,
            isLoaded: false,
        }
    },
    async created(){
        try {
            this.refresh();
        } catch (e){
            console.error(e);
        }
    },
    async mounted(){
    },
    methods: {
        async refresh(){
            try {
                this.courseLesson = new ResponseCourseLessonModel().copy(this.responseCourseLessonModel);
                if (this.courseLesson != null){
                    this.isLoaded = true;
                    return;
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getCourse: {}
    },
    provide(){
        return {
            getCourse: () => this.courseLesson?.Course,
            getLesson: () => this.courseLesson?.Lesson,
            getCourseLesson: () => this.courseLesson,
        }
    }
}

</script>


<style scoped>

.p-course-lesson-card{
    width: 100%;
    padding: 16px;
}

</style>

