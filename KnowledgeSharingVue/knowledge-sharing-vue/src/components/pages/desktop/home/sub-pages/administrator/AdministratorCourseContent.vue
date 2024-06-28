

<template>
    <AdministratorListCourseContent v-if="isAdminListCourse" />
    <AdministratorOneCourseContent v-if="isAdminOneCourse" />
</template>



<script>

import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import AdministratorListCourseContent from './AdministratorListCourseContent.vue';
import AdministratorOneCourseContent from './AdministratorOneCourseContent.vue';



export default {
    name: 'AdministratorCourseContent',
    components: {
        AdministratorListCourseContent,
        AdministratorOneCourseContent
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            isAdminListCourse: false,
            isAdminOneCourse: false,
        }
    },
    async created(){
    },
    async mounted(){
        this.refreshPage();
    },
    methods: {
        async refreshPage(){
            try {
                let filter = this.route.query.filter;
                if (Validator.isEmpty(filter)){
                    this.isAdminListCourse = true;
                    this.isAdminOneCourse = false;
                } else {
                    this.isAdminListCourse = false;
                    this.isAdminOneCourse = true;
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    watch: {
        '$route.query.search': {
            handler(){
                this.refreshPage();
            },
            deep: true,
        },
        '$route.query.filter': {
            handler(){
                this.refreshPage();
            },
            deep: true,
        },
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

</style>

