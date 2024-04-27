<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel text="Bài giảng hiện không tồn tại hoặc đã bị xóa" />
            </div>

            <div class="d-content-subpage__menu" v-show="isLessonExisted === true">
                <div class="menu-card">
                    <MarkdownToc :markdownContent="lesson?.Content" />
                </div>
            </div>

            <div class="d-content-subpage__content" v-if="isLessonExisted === true">
                <LessonCard :post="lesson" />
            </div>
        </div>
        
        
    </DesktopHomeFrame>
</template>

<script>

import MarkdownToc from '@/components/base/markdown/MarkdownToc.vue';
import LessonCard from './components/LessonCard.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import { useRoute } from 'vue-router';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest, Request } from '@/js/services/request';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';

export default {
    name: "CourseLessonDetailPage",
    data() {
        return {
            title: "CourseLesson Detail Page",
            lesson: null,
            isLessonExisted: null,
            route: useRoute(),
            currentUser: null,
            courseId: null,
            offset: null
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
                return this.getPopupManager().requiredLogin();
            }
            this.lessonId = this.route.params.courseId;
            this.offset = this.route.params.offset;

            let res = await new GetRequest('').execute();
            let body = await Request.tryGetBody(res);
            this.lesson = new ResponseLessonModel();
            this.lesson.copy(body);
            this.isLessonExisted = true;
        }
        catch (error) {
            this.lesson = null;
            this.isLessonExisted = false;
            console.error(error);
        } finally {
            this.getLoadingPanel().hide();
        }

    },
    methods: {

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

@import url(@/css/pages/desktop/components/lesson-detail.css);

</style>