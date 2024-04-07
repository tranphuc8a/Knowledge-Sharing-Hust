
import { createRouter, createWebHistory } from 'vue-router';

// import LoginPage from '@/components/pages/login-page/KSLoginPage.vue';
// import ForgotPasswordPage from '@/components/pages/forgot-password-page/KSForgotPasswordPage.vue';
// import EnterForgotPasswordVerificationCodePage from '@/components/pages/forgot-password-page/KSEnterForgotPasswordVerificationCodePage.vue';
// import SetNewPasswordPage from '@/components/pages/forgot-password-page/KSSetNewPasswordPage.vue';

// import RegisterPage from '@/components/pages/register-page/KSRegisterPage.vue';
// import EnterRegisterVerificationPage from '@/components/pages/register-page/KSEnterRegisterVerificationCodePage.vue';
// import CreateNewUserPage from '@/components/pages/register-page/KSCreateNewUserPage.vue';

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
    component: null,
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

