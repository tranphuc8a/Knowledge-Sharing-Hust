

<template>
    <div class="p-course-member-content">
        <CourseRevitationContent 
            :course-role-type="courseRoleType"
            :get-more-user="getMoreUserCallback"
        />
    </div>
</template>



<script>
import CourseRevitationContent from './CourseRevitationContent.vue';
import { myEnum } from '@/js/resources/enum';
import { Validator } from '@/js/utils/validator';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'CourseMemberContent',
    components: {
        CourseRevitationContent
    },
    props: {
    },
    data(){
        return {
            courseRoleType: myEnum.ECourseRoleType.Member,
            getMoreUserCallback: null,
        }
    },
    async created(){
        this.refresh();
    },
    async mounted(){
    },
    methods: {
        async resolveOnClickSearch(searchText){
            try {
                // change callback:
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;

                if (Validator.isNotEmpty(searchText)){
                    // search registers:
                    let url = "CourseRelations/search/registers/" + courseId;
                    this.getMoreUserCallback = this.getCallbackWithUrl(url, {search: searchText}).bind(this);
                } else {
                    // get registers
                    let url = "CourseRelations/registers/" + courseId;
                    this.getMoreUserCallback = this.getCallbackWithUrl(url).bind(this);
                }
            } catch (e) {
                console.error(e);
            }
        },

        async refresh(){
            try {
                if (this.registerOnClickSearch != null){
                    this.registerOnClickSearch(this.resolveOnClickSearch.bind(this));
                }
                this.courseRoleType = myEnum.ECourseRoleType.Member;
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                
                // prepare Url
                let url = "CourseRelations/registers/" + courseId;
                this.getMoreUserCallback = this.getCallbackWithUrl(url);
            } catch (e) {
                console.error(e);
            }
        },

        getCallbackWithUrl(url, params = {}){
            return async function(limit, offset){
                let res = await new GetRequest(url).setParams({
                        ...params,
                        limit: limit,
                        offset: offset
                    }).execute();
                return res;
            }
        }
    },
    inject: {
        getCourse: {},
        registerOnClickSearch: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-member-content {
    width: 100%;
}

</style>

