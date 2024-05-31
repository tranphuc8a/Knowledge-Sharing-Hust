

<template>
    <div class="p-profile-mark-course-content">
        <CourseSubpage :get-course="getCourseCallback" :rowCount="3" />
    </div>
</template>



<script>
import CourseSubpage from '@/components/pages/desktop/home/components/course-subpage/CourseSubpage.vue';
import { GetRequest } from '@/js/services/request';


export default {
    name: 'SearchCourseByTextContent',
    components: {
        CourseSubpage,
    },
    props: {
    },
    data(){
        return {
            getCourseCallback: null,
        }
    },
    async mounted(){
        await this.initContent();
    },
    methods: {
        async initContent(){
            try {
                this.getCourseCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset

                    // call request
                    let res = await new GetRequest('Marks/my/courses')
                        .setParams({ limit: limit, offset: offset})
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
        },
    },
    watch: {
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-profile-mark-course-content{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>



