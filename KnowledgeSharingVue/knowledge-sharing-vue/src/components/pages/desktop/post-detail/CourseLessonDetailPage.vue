<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel :text="errorMessage" />
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
    name: "LessonDetailPage",
    data() {
        return {
            title: "Lesson Detail Page",
            lesson: null,
            isLessonExisted: null,
            route: useRoute(),
            currentUser: null,
            errorMessage: 'Bài giảng hiện không tồn tại hoặc đã bị xóa',
            courseId: null,
            lessonOffset: null,
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
            // get all lessons of courses
            let url = '';

            // get lesson of position offset
            let res = await new GetRequest(url).execute();
            let body = await Request.tryGetBody(res);
            this.lesson = new ResponseLessonModel();
            this.lesson.copy(body);
            this.isLessonExisted = true;
        }
        catch (error) {
            try {
                this.lesson = null;
                this.isLessonExisted = false;
                let userMessage = await Request.getUserMessage(error);
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