<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel text="Bài giảng hiện không tồn tại hoặc đã bị xóa" />
            </div>

            <div class="d-content-subpage__menu" v-show="isLessonExisted === true">
                <MarkdownToc :markdownContent="lesson?.Content" />
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
            currentUser: null
        }
    },
    components: {
        NotFoundPanel, DesktopHomeFrame, LessonCard, MarkdownToc
    },
    async created() {
        try {
            this.getLoadingPanel().show();
            this.currentUser = CurrentUser.getInstance();
            this.lessonId = this.route.params.lessonId;
            let url = null;

            if (this.currentUser == null) {
                url = `Lessons/anonymous/${this.lessonId}`;
            } else {
                url = `Lessons/${this.lessonId}`;
            }

            let res = await new GetRequest(url).execute();
            let body = await Request.tryGetBody(res);
            this.lesson = new ResponseLessonModel();
            this.lesson.copy(body);
            this.lesson.User = this.lesson.getUser();
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

.d-empty-panel{
    width: 100%;
    height: 100%;

    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
}

.d-content-subpage__menu{
    flex-shrink: 0;
    flex-grow: 0;
    height: 100%;
    position: sticky;
    top: 0;
    overflow: auto;
}

.d-content-subpage__content{
    padding: 0px 24px 24px 24px;
}

@media screen and (min-width: 1464px){
    .d-content-subpage__menu
    {
        width: var(--subpage-max-width);
    }
}

@media screen and (min-width: 1104px) and (max-width: 1464px){
    .d-content-subpage__menu
    {
        width: var(--subpage-min-width);
    }
}

@media screen and (max-width: 1104px){
    .d-content-subpage__menu
    {
        width: var(--subpage-min-width);
    }
}
</style>