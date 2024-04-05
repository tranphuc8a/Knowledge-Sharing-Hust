
import sidebarResource from "./sidebar-resource";
import validatorResource from "./validator-resource";
import headerResource from "./header-resource";
import loginPageResource from "./authentication/login-page-resource";
import forgotPasswordResource from "./authentication/forgot-password-resource";
import enterForgotPasswordVerificationCode from "./authentication/enter-forgot-password-verification-code-resource";
import setnewPasswordResource from "./authentication/setnew-password-resource";
import registerPageResource from "./authentication/register-page-resource";
import enterRegisterVerificationCodeResource from "./authentication/enter-register-verification-code-resource";
import createNewUserPageResource from "./authentication/create-new-user-page-resource";

const language = {
    'vi': {
        layout: {
            sidebar: sidebarResource.vi,
            header: headerResource.vi,
        },
        value: {
            gender: {
                male: 'Nam',
                female: 'Nữ',
                other: 'Khác'
            }
        },
        validator: validatorResource.vi,
        pages: {
            login: loginPageResource.vi,
            forgotpassword: forgotPasswordResource.vi,
            enterforgotpasswordverificationcode: enterForgotPasswordVerificationCode.vi,
            setnewpassword: setnewPasswordResource.vi,
            register: registerPageResource.vi,
            enterregisterverificationcode: enterRegisterVerificationCodeResource.vi,
            createnewuser: createNewUserPageResource.vi,

        }
    },
    'en': {
        layout: {
            sidebar: sidebarResource.en,
            header: headerResource.en
        },
        value: {
            gender: {
                male: 'Male',
                female: 'Female',
                other: 'Others'
            }
        },
        validator: validatorResource.en,
        pages: {
            login: loginPageResource.en,
            forgotpassword: forgotPasswordResource.en,
            enterforgotpasswordverificationcode: enterForgotPasswordVerificationCode.en,
            setnewpassword: setnewPasswordResource.en,
            register: registerPageResource.en,
            enterregisterverificationcode: enterRegisterVerificationCodeResource.en,
            createnewuser: createNewUserPageResource.en,
            
        }
    }
};

export { language };
