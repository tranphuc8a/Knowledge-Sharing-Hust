
import LoginPage from '@/components/pages/authentication/login-page/LoginPage.vue';
import LoginWithGooglePage from '@/components/pages/authentication/login-page/LoginWithGooglePage.vue';

import ForgotPasswordPage from '@/components/pages/authentication/forgot-password-page/ForgotPasswordPage.vue';
import EnterForgotPasswordVerificationCodePage from '@/components/pages/authentication/forgot-password-page/EnterForgotPasswordVerificationCodePage.vue';
import SetNewPasswordPage from '@/components/pages/authentication/forgot-password-page/SetNewPasswordPage.vue';

import RegisterPage from '@/components/pages/authentication/register-page/RegisterPage.vue';
import EnterRegisterVerificationPage from '@/components/pages/authentication/register-page/EnterRegisterVerificationCodePage.vue';
import CreateNewUserPage from '@/components/pages/authentication/register-page/CreateNewUserPage.vue';
import RegisterWithGooglePage from '@/components/pages/authentication/register-page/RegisterWithGooglePage.vue';



const authenticationRouter = [
    {
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
    }
];

export default authenticationRouter;
