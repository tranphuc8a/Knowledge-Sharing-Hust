
import LessonDetailPage from '@/components/pages/desktop/post-detail/LessonDetailPage.vue';
import CreateLessonPage from '@/components/pages/desktop/post-detail/CreateLessonPage.vue';
import EditLessonPage from '@/components/pages/desktop/post-detail/EditLessonPage.vue';
import CourseLessonDetailPage from '@/components/pages/desktop/post-detail/CourseLessonDetailPage.vue';
import CreateQuestionPage from '@/components/pages/desktop/post-detail/CreateQuestionPage.vue';
import QuestionDetailPage from '@/components/pages/desktop/post-detail/QuestionDetailPage.vue';
import EditQuestionPage from '@/components/pages/desktop/post-detail/EditQuestionPage.vue';


const lestionDecreaditRouter = [
    {
        path: '/lesson/:lessonId',
        name: 'lesson-detail',
        component: LessonDetailPage
    }, {
        path: '/course-lesson/:courseId/:offset',
        name: 'course-lesson',
        component: CourseLessonDetailPage
    }, {
        path: '/lesson-create',
        name: 'lesson-create',
        component: CreateLessonPage
    }, {
        path: '/lesson-edit/:lessonId',
        name: 'lesson-edit',
        component: EditLessonPage
    }, {
        path: '/question-create',
        name: 'question-create',
        component: CreateQuestionPage
    }, {
        path: '/question/:questionId',
        name: 'question-detail',
        component: QuestionDetailPage
    }, {
        path: '/question-edit/:questionId',
        name: 'question-edit',
        component: EditQuestionPage
    }
];

export default lestionDecreaditRouter;
