

<template>
    <div class="p-administrator-list-course-content">
        <div class="p-alcc-result-card card">
            <div class="p-alcc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-alcc-result">
                <div class="p-alcc-result-notfound" v-if="isOutOfCourse && listCourse.length == 0" >
                    <NotFoundPanel 
                        text="Không tìm thấy khóa học nào" 
                    />
                </div>
                <div class="p-alcc-result-list">
                    <CourseAdministratorShortCard v-for="course in listCourse"
                        :key="course.UserItemId"
                        :course="course"
                    /> 

                    <CourseShortCardSkeleton v-if="!isOutOfCourse" />
                    <CourseShortCardSkeleton v-if="!isOutOfCourse" />
                    <CourseShortCardSkeleton v-if="!isOutOfCourse" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CourseAdministratorShortCard from '../../components/administrator/admin-useritem/CourseAdministratorShortCard.vue';
import CourseShortCardSkeleton from '@/components/pages/desktop/course/components/course-card/CourseShortCardSkeleton.vue'
import { useRoute } from 'vue-router';
import { Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import ViewCourse from '@/js/models/views/view-course';

export default {
    name: 'AdministratorListCourseContent',
    components: {
        NotFoundPanel,
        CourseAdministratorShortCard,
        CourseShortCardSkeleton,
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            listCourse: [],
            searchKey: '',
            isLoaded: false,
            isOutOfCourse: false,
            isWorking: false,
        }
    },
    async created(){
    },
    async mounted(){
        this.createdPage();
        this.registerScrollHandler(this.resolveOnScroll.bind(this));
    },
    methods: {
        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;
                if (this.isOutOfCourse || this.isWorking){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageCourseHeight = 450;
                let leftCourseNumber = 3;

                if (scrollHeight - scrollPosition < averageCourseHeight * leftCourseNumber){
                    // console.log("Load more post");
                    this.loadMoreCourse();
                }
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickShowQuery(){
            console.log(this.route.query);
            console.log(this.$route.query);
        },

        async createdPage(){
            try {
                this.isLoaded = false;
                this.searchKey = this.route.query['search'];
                this.listCourse = [];
                this.isOutOfCourse = false;
                this.loadMoreCourse();
            } catch (e){
                console.error(e);
            } finally {
                this.isLoaded = true;
            }
        },

        async loadMoreCourse(){
            if (this.isWorking || this.isOutOfCourse) return;
            try {
                this.isWorking = true;

                // prepare request
                let url = 'Courses/search';
                if (Validator.isEmpty(this.searchKey)){
                    this.listCourse = [];
                    this.isOutOfCourse = true;
                    return;
                }
                let limit = 20;
                let offset = this.listCourse.length;

                // call request
                let res = await new GetRequest(url)
                    .setParams({search: this.searchKey, limit: limit, offset: offset})
                    .execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // read data:
                if (body.length < limit){
                    this.isOutOfCourse = true;
                }
                if (body.length > 0){
                    let tempCourses = body.map(function(tCourse){
                        return new ViewCourse().copy(tCourse);
                    });
                    this.listCourse = this.listCourse.concat(tempCourses);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        }
    },
    watch: {
        '$route.query.search': {
            handler(){
                this.createdPage();
            },
            deep: true,
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getCurrentUser: {},
        registerScrollHandler: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-administrator-list-course-content{
    max-width: 100%;
    width: 100%;
    padding-bottom: 32px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    position: relative;
    gap: 16px;
}

.p-alcc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-alcc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-alcc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-alcc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-alcc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

