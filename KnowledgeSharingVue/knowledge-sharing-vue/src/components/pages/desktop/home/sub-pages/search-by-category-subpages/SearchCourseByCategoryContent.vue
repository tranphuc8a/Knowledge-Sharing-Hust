

<template>
    <div class="p-search-course-by-category-content">
        <CourseSubpage :get-course="getCourseCallback" :rowCount="2" />
    </div>
</template>



<script>
import CourseSubpage from '../../components/course-subpage/CourseSubpage.vue';
import { Validator } from '@/js/utils/validator';
import { GetRequest } from '@/js/services/request';


export default {
    name: 'SearchCourseByCategoryContent',
    components: {
        CourseSubpage,
    },
    props: {
    },
    data(){
        return {
            getCourseCallback: null,
            category: '',
        }
    },
    async mounted(){
    },
    methods: {
        async resolveChangeCategory(){
            try {
                this.category = this.$route.query.category;
                this.getCourseCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset, category
                    if (Validator.isEmpty(this.category)) return {
                        Body: [],
                    }
                    let url = 'Categories/courses/' + this.category;
                    let currentUser = await this.getCurrentUser();
                    if (currentUser == null){
                        url = 'Categories/anonymous/courses/' + this.category;
                    }

                    // call request
                    let res = await new GetRequest(url)
                        .setParams({ limit: limit, offset: offset })
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
        },
    },
    watch: {
        '$route.query.category': {
            handler: 'resolveChangeCategory',
            immediate: true,
        },
    },
    inject: {
        getCurrentUser: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-search-course-by-category-content{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>



