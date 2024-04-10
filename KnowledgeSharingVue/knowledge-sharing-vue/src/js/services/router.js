
import { createRouter, createWebHistory } from 'vue-router';

import LoginPage from '@/components/pages/authentication/login-page/LoginPage.vue';
// import ForgotPasswordPage from '@/components/pages/authentication/forgot-password-page/ForgotPasswordPage.vue';
// import EnterForgotPasswordVerificationCodePage from '@/components/pages/authentication/forgot-password-page/EnterForgotPasswordVerificationCodePage.vue';
// import SetNewPasswordPage from '@/components/pages/authentication/forgot-password-page/SetNewPasswordPage.vue';

// import RegisterPage from '@/components/pages/authentication/register-page/RegisterPage.vue';
// import EnterRegisterVerificationPage from '@/components/pages/authentication/register-page/EnterRegisterVerificationCodePage.vue';
// import CreateNewUserPage from '@/components/pages/authentication/register-page/CreateNewUserPage.vue';

import TestPage from '@/components/pages/test-page/TestPage.vue';
import { GetRequest } from './request';

const routers = [{
    path: '/',
    name: 'home',
    component: null,
    meta: {
        requiredAuth: true
    }
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
    path: '/forgotpassword',
    name: 'forgotpassword',
    component: null
}, {
    path: '/enter-forgotpassword-verification-code',
    name: 'enter-forgotpassword-verification-code',
    component: null
}, {
    path: '/set-new-password',
    name: 'set-new-password',
    component: null
}, {
    path: '/register',
    name: 'register',
    component: null
},  {
    path: '/enter-register-verification-code',
    name: 'enter-register-verification-code',
    component: null
},  {
    path: '/create-new-user',
    name: 'create-new-user',
    component: null
}, {
    path: '/test',
    name: 'test',
    component: TestPage
}];




const createMyRouter = (app) => {
    let router = createRouter({
        history: createWebHistory(),
        routes: routers
    });

    router.beforeEach(async (to, from, next) => {
        if (to.matched.some(record => record.meta.requiredAuth)) {
            app.store
            let isLogedIn = await new GetRequest().checkLogedIn();
            if (!isLogedIn) {
                localStorage.setItem('redirect-to', to.fullPath);
                next('/login');
            } else {
                next();
            }
        } else {
            next();
        }
    });

    return router;
};

export { createMyRouter };

