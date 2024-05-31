

<template>
    <DesktopHomeFrame>
        <div class="d-content p-course" 
            v-if="!isLoaded"
            >
            <div class="p-course-page">
                <div class="p-course-panel card">
                    <CoursePanel />
                </div>
            </div>
        </div>

        <div class="d-content p-course" v-if="isLoaded"
            @:scroll="throttleResolveOnScroll"
            ref="page"
        >
            <div class="d-empty-panel" v-show="isCourseExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="p-course-page" v-show="isCourseExisted === true">
                <div class="p-course-panel card">
                    <CoursePanel />
                </div>

                <router-view>
                </router-view>
            </div>
        </div>
    </DesktopHomeFrame>
</template>



<script>

import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CurrentUser from '@/js/models/entities/current-user';
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import CoursePanel from './components/course-panel/CoursePanel.vue';
import { useRoute, useRouter } from 'vue-router';
import { GetRequest, Request } from '@/js/services/request';
import { Validator } from '@/js/utils/validator';
import Debounce from '@/js/utils/debounce';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';

export default {
    name: 'CoursePage',
    components: {
        DesktopHomeFrame,
        CoursePanel, NotFoundPanel
    },
    props: {
    },
    async created(){
        this.createPage();
    },
    data(){
        return {
            isLoaded: true,
            isCourseExisted: null,
            course: null,
            currentUser: null,
            route: useRoute(),
            router: useRouter(),
            errorMessage: '',
            defaultErrorMessage: 'Khóa học không tồn tại',
            handlers: [],
        }
    },
    mounted(){
        this.debounceResolveOnScroll = this.debounce(
            this.resolveOnScroll, 
            500, 
            {
                leading: true,
                trailing: true
            }
        );
        this.throttleResolveOnScroll = this.throttle(
            this.resolveOnScroll, 
            1000
        );
    },
    provide(){
        return {
            getCourse: () => this.course,
            getIsMyCourse: this.isMyCourse,
            registerScrollHandler: this.registerScrollHandler,
            refreshCoursePage: this.createPage,
        }
    },
    methods: {
        // for scroll:
        debounce: Debounce.debounce,
        throttle: Debounce.throttle,
        registerScrollHandler(handler){
            this.handlers.push(handler);
        },
        async resolveOnScroll(){
            try {
                let page = this.$refs.page;
                await Promise.all(this.handlers.map(handler => handler(page)));
            } catch (e) {
                console.error(e);
            }
        },

        // init and rerender course page
        async createPage(){
            try {
                this.course = null;
                this.isLoaded = false;

                let courseId = this.route.params.courseId;
                if (Validator.isEmpty(courseId)) {
                    this.isCourseExisted = false;
                    return;
                }

                this.currentUser = await CurrentUser.getInstance();
                let url = 'Courses/' + courseId;
                if (this.currentUser == null) {
                    url = 'Courses/anonymous/' + courseId;
                }

                let res = await new GetRequest(url)
                    .execute();
                let body = await Request.tryGetBody(res);

                this.course = new ResponseCourseModel().copy(body);
                this.isCourseExisted = true;
                this.forceRender();
            } catch (error) {
                try {
                    Request.resolveAxiosError(error);
                    let userMsg = await Request.tryGetUserMessage(error);
                    this.errorMessage = userMsg ?? this.defaultErrorMessage;
                    this.isCourseExisted = false;
                    this.course = null;
                } catch (e){
                    console.error(e);
                }
            } finally {
                this.isLoaded = true;
            }
        },

        async forceRender(){
            try {
                this.isLoaded = false;
                this.$nextTick(() => {
                    this.isLoaded = true;
                });
            } catch (e) {
                console.error(e);
            }
        },

        async isMyCourse(){
            let currentUser = await CurrentUser.getInstance();
            if (currentUser?.UserId != null){
                return currentUser.UserId == this.course?.UserId;
            }
            return false;
        },


        async navigateNewCourse(){
            try {
                this.getLoadingPanel().show();
                
                let courseId = this.route.params.courseId;
                let url = 'Courses/' + courseId;
                if (this.currentUser == null) {
                    url = 'Courses/anonymous/' + courseId;
                }
                let res = await new GetRequest(url)
                    .execute();
                
                let body = await Request.tryGetBody(res);
                this.course = null;
                this.isLoaded = false;
                this.user = new ResponseCourseModel().copy(body);
                this.$nextTick(() => {
                    this.isLoaded = true;
                });
                // this.isCourseExisted = true;
            } catch (error){
                try {
                    Request.resolveAxiosError(error);
                    let userMsg = await Request.tryGetUserMessage(error);
                    this.errorMessage = userMsg ?? this.defaultErrorMessage;
                    this.isCourseExisted = false;
                    this.user = null;
                } catch (e){
                    console.error(e);
                }
            } finally {
                this.getLoadingPanel().hide();
            }
        }
    },
    watch: {
        user(){
            this.forceRender();
        },
        '$route.params.courseId'(){
            this.navigateNewCourse();
        },
        '$route.fullPath'() {
            this.handlers = [];
        }
    },
    inject: {
        getPopupManager: {},
        getLoadingPanel: {}
    }
}

</script>


<style>

.p-course .p-course-subpage{
    max-width: var(--course-page-max-width);
    width: 100%;
    height: fit-content;
}

</style>


<style scoped>
.d-content.p-course{
    padding-top: 0;
    justify-content: center;
}
.p-course-page{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
}
.p-course-panel{
    width: 100%;
}

</style>

