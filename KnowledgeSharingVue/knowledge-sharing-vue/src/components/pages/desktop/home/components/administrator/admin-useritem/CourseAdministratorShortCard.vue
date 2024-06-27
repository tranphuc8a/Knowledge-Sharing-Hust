

<template>

    <CourseShortCard
        :course="dCourse"
    >
        <template #courseMenuContext>
            <CourseAdministratorMenuContext />
        </template>
    </CourseShortCard>
    
</template>



<script>

import CourseShortCard from '@/components/pages/desktop/course/components/course-card/CourseShortCard.vue';
import CourseAdministratorMenuContext from './CourseAdministratorMenuContext.vue';

export default {
    name: 'CourseAdministatorShortCard',
    components: {
        CourseShortCard,
        CourseAdministratorMenuContext,
    },
    props: {
        course: {
            type: Object,
            required: true,
        }
    },
    watch: {
        course: {
            handler(){
                this.refresh();
            },
            deep: true,
        }
    },
    data(){
        return {
            dCourse: this.course
        }
    },
    async mounted(){
        this.dCourse = this.course;
    },
    methods: {
        async refresh(){
            try {
                this.dCourse = this.course;
            } catch (e){
                console.error(e);
            }
        
        }
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>

