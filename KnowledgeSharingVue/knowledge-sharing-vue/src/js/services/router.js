
import { createRouter, createWebHistory } from 'vue-router';

import LoginPage from '@/components/pages/authentication/login-page/LoginPage.vue';
import LoginWithGooglePage from '@/components/pages/authentication/login-page/LoginWithGooglePage.vue';

import ForgotPasswordPage from '@/components/pages/authentication/forgot-password-page/ForgotPasswordPage.vue';
import EnterForgotPasswordVerificationCodePage from '@/components/pages/authentication/forgot-password-page/EnterForgotPasswordVerificationCodePage.vue';
import SetNewPasswordPage from '@/components/pages/authentication/forgot-password-page/SetNewPasswordPage.vue';

import RegisterPage from '@/components/pages/authentication/register-page/RegisterPage.vue';
import EnterRegisterVerificationPage from '@/components/pages/authentication/register-page/EnterRegisterVerificationCodePage.vue';
import CreateNewUserPage from '@/components/pages/authentication/register-page/CreateNewUserPage.vue';
import RegisterWithGooglePage from '@/components/pages/authentication/register-page/RegisterWithGooglePage.vue';

import DesktopHomePage from '@/components/pages/desktop/home/DesktopHomePage.vue';
import FeedSubPage from '@/components/pages/desktop/home/sub-pages/FeedSubPage.vue';

import TestPage from '@/components/pages/test-page/TestPage.vue';
import { GetRequest } from './request';

import LessonDetailPage from '@/components/pages/desktop/post-detail/LessonDetailPage.vue';
import CreateLessonPage from '@/components/pages/desktop/post-detail/CreateLessonPage.vue';
import EditLessonPage from '@/components/pages/desktop/post-detail/EditLessonPage.vue';

import HustPage from '@/components/pages/hust-page/HustPage.vue';

const routers = [{
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
}, {
    path: '/employee',
    name: 'employee',
    component: null,
    meta: {
        requiredAuth: true
    },
    children: [ { // when /employee
        path: '',
        name: 'employee-home-page',
        component: null
    }, { // when /employee/import
        path: 'import',
        component: null, 
    }],
}, {
    path: '/login',
    name: 'login',
    component: LoginPage,
    meta: {
        autoRedirect: true
    }
}, {
    path: '/login/google',
    name: 'loginWithGoogle',
    component: LoginWithGooglePage
}, {
    path: '/forgotpassword',
    name: 'forgotpassword',
    component: ForgotPasswordPage
}, {
    path: '/enter-forgotpassword-verification-code',
    name: 'enter-forgotpassword-verification-code',
    component: EnterForgotPasswordVerificationCodePage
}, {
    path: '/set-new-password',
    name: 'set-new-password',
    component: SetNewPasswordPage
}, {
    path: '/register',
    name: 'register',
    component: RegisterPage
}, {
    path: '/register/google',
    name: 'register-with-google',
    component: RegisterWithGooglePage
}, {
    path: '/enter-register-verification-code',
    name: 'enter-register-verification-code',
    component: EnterRegisterVerificationPage
},  {
    path: '/create-new-user',
    name: 'create-new-user',
    component: CreateNewUserPage
}, {
    path: '/test',
    name: 'test',
    component: TestPage
}, {
    path: '/hust',
    name: 'hust',
    component: HustPage
}, {
    path: '/lesson/:lessonId',
    name: 'lesson-detail',
    component: LessonDetailPage
}, {
    path: '/lesson-create',
    name: 'lesson-create',
    component: CreateLessonPage
}, {
    path: '/lesson-edit/:lessonId',
    name: 'lesson-edit',
    component: EditLessonPage
}, ];




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
            new GetRequest().checkLogedIn();
            next();
        }
    });

    return router;
};

export { createMyRouter };

