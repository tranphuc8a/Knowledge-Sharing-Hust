

import ProfilePage from '@/components/pages/desktop/profile/ProfilePage.vue';
import ProfileHomeSubpage from '@/components/pages/desktop/profile/subpages/ProfileHomeSubpage.vue';

const profileRouter = [
    {
        path: '/profile/:username',
        name: 'profile',
        component: ProfilePage,
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
                component: null, 
            }, { // when /profile.../lesson
                path: 'lesson',
                name: 'profile-lesson-page',
                component: null, 
            }, { // when /profile.../question
                path: 'question',
                name: 'profile-question-page',
                component: null, 
            }, { // when /profile.../course
                path: 'course',
                name: 'profile-course-page',
                component: null, 
            }, { // when /profile.../save
                path: 'save',
                name: 'profile-save-page',
                component: null, 
            }, { // when /profile.../friend
                path: 'friend',
                name: 'profile-friend-page',
                component: null, 
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
                component: null, 
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
                component: null, 
            }, { // when /profile.../profile-detail
                path: 'profile-detail',
                name: 'profile-profile-detail-page',
                component: null, 
            }
        ],
    }
];

export default profileRouter;