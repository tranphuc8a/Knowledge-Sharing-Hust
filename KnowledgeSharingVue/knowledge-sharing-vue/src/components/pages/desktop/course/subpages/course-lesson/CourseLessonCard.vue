

<template>
    <div class="p-course-lesson-card card">
        
    </div>
</template>



<script>



export default {
    name: 'CourseLessonCard',
    components: {
    },
    props: {
        responseCourseLessonModel: {}
    },
    data(){
        return {
            isLoaded: false,
        }
    },
    async created(){
        try {

        } catch (e){
            console.error(e);
        }
    },
    async mounted(){
    },
    methods: {

    },
    inject: {
        getCourse: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-lesson-card{
    width: 100%;
    padding: 16px;
}

</style>

