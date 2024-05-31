

<template>
    <div class="p-home-course-subpage">
        <CourseSubpage 
                :get-course="getCourse"
                :rowCount="2"
            >
                <template #addcourse>
                    <AddCourseFeedCard />
                </template>
        </CourseSubpage>
    </div>
</template>



<script>
import CourseSubpage from '../components/course-subpage/CourseSubpage.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest } from '@/js/services/request';
import AddCourseFeedCard from '../components/course-subpage/AddCourseFeedCard.vue';

export default {
    name: 'HomeCourseSubpage',
    components: {
        CourseSubpage,
        AddCourseFeedCard
    },
    props: {
    },
    data(){
        return {
        }
    },
    methods: {
        async getCourse(limit, offset){
            let currentUser = await CurrentUser.getInstance();
            let url = "Courses";
            if (currentUser == null){
                url = "Courses/anonymous";
            }
            let res = new GetRequest(url)
                .setParams({
                    limit: limit,
                    offset: offset
                })
                .execute();
            return res;
        }
    },
}

</script>

<style scoped>

.p-home-course-subpage{
    width: 100%;
    padding-bottom: 32px;
}


</style>

