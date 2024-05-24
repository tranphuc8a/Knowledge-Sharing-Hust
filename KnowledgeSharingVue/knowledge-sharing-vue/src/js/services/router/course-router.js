
import CoursePage from "@/components/pages/desktop/course/CoursePage.vue";

import CourseIntroductionSubpage from "@/components/pages/desktop/course/subpages/course-introduction/CourseIntroductionSubpage.vue";
import CourseRevitationSubpage from "@/components/pages/desktop/course/subpages/course-revitation/CourseRevitationSubpage.vue";

import CourseRequestContent from "@/components/pages/desktop/course/subpages/course-revitation/CourseRequestContent.vue";
import CourseInviteContent from "@/components/pages/desktop/course/subpages/course-revitation/CourseInviteContent.vue";
import CourseMemberContent from "@/components/pages/desktop/course/subpages/course-revitation/CourseMemberContent.vue";


export default [
    {
        path: '/course/:courseId',
        name: 'course',
        component: CoursePage,
        meta: {
            requiredAuth: false
        },
        children: [
            {
                path: '',
                name: 'course-home-page',
                component: CourseIntroductionSubpage,
            },
            {
                path: 'introduction',
                name: 'course-introduction-page',
                component: CourseIntroductionSubpage,
            },
            {
                path: 'lesson',
                name: 'course-lesson-page',
                component: null,
            },
            {
                path: 'question',
                name: 'course-question-page',
                component: null,
            },
            {
                path: 'register',
                name: 'course-register-page',
                component: null,
            },
            {
                path: 'revite',
                name: 'course-request-invite-page',
                component: CourseRevitationSubpage,
                children: [
                    {
                        path: 'request',
                        name: 'course-revite-request-page',
                        component: CourseRequestContent,
                    },
                    {
                        path: 'invite',
                        name: 'course-revite-invite-page',
                        component: CourseInviteContent,
                    },
                    {
                        path: 'member',
                        name: 'course-revite-member-page',
                        component: CourseMemberContent,
                    },
                ]
            },
            {
                path: 'payment',
                name: 'course-payment-page',
                component: null,
            },
            {
                path: 'edit',
                name: 'course-edit-page',
                component: null,
            },
            {
                path: 'lesson-edit',
                name: 'course-lesson-edit-page',
                component: null,
            }
        ]
    }
]

