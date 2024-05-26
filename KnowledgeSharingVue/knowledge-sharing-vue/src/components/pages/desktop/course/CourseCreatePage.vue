

<template>
    <DesktopHomeFrame>
        <div class="p-course-create-page d-content">
            <!-- Create course subpage -->
    
            <CourseCreateSubpage />
        </div>
    </DesktopHomeFrame>
</template>



<script>
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import CourseCreateSubpage from './subpages/course-create/CourseCreateSubpage.vue';



export default {
    name: 'CourseCreatePage',
    components: {
        CourseCreateSubpage,
        DesktopHomeFrame
    },
    props: {
    },
    data(){
        return {
            
        }
    },
    async mounted(){
    },
    methods: {

    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style>
.p-course-create-page .p-course-subpage{
    max-width: var(--course-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.p-course-create-page{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

