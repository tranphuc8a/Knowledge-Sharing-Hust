

import ProfilePage from '@/components/pages/desktop/profile/ProfilePage.vue';
import ProfileHomeSubpage from '@/components/pages/desktop/profile/subpages/profile-home/ProfileHomeSubpage.vue';
import ProfileLearnSubpage from '@/components/pages/desktop/profile/subpages/profile-learn/ProfileLearnSubpage.vue';
import ProfileLessonSubpage from '@/components/pages/desktop/profile/subpages/profile-lesson/ProfileLessonSubpage.vue';
import ProfileQuestionSubpage from '@/components/pages/desktop/profile/subpages/profile-question/ProfileQuestionSubpage.vue';

import ProfileRelationSubpage from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileRelationSubpage.vue';
import ProfileFriendContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileFriendContent.vue';
import ProfileFollowerContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileFollowerContent.vue';
import ProfileFolloweeContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileFolloweeContent.vue';
import ProfileRequesterContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileRequesterContent.vue';
import ProfileRequesteeContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileRequesteeContent.vue';
import ProfileBlockerContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileBlockerContent.vue';
import ProfileBlockeeContent from '@/components/pages/desktop/profile/subpages/profile-friend/ProfileBlockeeContent.vue';


import ProfileImageSubpage from '@/components/pages/desktop/profile/subpages/profile-image/ProfileImageSubpage.vue';
import ProfileEditSubpage from '@/components/pages/desktop/profile/subpages/profile-edit/ProfileEditSubpage.vue';

import ProfileAccountSubpage from '@/components/pages/desktop/profile/subpages/profile-account/ProfileAccountSubpage.vue';
import ProfileAccountChangePasswordContent from '@/components/pages/desktop/profile/subpages/profile-account/ProfileAccountChangePasswordContent.vue';
import ProfileAccountLogoutContent from '@/components/pages/desktop/profile/subpages/profile-account/ProfileAccountLogoutContent.vue';

import ProfileCourseSubpage from '@/components/pages/desktop/profile/subpages/profile-course/ProfileCourseSubpage.vue';

const profileRouter = [
    {
        path: '/profile/:username',
        name: 'profile',
        component: ProfilePage,
        meta: {
            requiredAuth: false
        },
        children: [ 
            { // when /profile
                path: '',
                name: 'profile-home-page',
                component: ProfileHomeSubpage
            }, { // when /profile.../feed
                path: 'feed',
                name: 'profile-feed-page',
                component: ProfileHomeSubpage, 
            }, { // when /profile.../learn
                path: 'learn',
                name: 'profile-learn-page',
                component: ProfileLearnSubpage, 
            }, { // when /profile.../lesson
                path: 'lesson',
                name: 'profile-lesson-page',
                component: ProfileLessonSubpage,  
            }, { // when /profile.../question
                path: 'question',
                name: 'profile-question-page',
                component: ProfileQuestionSubpage,  
            }, { // when /profile.../course
                path: 'course',
                name: 'profile-course-page',
                component: ProfileCourseSubpage, 
            }, { // when /profile.../save
                path: 'save',
                name: 'profile-save-page',
                component: null, 
            }, { // when /profile.../friend
                path: 'friend',
                name: 'profile-friend-page',
                component: ProfileRelationSubpage, 
                children: [
                    { // when /profile/.../friend/
                        path: '',
                        name: 'profile-friend-home',
                        component: ProfileFriendContent,
                    },
                    { // when /profile/.../friend/friend
                        path: 'friend',
                        name: 'profile-friend-friend',
                        component: ProfileFriendContent,
                    },
                    { // when /profile/.../friend/follower
                        path: 'follower',
                        name: 'profile-friend-follower',
                        component: ProfileFollowerContent,
                    },
                    { // when /profile/.../friend/followee
                        path: 'followee',
                        name: 'profile-friend-followee',
                        component: ProfileFolloweeContent,
                    },
                    { // when /profile/.../friend/requester
                        path: 'requester',
                        name: 'profile-friend-requester',
                        component: ProfileRequesterContent,
                    },
                    { // when /profile/.../friend/requestee
                        path: 'requestee',
                        name: 'profile-friend-requestee',
                        component: ProfileRequesteeContent,
                    },
                    { // when /profile/.../friend/blocker
                        path: 'blocker',
                        name: 'profile-friend-blocker',
                        component: ProfileBlockerContent,
                    },
                    { // when /profile/.../friend/blockee
                        path: 'blockee',
                        name: 'profile-friend-blockee',
                        component: ProfileBlockeeContent,
                    },
                ]
            }, { // when /profile.../star
                path: 'star',
                name: 'profile-star-page',
                component: null, 
            }, { // when /profile.../comment
                path: 'comment',
                name: 'profile-comment-page',
                component: null, 
            }, { // when /profile.../payment
                path: 'payment',
                name: 'profile-payment-page',
                component: null, 
            }, { // when /profile.../image
                path: 'image',
                name: 'profile-image-page',
                component: ProfileImageSubpage, 
            }, { // when /profile.../message
                path: 'message',
                name: 'profile-message-page',
                component: null, 
            }, { // when /profile.../notification
                path: 'notification',
                name: 'profile-notification-page',
                component: null, 
            }, { // when /profile.../block
                path: 'block',
                name: 'profile-block-page',
                component: null, 
            }, { // when /profile.../profile-edit
                path: 'profile-edit',
                name: 'profile-profile-edit-page',
                component: ProfileEditSubpage, 
            }, { // when /profile.../profile-detail
                path: 'profile-detail',
                name: 'profile-profile-detail-page',
                component: null, 
            }, { // when /profile.../account
                path: 'account',
                name: 'profile-account-page',
                component: ProfileAccountSubpage,
                children: [
                    {
                        path: '',
                        name: 'profile-account-home',
                        component: ProfileAccountChangePasswordContent,
                    }, {
                        path: 'change-password',
                        name: 'profile-account-change-password',
                        component: ProfileAccountChangePasswordContent,
                    }, {
                        path: 'logout',
                        name: 'profile-account-logout',
                        component: ProfileAccountLogoutContent,
                    }
                ]
            }
        ],
    }
];

export default profileRouter;