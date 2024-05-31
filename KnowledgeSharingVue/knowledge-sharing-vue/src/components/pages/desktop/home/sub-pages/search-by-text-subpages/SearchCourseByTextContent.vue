

<template>
    <div class="p-search-course-by-text-content">
        <CourseSubpage :get-course="getCourseCallback" />
    </div>
</template>



<script>
import CourseSubpage from '../../components/course-subpage/CourseSubpage.vue';
import { Validator } from '@/js/utils/validator';
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
            searchKey: '',
        }
    },
    async mounted(){
    },
    methods: {
        async resolveChangeSearchKey(){
            try {
                this.searchKey = this.$route.query.search;
                this.getCourseCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset, searchKey
                    if (Validator.isEmpty(this.searchKey)) return {
                        Body: [],
                    }

                    // call request
                    let res = await new GetRequest('Courses/search')
                        .setParams({search: this.searchKey, limit: limit, offset: offset})
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
        },
    },
    watch: {
        '$route.query.search': {
            handler: 'resolveChangeSearchKey',
            immediate: true,
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

.p-search-course-by-text-content{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>



