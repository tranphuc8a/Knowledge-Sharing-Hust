

import DesktopHomePage from '@/components/pages/desktop/home/DesktopHomePage.vue';
import FeedSubPage from '@/components/pages/desktop/home/sub-pages/FeedSubPage.vue';


const homepageRouter = [
    {
        path: '/',
        name: 'home',
        component: DesktopHomePage,
        meta: {
            requiredAuth: true
        },
        children: [ { // when /
            path: '/',
            name: 'feed',
            component: FeedSubPage
        }, { // when /feed
            path: '/feed',
            name: 'feed-page',
            component: FeedSubPage
        }, { // when /lessons
            path: 'lessons',
            name: 'lessons-page',
            component: null
        }, { // when /courses
            path: 'courses',
            name: 'courses-page',
            component: null
        }, { // when /questions
            path: 'questions',
            name: 'questions-page',
            component: null
        }],
    },
];

export default homepageRouter;