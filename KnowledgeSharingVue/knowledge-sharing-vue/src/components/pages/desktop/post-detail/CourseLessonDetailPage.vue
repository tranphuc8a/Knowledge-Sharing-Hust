<template>
    <DesktopHomeFrame>
        
        <div class="d-content p-lestion-detail" v-if="isLoaded">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="d-content-subpage__menu" v-show="isLessonExisted === true">
                <div class="menu-card">
                    <div class="p-course-navigation">
                        <div class="p-back-to-course" @:click="resolveBackToCourse">
                            Trở lại khóa học
                        </div>
                        <div class="p-next-previous-lesson">
                            <div class="p-previous-lesson" >
                                <div v-if="isHasPrevious" @:click="resolvePreviousLesson">
                                    <MIcon fa="chevron-left" :style="iconStyle"/>
                                    Bài trước
                                </div>
                            </div>
                            <div class="p-next-lesson" >
                                <div v-if="isHasNext" @:click="resolveNextLesson">
                                    Bài sau
                                    <MIcon fa="chevron-right" :style="iconStyle" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="p-devider">
                        <div></div>
                    </div>

                    <MarkdownToc :markdownContent="lesson?.Content" />
                </div>
            </div>

            <div class="d-content-subpage__content" v-if="isLessonExisted === true">
                <div class="lestion-card">
                    <LessonCard :post="lesson" />
                </div>
            </div>
        </div>
        
        
    </DesktopHomeFrame>
</template>

<script>

import MarkdownToc from '@/components/base/markdown/MarkdownToc.vue';
import LessonCard from './components/LessonCard.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import { useRoute, useRouter } from 'vue-router';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest, Request } from '@/js/services/request';
import ResponseCourseLessonModel from '@/js/models/api-response-models/response-course-lesson-model';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';
// import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';

export default {
    name: "CourseLessonDetailPage",
    data() {
        return {
            title: "Course Lesson Detail Page",
            lesson: null,
            isLessonExisted: null,
            route: useRoute(),
            router: useRouter(),
            currentUser: null,
            errorMessage: 'Bài giảng hiện không tồn tại hoặc đã bị xóa',
            defaultErrorMessage: 'Bài giảng hiện không tồn tại hoặc đã bị xóa',
            courseId: null,
            lessonOffset: null,
            isHasNext: false,
            isHasPrevious: false,
            courselesson: null,
            iconStyle: {
                fontSize: '14px'
            },
            isLoaded: false,
        }
    },
    components: {
        NotFoundPanel, DesktopHomeFrame, LessonCard, MarkdownToc
    },
    async created() {
        this.createPage();
    },
    methods: {
        async createPage(){
            try {
                this.getLoadingPanel().show();
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.courseId = this.route.params.courseId;
                this.lessonOffset = this.route.params.offset;
                // get course-lessons at offfset of courses
                let res = await new GetRequest('CourseLessons/' + this.courseId)
                    .setParams({
                        limit: 1,
                        offset: this.lessonOffset - 1
                    })
                    .execute();
                let body = await Request.tryGetBody(res);
                this.isHasPrevious = this.lessonOffset > 1;
                this.isHasNext = this.lessonOffset < body.Total;
                // get lessons detail
                let tempLesson = body.Results?.[0];
                if (tempLesson == null){
                    this.isLessonExisted = false;
                    this.errorMessage = this.defaultErrorMessage;
                    return;
                }
                this.courselesson = new ResponseCourseLessonModel().copy(tempLesson);
                this.lesson = this.courselesson.Lesson;
                this.isLessonExisted = true;
                this.refreshLesson();
            }
            catch (error) {
                try {
                    this.lesson = null;
                    this.isLessonExisted = false;
                    let userMessage = await Request.tryGetUserMessage(error);
                    if (userMessage != null) {
                        this.errorMessage = userMessage;
                    }
                    Request.resolveAxiosError(error);
                    console.error(error);
                } catch (error2){
                    console.error(error2);
                }
            } finally {
                this.getLoadingPanel().hide();
            }
        },

        async refreshLesson(){
            try {
                if (this.lesson == null){
                    return;
                }
                let res = await new GetRequest('Lessons/' + this.lesson.UserItemId)
                    .execute();
                let body = await Request.tryGetBody(res);
                let tempLesson = new ResponseLessonModel().copy(body);
                this.lesson = tempLesson;
                this.forceRender();
            } catch (e) {
                console.error(e);
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

        async resolveBackToCourse(){
            try {
                this.router.push('/course/' + this.courseId);
            } catch (e) {
                console.error(e);
            }
        },

        async resolvePreviousLesson(){
            try {
                await this.router.push('/course-lesson/' + this.courseId + '/' + Number(Number(this.lessonOffset) - 1));
            } catch (e) {
                console.error(e);
            }
        },

        async resolveNextLesson(){
            try {
                await this.router.push('/course-lesson/' + this.courseId + '/' + Number(Number(this.lessonOffset) + 1));
            } catch (e) {
                console.error(e);
            }
        }
    },
    props: {

    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {},
        getLoadingPanel: {}
    },
    watch: {
        '$route.params.offset'(newValue, oldValue) {
            if (newValue != oldValue) {
                this.createPage();
            }
        },
    },
}
</script>

<style scoped>

@import url(@/css/pages/desktop/components/lestion-detail.css);
@import url(@/css/pages/desktop/components/course-lesson.css);


</style>