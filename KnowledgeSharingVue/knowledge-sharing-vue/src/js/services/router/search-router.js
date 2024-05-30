
import SearchByTextSubpage from "@/components/pages/desktop/home/sub-pages/search-by-text-subpages/SearchByTextSubpage.vue";
import SearchByCategorySubpage from "@/components/pages/desktop/home/sub-pages/search-by-category-subpages/SearchByCategorySubpage.vue";

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
                component: null
            },
            {
                path: 'user',
                name: 'search-text-user',
                component: null
            },
            {
                path: 'post',
                name: 'search-text-post',
                component: null
            },
            {
                path: 'lesson',
                name: 'search-text-lesson',
                component: null
            },
            {
                path: 'course',
                name: 'search-text-course',
                component: null
            },
            {
                path: 'question',
                name: 'search-text-question',
                component: null
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
                component: null
            },
            {
                path: 'post',
                name: 'search-category-post',
                component: null
            },
            {
                path: 'lesson',
                name: 'search-category-lesson',
                component: null
            },
            {
                path: 'course',
                name: 'search-category-course',
                component: null
            },
            {
                path: 'question',
                name: 'search-category-question',
                component: null
            },
        ]
    }
];