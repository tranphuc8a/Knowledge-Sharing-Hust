
import CoursePage from "@/components/pages/desktop/course/CoursePage.vue"


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
                component: null,
            },
            {
                path: 'introduction',
                name: 'course-introduction-page',
                component: null,
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
                component: null,
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

