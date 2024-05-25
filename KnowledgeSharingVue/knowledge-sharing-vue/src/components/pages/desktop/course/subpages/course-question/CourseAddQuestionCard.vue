<template>
    <FeedCardFrame>
        <div class="p-course-addquestion">
            <div class="p-course-addquestion__header">
                <TooltipUserAvatar :user="currentUser" :size="36" />
                <div class="p-course-addquestion__reminder">
                    Chào {{ currentUser?.FullName?? "bạn" }}, bạn có thắc mắc gì về khóa học này không, hãy tạo một bài thảo luận ngay nào!
                </div>
            </div>
            <div class="p-course-devide"></div>
            <div class="p-course-buttons">
                <MEmbeddedButton 
                    fa="comments"
                    label="Tạo bài thảo luận trong khóa học"
                    :onclick="resolveClickAddQuestion"
                    :buttonStyle="buttonStyle"
                    :iconStyle="iconStyle"
                />
            </div>
        </div>
    </FeedCardFrame>
</template>

<script>

import FeedCardFrame from '../../../home/components/feed-subpage/postcard/FeedCardFrame.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton';
import CurrentUser from '@/js/models/entities/current-user';
import { useRouter } from 'vue-router';

export default {
    name: "CourseAddQuestionCard",
    data() {
        return {
            label: null,
            buttonStyle: {
                color: 'var(--grey-color-600)',
            },
            iconStyle: {
                color: 'var(--grey-color)',
            },
            currentUser: null,
            router: useRouter(),
        }
    },
    async mounted() {
        this.currentUser = await CurrentUser.getInstance();
    },
    components: {
        FeedCardFrame, TooltipUserAvatar, MEmbeddedButton
    },
    methods: {

        async resolveClickAddQuestion(){
            try {
                let courseId = this.getCourse().UserItemId;
                if (courseId == null) return;
                this.router.push('/question-create?courseId=' + courseId);
            } catch (error) {
                console.log(error);
            }
        }
        
    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {},
        getCourse: {},
    }
}
</script>

<style scoped>
.p-course-addquestion{
    padding: 16px 16px;
    box-sizing: border-box;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.p-course-addquestion__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.p-course-addquestion__reminder{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;

    font-weight: 500;
    flex: 1;
    text-align: justify;
    font-size: 16px;
    min-height: 42px;
    border-radius: 32px;
    background-color: var(--grey-color-200);
    padding: 8px 16px;
    box-sizing: border-box;
    cursor: pointer;
    color: var(--grey-color-600);
}
.p-course-addquestion__reminder:hover{
    background-color: var(--grey-color-300);
    color: var(--blue-grey-color-800);
}

.p-course-devide{
    width: 100%;
    height: 1px;
    background-color: var(--primary-color-200);
}

.p-course-buttons{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-course-buttons .p-button{
    border-radius: 8px;
}
</style>