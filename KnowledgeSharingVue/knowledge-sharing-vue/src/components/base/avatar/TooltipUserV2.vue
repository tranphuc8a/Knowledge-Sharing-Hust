<template>
    <div class="p-tooltip-user-info card">

        <div class="p-tui-cover">
            <router-link :to="userDetailLink" class="router-link">
                <div class="p-tui-cover-image"
                    :style="{ backgroundImage: `url(${coverImage})` }"
                >
                    <div class="p-tui-cover-image-overlay"></div>
                </div>
            </router-link>
        </div>

        <div class="p-tui-avatar-name">
            <div class="p-tui-avatar">
                <div class="p-tui-avatar-frame">
                    <UserAvatar :user="user" :size="138" />
                </div>
            </div>
            <div class="p-tui-name">
                <div class="p-tui-fullname">
                    <router-link :to="userDetailLink">
                        <span>
                            <EllipsisText :text="user?.FullName" :style="fullnameStyle" :max-line="3" />
                        </span>
                    </router-link>
                </div>
                <div class="p-tui-username">
                    <router-link :to="userDetailLink">
                        <span>
                            <EllipsisText :text="`@${user?.Username}`" :style="{ color: '#666' }" :max-line="1" />
                        </span>
                    </router-link>
                </div>
            </div>
        </div>

        <div class="p-tui-button">
            <div class="p-tui-relation-button">
                <UserRelationButton :is-call-api-when-create="false" />
            </div>
            <div class="p-tui-message-button" v-if="!isMySelf">
                <MessageButton />
            </div>
        </div>
    </div>
</template>


<script>
import UserAvatar from './UserAvatar.vue';
import Common from '@/js/utils/common';
import EllipsisText from '../text/EllipsisText.vue';
import CurrentUser from '@/js/models/entities/current-user';
import UserRelationButton from './../../pages/desktop/profile/components/user-relation-button/UserRelationButton.vue'
import MessageButton from './../../pages/desktop/profile/components/user-relation-button/MessageButton.vue'

export default {
    name: "TooltipUserV2",
    data() {
        return {
            coverImage: null,
            defaultCoverImage: require("@/assets/default-thumbnail/default-cover.jpg"),
            fullnameStyle: {
                fontSize: '1.2rem',
                fontWeight: 'bold',
                color: '#333'
            },
            isMySelf: true,
            userDetailLink: '',
        }
    },
    async created(){
        await this.refreshUser();
    },
    components: {
        UserAvatar,
        EllipsisText,
        UserRelationButton,
        MessageButton
    },
    methods: {
        async refreshUser(){
            try {
                let currentUser = await CurrentUser.getInstance();
                this.isMySelf = (currentUser != null) && (currentUser.UserId == this.user?.UserId);
                this.userDetailLink = `/profile/${this.user?.Username}`;
                
                let userCover = this.user?.Cover;
                if (await Common.isValidImage(userCover)) {
                    this.coverImage = userCover;
                } else {
                    this.coverImage = this.defaultCoverImage;
                }
            } catch (e) {
                console.error(e);
            }
        }
    },
    provide(){
        return {
            getUser: () => this.user
        }
    },
    props: {
        user: {
            required: true,
            default: null
        }
    },
    watch: {
        user: {
            handler: function(){
                this.refreshUser();
            },
            immediate: true
        }
    }
};
</script>

<style scoped>
.p-tooltip-user-info {
    width: 350px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    border-radius: 8px;
}

.p-tui-cover {
    width: 100%;
    height: 100px;
    position: relative;
    border-top-left-radius: 8px;
    border-top-right-radius: 8px;
    overflow: hidden;
}

.p-tui-cover-image {
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
    position: relative;
}

.p-tui-cover-image-overlay {
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.1);
    position: absolute;
    top: 0;
    left: 0;
}

.p-tui-cover-image-overlay:hover {
    background: rgba(0, 0, 0, 0.3);
}

.p-tui-avatar-name {
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
    padding: 0 16px;
}

.p-tui-avatar {
    width: 150px;
    height: 100px;
    position: relative;
    flex-grow: 0;
    flex-shrink: 0;
}

.p-tui-avatar-frame {
    width: 150px;
    height: 150px;
    position: absolute;
    bottom: 0px;
    left: 0px;
    border-radius: 50%;
    overflow: hidden;
    padding: 6px;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #fff;
}

.p-tui-name {
    display: flex;
    width: 0;
    flex-grow: 1;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
    gap: 8px;
}

.p-tui-fullname {
    max-width: 100%;
    font-size: 1.2rem;
    font-weight: bold;
    color: #333;
}

.p-tui-fullname > a {
    max-width: 100%;
}

.p-tui-username {
    max-width: 100%;
    font-size: 0.9rem;
    color: #666;
}

.p-tui-button {
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 8px;
    padding: 16px;
}

.p-tui-relation-button {
    width: 0;
    flex-grow: 3;
    flex-shrink: 0;
}

.p-tui-message-button {
    width: 0;
    flex-grow: 2;
    flex-shrink: 0;
}

a {
    text-decoration: none;
    color: inherit;
}

</style>

