<template>
    <DesktopHomeFrame>
        
        <div class="d-content p-lestion-detail">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="d-content-subpage__menu" v-show="isLessonExisted === true">
                <div class="p-course-navigation">
                    <div class="p-back-to-course" @:click="resolveBackToCourse">
                        Trở lại khóa học
                    </div>
                    <div class="p-next-previous-lesson">
                        <div class="p-previous-lesson" v-if="isHasPrevious"
                            @:click="resolvePreviousLesson">
                            Bài trước
                        </div>
                        <div class="p-next-lesson" v-if="isHasNext"
                            @:click="resolveNextLesson">
                            Bài sau
                        </div>
                    </div>
                </div>
                <div class="menu-card">
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
            courseId: null,
            lessonOffset: null,
            isHasNext: false,
            isHasPrevious: false,
        }
    },
    components: {
        NotFoundPanel, DesktopHomeFrame, LessonCard, MarkdownToc
    },
    async created() {
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
                    offset: this.lessonOffset
                })
                .execute();
            let body = await Request.tryGetBody(res);
            // get lessons detail 
            this.lesson.copy(body);
            this.isLessonExisted = true;
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
    methods: {
        async resolveBackToCourse(){
            try {
                this.router.push({name: 'course-detail', params: {courseId: this.courseId}});
            } catch (e) {
                console.error(e);
            }
        },

        async resolvePreviousLesson(){
            try {
                this.router.push('/course-lesson/' + this.courseId + '/' + (this.lessonOffset - 1));
            } catch (e) {
                console.error(e);
            }
        },

        async resolveNextLesson(){
            try {
                this.router.push('/course-lesson/' + this.courseId + '/' + (this.lessonOffset + 1));
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
    }
}
</script>

<style scoped>

@import url(@/css/pages/desktop/components/lestion-detail.css);

</style>