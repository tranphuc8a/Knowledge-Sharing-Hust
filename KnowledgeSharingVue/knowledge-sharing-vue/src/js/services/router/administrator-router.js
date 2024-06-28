

import AdministratorSubpage from "@/components/pages/desktop/home/sub-pages/administrator/AdministratorSubpage.vue";

import AdministratorUserContent from "@/components/pages/desktop/home/sub-pages/administrator/AdministratorUserContent.vue";
import AdministratorPostContent from "@/components/pages/desktop/home/sub-pages/administrator/AdministratorPostContent.vue";
import AdministratorLessonContent from "@/components/pages/desktop/home/sub-pages/administrator/AdministratorLessonContent.vue";
import AdministratorQuestionContent from '@/components/pages/desktop/home/sub-pages/administrator/AdministratorQuestionContent.vue';
import AdministratorCourseContent from '@/components/pages/desktop/home/sub-pages/administrator/AdministratorCourseContent.vue';

export default [
    {
        path: '/administrator',
        name: 'administrator',
        component: AdministratorSubpage,
        meta: {
            requiredAuth: false
        },
        children: [
            {
                path: '',
                name: 'administrator-home',
                component: AdministratorPostContent,
            },
            {
                path: 'user',
                name: 'administrator-user',
                component: AdministratorUserContent,
            },
            {
                path: 'post',
                name: 'administrator-post',
                component: AdministratorPostContent,
            },
            {
                path: 'lesson',
                name: 'administrator-lesson',
                component: AdministratorLessonContent
            },
            {
                path: 'course',
                name: 'administrator-course',
                component: AdministratorCourseContent
            },
            {
                path: 'question',
                name: 'administrator-question',
                component: AdministratorQuestionContent
            },
        ]
    },
];