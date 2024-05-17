

import DesktopHomePage from '@/components/pages/desktop/home/DesktopHomePage.vue';
import HomeFeedSubpage from '@/components/pages/desktop/home/sub-pages/HomeFeedSubpage.vue';
import HomeLessonSubpage from '@/components/pages/desktop/home/sub-pages/HomeLessonSubpage.vue';
import HomeQuestionSubpage from '@/components/pages/desktop/home/sub-pages/HomeQuestionSubpage.vue';
import HomeCourseSubpage from '@/components/pages/desktop/home/sub-pages/HomeCourseSubpage.vue';

const homepageRouter = [
    {
        path: '/',
        name: 'home',
        component: DesktopHomePage,
        meta: {
            requiredAuth: true
        },
        children: [ { // when /
            path: '',
            name: 'feed',
            component: HomeFeedSubpage
        }, { // when /feed
            path: 'feed',
            name: 'feed-page',
            component: HomeFeedSubpage
        }, { // when /lessons
            path: 'lessons',
            name: 'lessons-page',
            component: HomeLessonSubpage
        }, { // when /courses
            path: 'courses',
            name: 'courses-page',
            component: HomeCourseSubpage
        }, { // when /questions
            path: 'questions',
            name: 'questions-page',
            component: HomeQuestionSubpage
        }],
    },
];

export default homepageRouter;