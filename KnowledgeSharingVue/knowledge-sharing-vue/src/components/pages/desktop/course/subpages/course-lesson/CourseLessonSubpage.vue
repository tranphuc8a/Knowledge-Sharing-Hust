

<template>
    <div class="p-course-lesson-subpage p-course-subpage">
        <div class="p-cls-card card">

            <!-- Card header -->
            <div class="p-cls-card-header card-header">
                <div class="p-pls-heading card-heading">
                    <span> Danh sách bài giảng </span>
                </div>
                <div class="p-pls-header-button">
                    <span v-show="isMySelf">
                        <MSecondaryButton 
                            fa="pencil" label="Cập nhật danh sách bài giảng"
                            :onclick="resolveClickUpdateLesson" />
                    </span>
                </div>
            </div>

            <!-- Card toolbar -->
            <div class="p-cls-card-toolbar" v-show="false">
            </div>

            <div class="p-devide"></div>

            <!-- Card content -->
            <div class="p-pls-content">
                <div class="p-pls-not-found" v-show="isOutOfLesson && !(listCourseLesson.length > 0)">
                    <NotFoundPanel text="Không tìm thấy bài giảng nào" />
                </div>

                <div class="p-pls-course-lesson-item"
                    v-for="(courseLesson, index) in listCourseLesson"
                    :key="courseLesson.CourseLessonId ?? index"
                >
                    <CourseLessonCard :responseCourseLessonModel="courseLesson" />
                </div>

                <div class="p-pls-course-lesson-item" v-show="!isOutOfLesson">
                    <CourseLessonCardSkeleton />
                </div>
                <div class="p-pls-course-lesson-item" v-show="!isOutOfLesson">
                    <CourseLessonCardSkeleton />
                </div>
                <div class="p-pls-course-lesson-item" v-show="!isOutOfLesson">
                    <CourseLessonCardSkeleton />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { GetRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue';
import CourseLessonCard from './../../../../../base/cards/CourseLessonCard.vue';
import CourseLessonCardSkeleton from './../../../../../base/cards/CourseLessonCardSkeleton.vue'
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import ResponseCourseLessonModel from '@/js/models/api-response-models/response-course-lesson-model';
import { useRouter } from 'vue-router';

export default {
    name: 'CourseLessonSubpage',
    components: {
        MSecondaryButton,
        CourseLessonCard,
        CourseLessonCardSkeleton,
        NotFoundPanel
    },
    props: {
    },
    data(){
        return {
            dCourse: null,
            isMySelf: false,
            listCourseLesson: [],
            isOutOfLesson: false,
            isLoadingMore: false,
            router: useRouter(),
        }
    },
    async created(){
        await this.refreshListLesson();
    },
    async mounted(){
        try {
            this.registerScrollHandler(this.resolveOnScroll.bind(this));
        } catch (error) {
            console.error(error);
        }
    },
    methods: {
        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;
                if (this.isOutOfLesson || this.isLoadingMore){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averagePostHeight = 200;
                let leftPostNumber = 6;

                if (scrollHeight - scrollPosition < averagePostHeight * leftPostNumber){
                    // console.log("Load more post");
                    this.loadMoreLesson();
                }
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreLesson(){
            if (this.isLoadingMore) return;
            try {
                this.isLoadingMore = true;
                // prepare request
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                let url = 'CourseLessons/' + courseId;
                let limit = 10;
                let offset = this.listCourseLesson.length;

                // send request
                let res = await new GetRequest(url)
                    .setParams({ limit, offset })
                    .execute();

                // read response
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;
                if (body.length < limit){
                    this.isOutOfLesson = true;
                }
                let tempCourseLessonModels = body.map(function(item){
                    return new ResponseCourseLessonModel().copy(item);
                });
                if (tempCourseLessonModels.length > 0){
                    this.listCourseLesson = this.listCourseLesson.concat(tempCourseLessonModels);
                }
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isLoadingMore = false;
            }
        },

        async refreshListLesson(){
            try {
                this.dCourse = await this.getCourse();
                this.isMySelf = await this.getIsMyCourse();
            } catch (error) {
                console.error(error);
            }
        }, 

        async resolveClickUpdateLesson(){
            try {
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                this.router.push('/course/' + courseId + '/course-update');
            } catch (error) {
                console.error(error);
            }
        }
    },
    inject: {
        getCourse: {},
        getIsMyCourse: {},
        registerScrollHandler: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-lesson-subpage{
    min-width: 50%;
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    padding-bottom: 32px;
}

.p-course-lesson-subpage .p-cls-card{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    padding: 16px;
    gap: 16px;
}

.p-course-lesson-subpage .p-cls-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-course-lesson-subpage .p-cls-card-toolbar{
    width: 100%;
    height: 50px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}


.p-course-lesson-subpage .p-cls-card-header .p-pls-header-button{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
}

.p-course-lesson-subpage .p-pls-content{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

.p-course-lesson-subpage .p-pls-course-lesson-item{
    width: 100%;
    height: auto;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-course-lesson-subpage .p-pls-not-found{
    width: 100%;
    height: 250px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

</style>

