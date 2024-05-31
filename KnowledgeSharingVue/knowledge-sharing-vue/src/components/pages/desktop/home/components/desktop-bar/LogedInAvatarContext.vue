

<template>
    <div class="p-logedin-avatar-context">
        <MContextPopup position="">

            <template #popupContextMask>
                <Avatar 
                    :src="useravatar"
                    :size="40"
                    :title="username"
                />
            </template>
    
            <template #popupContextContent>
                <div class="p-lac-context card">
                    <div class="p-lac-user">
                        <router-link :to="detailUserLink" class="router-link">
                            <span>
                                <Avatar 
                                    :src="useravatar"
                                    :size="32"
                                    :title="username"
                                />
                            </span>
                            <div class="p-lac-username card-subheading">
                                {{username}}
                            </div>
                        </router-link>
                    </div>
                    <div class="p-devide"></div>
                    <div class="p-lac-menu">
                        <div class="p-lac-menu-item" 
                            v-for="item in menuItems"
                            :key="item.key"
                            @:click="item.onclick"
                            :title="item.label"
                        >
                            <span>
                                <MIcon :fa="item.fa" :family="item.family" :style="iconStyle" />
                            </span>
                            <span>
                                {{item.label}}
                            </span>
                        </div>
                    </div>
                    <div class="p-devide">
                    </div>
                    <div class="p-lac-logout p-lac-menu-item" @:click="resolveClickLogout">
                        <span>
                            <MIcon fa="right-from-bracket" :style="iconStyle" />
                        </span>
                        <span>
                            Đăng xuất
                        </span>
                    </div>
                </div>
            </template>
    
        </MContextPopup>
    </div>
</template>



<script>
import MContextPopup from '@/components/base/popup/MContextPopup.vue';
import CurrentUser from '@/js/models/entities/current-user';
import Avatar from '@/components/base/avatar/Avatar.vue';
import Common from '@/js/utils/common';
import { PostRequest, Request } from '@/js/services/request';

export default {
    name: 'LogedInAvatarContext',
    components: {
        MContextPopup,
        Avatar
    },
    props: {
    },
    data(){
        return {
            defaultAvatar: require('@/assets/default-thumbnail/student-image-icon.png'),
            useravatar: null,
            username: null,
            menuItems: [],
            detailUserLink: '',
        }
    },
    async created(){
        await this.createElement();
    },
    async mounted(){
    },
    methods: {
        async initItems(){
            let that = this;
            let user = await this.getCurrentUser();
            let username = user?.Username ?? user?.UserId;
            let postfix = '/profile/' + String(username);
            let fa = ['pencil', 'book-reader', 'layer-group', 'book-open', 'comments', 'user-friends', 'bookmark'];
            let label = ['Chỉnh sửa trang cá nhân', 'Đang học', 'Khóa học của tôi', 'Bài giảng của tôi', 'Bài thảo luận của tôi', 'Bạn bè', 'Đã lưu'];
            let destination = [postfix + '/profile-edit', postfix + '/learn', postfix + '/course', 
                                postfix + '/lesson', postfix + '/question', 
                                postfix + '/friend', postfix + '/save'];
            for (let i = 0; i < fa.length; i++) {
                that.menuItems.push({
                    fa: fa[i],
                    family: 'fas',
                    label: label[i],
                    key: i,
                    onclick: () => {
                        that.$router.push(destination[i]);
                    }
                });
            }
        },

        async createElement(){
            try {
                let user = await this.getCurrentUser();
                if (await Common.isValidImage(user?.Avatar)) {
                    this.useravatar = user.Avatar;
                }
                this.username = user?.FullName ?? 'Người dùng hệ thống';
                this.detailUserLink = '/profile/' + String(user?.Username ?? user?.UserId);
                this.initItems();
            } catch (e){
                this.useravatar = this.defaultAvatar;
                console.error(e);
            }
        },


        async resolveClickLogout(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn đăng xuất?";
                this.getPopupManager().warning(alertMsg, this.submitLogout.bind(this));
            } catch (e){
                await Request.resolveAxiosError(e);
            }
        },


        async submitLogout(){
            try {
                await new PostRequest('Authentications/logout').execute();
                await CurrentUser.deleteInstance();
                await Request.deleteLocalStorage();
                location.reload();
            } catch (e){
                await Request.resolveAxiosError(e);
            }
        }
        
    },
    inject: {
        getCurrentUser: {},
        getPopupManager: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-logedin-avatar-context{
    width: 100%;
    height: 100%;
}

.p-lac-context {
    padding: 16px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 8px;
}

.p-lac-user{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
    padding: 8px 16px;
}

.p-lac-user:hover{
    background-color: rgba(var(--primary-color-100-rgb), .56);
}

.p-lac-user .router-link{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}


.p-lac-menu{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}



.p-devide{
    margin: 0px 4px;
}

.p-lac-menu-item{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    padding: 8px 16px;
    gap: 16px;
    cursor: pointer;
    font-family: 'ks-font-semibold';
}

.p-lac-menu-item:hover{
    background-color: rgba(var(--primary-color-100-rgb), .56);
}   

.p-lac-menu-item > :first-child{
    width: 24px;
}

</style>

