

<template>
    <AdministratorListLessonContent v-if="isAdminListPost" />
    <AdministratorOneLessonContent v-if="isAdminOnePost" />
</template>



<script>

import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import AdministratorListLessonContent from './AdministratorListLessonContent.vue';
import AdministratorOneLessonContent from './AdministratorOneLessonContent.vue';



export default {
    name: 'AdministratorLessonContent',
    components: {
        AdministratorListLessonContent,
        AdministratorOneLessonContent
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            isAdminListPost: false,
            isAdminOnePost: false,
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
                    this.isAdminListPost = true;
                    this.isAdminOnePost = false;
                } else {
                    this.isAdminListPost = false;
                    this.isAdminOnePost = true;
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

