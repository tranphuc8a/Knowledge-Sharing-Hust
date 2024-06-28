

<template>
    <div class="p-administrator-one-course-content">
        <div class="p-aocc-result-card card">
            <div class="p-aocc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-aocc-result">
                <div class="p-aocc-result-list" v-if="!isLoaded">
                    <CourseShortCardSkeleton />
                </div>
                <div class="p-aocc-result-list" v-if="isLoaded && isCourseExisted">
                    <CourseAdministratorShortCard :course="course"/>
                </div>
                <div class="p-aocc-result-notfound" v-if="isLoaded && !isCourseExisted" >
                    <NotFoundPanel 
                        :text="errorText" 
                    />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CourseAdministratorShortCard from '@/components/pages/desktop/home/components/administrator/admin-useritem/CourseAdministratorShortCard.vue'
import CourseShortCardSkeleton from '../../../course/components/course-card/CourseShortCardSkeleton.vue';
import { useRoute } from 'vue-router';
import { GetRequest, Request } from '@/js/services/request';
import ViewCourse from '@/js/models/views/view-course';

export default {
    name: 'AdministratorOneCourseContent',
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
            course: null,
            filter: '',
            isLoaded: false,
            isCourseExisted: false,
            errorText: 'Không tìm thấy khóa học',
        }
    },
    async created(){
    },
    async mounted(){
        this.createdPage();
    },
    methods: {

        async createdPage(){
            try {
                this.isLoaded = false;
                this.filter = this.route.query['filter'];
                let url = 'Courses/admin/' + this.filter;
                let res = await new GetRequest(url).execute();
                let body = await Request.tryGetBody(res);

                if (body == null){
                    this.isCourseExisted = false;
                    this.course = null;
                    return;
                }

                this.course = new ViewCourse().copy(body);
                this.isCourseExisted = true;
            } catch (e){
                let userMsg = await Request.tryGetUserMessage(e);
                if (userMsg != null){
                    this.errorText = userMsg;
                } else {
                    console.error(e);
                }
                this.isCourseExisted = false;
                this.course = null;
            } finally {
                this.isLoaded = true;
            }
        },

        resolveDeletedCourse(){
            this.createdPage();
        }
    },
    watch: {
        '$route.query.filter': {
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
    },
    provide(){
        return {
            onCourseDeleted: this.resolveDeletedCourse,
        }
    }
}

</script>


<style scoped>

.p-administrator-one-course-content{
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

.p-aocc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aocc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-aocc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aocc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-aocc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

