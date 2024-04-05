
import { createRouter, createWebHistory } from 'vue-router';
import CustomerPage from '@/components/pages/customer-page/MISACustomerPage.vue';
import ImportPage from '@/components/pages/employee-page/import-employee-page/MISAImportPage.vue';
import EmployeeHomePage from '@/components/pages/employee-page/employee-home-page/MISAEmployeeHomePageServerSidePagination.vue';
import EmployeePage from '@/components/pages/employee-page/MISAEmployeePage.vue';

import LoginPage from '@/components/pages/login-page/MISALoginPage.vue';
import ForgotPasswordPage from '@/components/pages/forgot-password-page/MISAForgotPasswordPage.vue';
import EnterForgotPasswordVerificationCodePage from '@/components/pages/forgot-password-page/MISAEnterForgotPasswordVerificationCodePage.vue';
import SetNewPasswordPage from '@/components/pages/forgot-password-page/MISASetNewPasswordPage.vue';

import RegisterPage from '@/components/pages/register-page/MISARegisterPage.vue';
import EnterRegisterVerificationPage from '@/components/pages/register-page/MISAEnterRegisterVerificationCodePage.vue';
import CreateNewUserPage from '@/components/pages/register-page/MISACreateNewUserPage.vue';

import HustPage from '@/components/pages/hust-page/HustPage.vue';

import TestPage from '@/components/pages/test-page/MISATestPage.vue';
import { GetRequest } from './request';

const routers = [{
    path: '/',
    name: 'home',
    component: EmployeePage,
    meta: {
        requiredAuth: true
    }
}, {
    path: '/report',
    name: 'report',
    component: null,
    meta: {
        requiredAuth: true
    }
}, {
    path: '/customer',
    name: 'customer',
    component: CustomerPage,
    meta: {
        requiredAuth: true
    }
}, {
    path: '/employee',
    name: 'employee',
    component: EmployeePage,
    meta: {
        requiredAuth: true
    },
    children: [ { // when /employee
        path: '',
        name: 'employee-home-page',
        component: EmployeeHomePage
    }, { // when /employee/import
        path: 'import',
        component: ImportPage, 
    }],
}, {
    path: '/setting',
    name: 'setting',
    component: null,
    meta: {
        requiredAuth: true
    }
}, {
    path: '/import',
    name: 'import',
    component: null
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
},  {
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

