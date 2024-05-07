

<template>
    <div class="p-user-information">
        <div class="p-user-information__left">
            <div class="p-user-avatar">
                <!-- ProfilePanelUserAvatar -->
                <ProfilePanelUserAvatar />
            </div>
            <div class="p-user-infor">
                <div class="p-user-name">
                    {{ dUser.FullName }}
                </div>
                <div class="p-user-friend">
                    <!-- ProfilePaneUserlFriends -->
                    <ProfilePanelUserFriend />
                </div>
            </div>
        </div>
        <div class="p-user-information__right">
            <!-- ProfilePanelUserButtons -->
            <ProfilePanelButton />
        </div>
    </div>
</template>



<script>
import ProfilePanelUserAvatar from './ProfilePanelUserAvatar.vue';
import ProfilePanelUserFriend from './ProfilePanelUserFriend.vue';
import ProfilePanelButton from './ProfilePanelButton';

export default {
    name: 'ProfilePanelInformation',
    components: {
        ProfilePanelUserAvatar,
        ProfilePanelUserFriend,
        ProfilePanelButton,
    },
    props: {
    },
    data(){
        return {
            dUser: {}
        }
    },
    mounted(){
        try {
            this.dUser = this.getUser() ?? {};
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
    },
    inject: {
        getUser: {},
    }
}

</script>

<style scoped>
.p-user-information{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-end;
    height: 132px;
}

.p-user-information > * {
    height: 100%;
}

.p-user-information__left{
    width: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-end;
    height: 100%;
    gap: 24px;
}

.p-user-infor{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
    height: auto;
    align-self: stretch;
    gap: 8px;
}

.p-user-information__left > *,
.p-user-information > *{
    flex-shrink: 0;
}

.p-user-name{
    font-size: 28px;
    font-family: 'ks-font-bold';
    line-height: 28px;
    color: #000000;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}
</style>


