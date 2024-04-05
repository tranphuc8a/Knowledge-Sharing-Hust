import statusCodeEnum from "./status-code-enum";

let myEnum = {
    gender: {
        MALE: 0,
        FEMALE: 1,
        OTHER: 2
    },

    buttonState: {
        LOADING: "loading",
        NORMAL: "normal",
        DISABLED: "diable"
    },

    inputState: {
        NORMAL: "normal",
        READONLY: 'read-only',
        DISABLED: "disable",
        EXPAND: "expand"
    },

    
    dropdownState: {
        NORMAL: "normal",
        LOADING: "loading",
        EMPTY: "empty"
    },

    comboboxItemState: {
        NORMAL: "normal",
        DEFAULT: "default",
        CHOSEN: "chosen",
        FOCUS: "focus",
    },

    tableState: {
        NORMAL: 'normal',
        LOADING: 'loading',
        EMPTY: 'empty'
    },


    iconState: {
        NORMAL: 'normal',
        DISABLED: 'disable',
    },


    sidebarState: {
        NORMAL: 'normal',
        COLLAPSE: 'collapse',
    },

    importEmployeeStep: {
        STEP1: 1,
        STEP2: 2,
        STEP3: 3
    },

    language: {
        VIETNAMESE: 0,
        ENGLISH: 1
    },

    statusCode: statusCodeEnum,

    requestMethod: {
        GET     : 'get',
        POST    : 'post',
        PUT     : 'put',
        PATCH   : 'patch',
        DELETE  : 'delete',
        OPTIONS : 'options',
        HEAD    : 'head',
        CONNECT : 'connect',
        TRACE   : 'trace'
    },

    contentType: {
        JSON        : 'application/json',
        FORM_DATA   : 'multipart/form-data',
        URL_ENCODED : 'application/x-www-form-urlencoded'
    }
};

export { myEnum };

