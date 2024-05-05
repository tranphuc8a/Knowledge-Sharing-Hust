<template>
    <DesktopHomeFrame>
        
        <div class="d-content p-lestion-detail">
            <div class="d-empty-panel" v-show="isQuestionExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="d-content-subpage__menu" v-show="isQuestionExisted === true">
                <div class="menu-card">
                    <MarkdownToc :markdownContent="question?.Content" />
                </div>
            </div>

            <div class="d-content-subpage__content" v-if="isQuestionExisted === true">
                <div class="lestion-card">
                    <QuestionCard :post="question" />
                </div>
            </div>
        </div>
        
        
    </DesktopHomeFrame>
</template>

<script>

import MarkdownToc from '@/components/base/markdown/MarkdownToc.vue';
import QuestionCard from './components/QuestionCard.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import { useRoute } from 'vue-router';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest, Request } from '@/js/services/request';
import ResponseQuestionModel from '@/js/models/api-response-models/response-question-model';

export default {
    name: "QuestionDetailPage",
    data() {
        return {
            errorMessage: "Bài thảo luận hiện không tồn tại hoặc đã bị xóa",
            defaultErrorMessage: "Bài thảo luận hiện không tồn tại hoặc đã bị xóa",
            title: "Question Detail Page",
            question: null,
            isQuestionExisted: null,
            route: useRoute(),
            currentUser: null
        }
    },
    components: {
        NotFoundPanel, DesktopHomeFrame, QuestionCard, MarkdownToc
    },
    async created() {
        try {
            this.getLoadingPanel().show();
            this.currentUser = await CurrentUser.getInstance();
            this.questionId = this.route.params.questionId;
            let url = null;

            if (this.currentUser == null) {
                url = `Questions/anonymous/${this.questionId}`;
            } else {
                url = `Questions/${this.questionId}`;
            }

            let res = await new GetRequest(url).execute();
            let body = await Request.tryGetBody(res);
            this.question = new ResponseQuestionModel();
            this.question.copy(body);
            this.isQuestionExisted = true;
        }
        catch (error) {
            try {
                Request.resolveAxiosError(error);
                let userMsg = await Request.tryGetUserMessage(error);
                this.errorMessage = userMsg ?? this.defaultErrorMessage;
                this.isQuestionExisted = false;
                this.question = null;
            } catch (e){
                console.error(e);
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

@import url(@/css/pages/desktop/components/lestion-detail.css);

</style>