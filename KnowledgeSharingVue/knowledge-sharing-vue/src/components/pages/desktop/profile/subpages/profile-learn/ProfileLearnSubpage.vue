

<template>
    <div class="profile-learn-subpage p-profile-main-subpage">
        <div class="pls-top">            
            <!-- Profile Learn Toolbar Subpage -->
        </div>

        <div class="pls-bottom">
            <!-- Profile Learn Content Subpage -->
        </div>
            
    </div>
</template>



<script>
import { GetRequest } from '@/js/services/request';



export default {
    name: 'ProfileLearnSubpage',
    components: {
        
    },
    props: {
    },
    data(){
        return {
            isMySelf: false,
            listFilteredCourse: null,
            listCourse: [],
        }
    },
    async created(){
        try {
            this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    async mounted(){
    },
    methods: {
        async resolveOnchangeConfig(config){
            try {
                let { text, isLatest } = config;

            } catch (error){
                console.error(error);
            }
        },

        async refresh(){
            try {
                this.isMySelf = await this.getIsMySelf();
                this.listFilteredCourse = null;
                let res = await new GetRequest('')
            } catch (error){
                console.error(error);
            }
        },

        similitary(course, text){

        }
    },
    inject: {
        getIsMySelf: {}
    }
}

</script>


<style scoped>

.profile-learn-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.profile-learn-subpage > div{
    width: 100%;
}

</style>

