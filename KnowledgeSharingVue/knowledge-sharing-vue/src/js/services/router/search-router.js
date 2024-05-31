
import SearchByTextSubpage from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchByTextSubpage.vue";
import SearchByCategorySubpage from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchByCategorySubpage.vue";

import SearchUserByTextContent from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchUserByTextContent.vue";
import SearchPostByTextContent from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchPostByTextContent.vue";
import SearchLessonByTextContent from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchLessonByTextContent.vue";
import SearchQuestionByTextContent from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchQuestionByTextContent.vue";
import SearchCourseByTextContent from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchCourseByTextContent.vue";

import SearchPostByCategoryContent from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchPostByCategoryContent.vue";
import SearchLessonByCategoryContent from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchLessonByCategoryContent.vue";
import SearchQuestionByCategoryContent from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchQuestionByCategoryContent.vue";
import SearchCourseByCategoryContent from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchCourseByCategoryContent.vue";


export default [
    {
        path: '/search-text',
        name: 'search-text-page',
        component: SearchByTextSubpage,
        meta: {
            requiredAuth: false
        },
        children: [
            {
                path: '',
                name: 'search-text-home',
                component: SearchPostByTextContent,
            },
            {
                path: 'user',
                name: 'search-text-user',
                component: SearchUserByTextContent,
            },
            {
                path: 'post',
                name: 'search-text-post',
                component: SearchPostByTextContent,
            },
            {
                path: 'lesson',
                name: 'search-text-lesson',
                component: SearchLessonByTextContent
            },
            {
                path: 'course',
                name: 'search-text-course',
                component: SearchCourseByTextContent
            },
            {
                path: 'question',
                name: 'search-text-question',
                component: SearchQuestionByTextContent
            },
        ]
    },
    {
        path: '/search-category',
        name: 'search-category-page',
        component: SearchByCategorySubpage,
        meta: {
            requiredAuth: false
        },
        children: [
            {
                path: '',
                name: 'search-category-home',
                component: SearchPostByCategoryContent
            },
            {
                path: 'post',
                name: 'search-category-post',
                component: SearchPostByCategoryContent
            },
            {
                path: 'lesson',
                name: 'search-category-lesson',
                component: SearchLessonByCategoryContent
            },
            {
                path: 'course',
                name: 'search-category-course',
                component: SearchCourseByCategoryContent
            },
            {
                path: 'question',
                name: 'search-category-question',
                component: SearchQuestionByCategoryContent
            },
        ]
    }
];