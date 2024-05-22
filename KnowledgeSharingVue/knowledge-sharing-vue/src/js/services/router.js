
import { createRouter, createWebHistory } from 'vue-router';
import { GetRequest } from './request';

import authenticationRouter from '@/js/services/router/authentication-router';
import homepageRouter from '@/js/services/router/homepage-router';
import lestionDecreaditRouter from '@/js/services/router/lestion-decreadit-router';
import profileRouter from '@/js/services/router/profile-router';
import courseRouter from './router/course-router';

import TestPage from '@/components/pages/test-page/TestPage.vue';
import HustPage from '@/components/pages/hust-page/HustPage.vue';

let routers = [  
    {
        path: '/test',
        name: 'test',
        component: TestPage
    }, {
        path: '/hust',
        name: 'hust',
        component: HustPage
    },  
];

routers = routers.concat(
    authenticationRouter, 
    homepageRouter, 
    lestionDecreaditRouter, 
    profileRouter,
    courseRouter,
);




const createMyRouter = (app) => {
    let router = createRouter({
        history: createWebHistory(),
        routes: routers
    });

    router.beforeEach(async (to, from, next) => {
        if (to.matched.some(record => record.meta.requiredAuth)) {
            // show loading panel:
            // console.log(app);
            let loadingPanel = app?.config?.globalProperties?.globalMethods?.getLoadingPanel();
            loadingPanel?.show?.();

            let isLogedIn = await new GetRequest().checkLogedIn();
            if (!isLogedIn) {
                localStorage.setItem('redirect-to', to.fullPath);
                next('/login');
            } else {
                next();
            }

            loadingPanel?.hide?.();
        } else {
            // new GetRequest().checkLogedIn();
            next();
        }
    });

    return router;
};

export { createMyRouter };

