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
    },

    /**
     * Danh sách giới tính
     */
    EGender: {
        Male: 0,
        Female: 1,
        Other: 2
    },

    /**
     * Danh sách trạng thái công việc
     */
    EWorkStatus: {
        Workful: 0,
        Workless: 1,
        Intern: 2,
        Rest: 3,
        Study: 4
    },

    /**
     * Danh sách Phân quyền người dùng
     */
    EUserRole: {
        User: "User",
        Admin: "Admin",
        Guest: "Guest",
        Banned: "Banned"
    },

    /**
     * Danh sách loại code cần xác minh
     */
    EVerifyCodeType: {
        Register: 0,
        ForgotPassword: 1,
        CancelUser: 2,
        Payment: 3
    },

    /**
     * Danh sách các bước trong một pipe, procedure nào đó
     */
    EProcedureStep: {
        Step1: 0,
        Step2: 1,
        Step3: 2,
        Step4: 3,
        Step5: 4,
        Step6: 5,
        Step7: 6,
        Step8: 7,
        Step9: 8,
        Step10: 9
    },

    /**
     * Danh sách phân loại quan hệ user
     */
    EUserRelationType: {
        Friend: 0,
        FriendRequest: 1,
        Follow: 2,
        Block: 3,
        Requester: 11,
        FriendRequester: 11,
        Requestee: 12,
        FriendRequestee: 12,
        Follower: 21,
        Followee: 22,
        Blocker: 31,
        Blockee: 32,
        NotInRelation: -1
    },

    /**
     * Quyền riêng tư
     */
    EPrivacy: {
        Private: 0,
        Public: 1
    },

    /**
     * Loại UserItem
     */
    EUserItemType: {
        Knowledge: 0,
        Comment: 1
    },

    /**
     * Loại phần tử kiến thức
     */
    EKnowledgeType: {
        Post: 0,
        Course: 1
    },

    /**
     * Loại bài đăng
     */
    EPostType: {
        Lesson: 0,
        Question: 1
    },

    /**
     * Loại quan hệ khóa học
     */
    ECourseRelationType: {
        Request: 0,
        Invite: 1
    },

    /**
     * Các lại vai trò của một user thường với một khóa học
     */
    ECourseRoleType: {
        NotAccessible: 0,
        Guest: 1,
        Member: 2,
        Owner: 3
    },

};

export { myEnum };

