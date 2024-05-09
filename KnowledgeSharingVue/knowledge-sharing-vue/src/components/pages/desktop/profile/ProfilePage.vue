

<template>
    <DesktopHomeFrame>
        <div class="d-content p-profile" v-if="isShow">
            <div class="d-empty-panel" v-show="isUserExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="p-profile-page" v-show="isUserExisted === true">
                <div class="p-profile-panel card">
                    <ProfilePanel />
                </div>

                <router-view>
                </router-view>
            </div>

        </div>
    </DesktopHomeFrame>
</template>



<script>

import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CurrentUser from '@/js/models/entities/current-user';
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import ProfilePanel from './components/profile-panel/ProfilePanel.vue';
import { useRoute, useRouter } from 'vue-router';
import { GetRequest, Request } from '@/js/services/request';
import ViewUser from '@/js/models/views/view-user';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'ProfilePage',
    components: {
        DesktopHomeFrame,
        ProfilePanel, NotFoundPanel
    },
    props: {
    },
    async created(){
        this.createPage();
    },
    data(){
        return {
            isShow: true,
            isUserExisted: null,
            user: null,
            currentUser: null,
            route: useRoute(),
            router: useRouter(),
            errorMessage: '',
            defaultErrorMessage: 'Người dùng không tồn tại',
        }
    },
    mounted(){
        
    },
    provide(){
        return {
            getUser: () => this.user,
            getIsMySelf: this.isMySelf,
        }
    },
    methods: {
        async createPage(){
            try {
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }

                let username = this.route.params.username;
                let userId = this.route.query.userId;
                let usernameOrUserId = username ?? userId;
                if (Validator.isEmpty(usernameOrUserId)) {
                    this.user = this.currentUser;
                    this.isUserExisted = true;
                    return;
                }

                let res = await new GetRequest('Users/user-detail')
                    .setParams({unOruid: usernameOrUserId})
                    .execute();
                let body = await Request.tryGetBody(res);

                this.user = new ViewUser().copy(body);
                this.isUserExisted = true;
                this.forceRender();
            } catch (error) {
                try {
                    Request.resolveAxiosError(error);
                    let userMsg = await Request.tryGetUserMessage(error);
                    this.errorMessage = userMsg ?? this.defaultErrorMessage;
                    this.isUserExisted = false;
                    this.user = null;
                } catch (e){
                    console.error(e);
                }
            }
        },

        async forceRender(){
            try {
                this.isShow = false;
                this.$nextTick(() => {
                    this.isShow = true;
                });
            } catch (e) {
                console.error(e);
            }
        },

        async isMySelf(){
            let currentUser = await CurrentUser.getInstance();
            if (currentUser?.UserId != null){
                return currentUser.UserId === this.user?.UserId;
            }
            return false;
        }
    },
    watch: {
        user(){
            this.forceRender();
        },
        '$route.params.username'(){
            this.createPage();
        }
    },
    inject: {
        getPopupManager: {},
    }
}

</script>

<style scoped>
.d-content.p-profile{
    padding-top: 0;
    justify-content: center;
}
.p-profile-page{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
}
.p-profile-panel{
    width: 100%;
}

</style>

