

<template>
    <div class="p-search-course-by-category-content">
        <CourseSubpage :get-course="getCourseCallback" />
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
                    let url = 'Categories/courses/' + this.searchKey;
                    let currentUser = await this.getCurrentUser();
                    if (currentUser == null){
                        url = 'Categories/anonymous/courses/' + this.searchKey;
                    }

                    // call request
                    let res = await new Request(url)
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
        '$route.query.search': {
            handler: 'resolveChangeSearchKey',
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



