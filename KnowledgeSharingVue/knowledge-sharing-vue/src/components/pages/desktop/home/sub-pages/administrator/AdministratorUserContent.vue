

<template>
    <AdministratorListUserContent v-if="isAdminListUser" />
    <AdministratorOneUserContent v-if="isAdminOneUser" />
</template>



<script>
import AdministratorListUserContent from './AdministratorListUserContent.vue';
import AdministratorOneUserContent from './AdministratorOneUserContent.vue';
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';



export default {
    name: 'AdministratorUserContent',
    components: {
        AdministratorListUserContent,
        AdministratorOneUserContent
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            isAdminListUser: false,
            isAdminOneUser: false,
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
                    this.isAdminListUser = true;
                    this.isAdminOneUser = false;
                } else {
                    this.isAdminListUser = false;
                    this.isAdminOneUser = true;
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

