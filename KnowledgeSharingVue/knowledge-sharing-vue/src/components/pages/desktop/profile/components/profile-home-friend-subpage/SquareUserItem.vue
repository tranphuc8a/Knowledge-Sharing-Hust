

<template>
    <div class="p-square-user-item">
        <TooltipFrame>
            <template #tooltipMask>
                <div class="p-square-user-avatar"
                    :style="{ backgroundImage: `url(${userAvatar})` }"
                    @:click="resolveClickUser">
                    
                    <div class="p-avatar-overlay"></div>
                </div>
            </template>
    
            <template #tooltipContent>
                <TooltipUserV2 :user="user" />
            </template>
        </TooltipFrame>


        <div class="p-username-frame">
            <TooltipFrame>
                <template #tooltipMask>
                    <div class="p-username"
                        @:click="resolveClickUser">
                        {{ user.FullName ?? "Người dùng hệ thống" }}
                    </div>
                </template>
        
                <template #tooltipContent>
                    <TooltipUserV2 :user="user" />
                </template>
            </TooltipFrame>
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import TooltipUserV2 from '@/components/base/avatar/TooltipUserV2.vue';
import TooltipFrame from '@/components/base/tooltip/TooltipFrame.vue';
import { useRouter } from 'vue-router';

export default {
    name: 'SquareUserItem',
    components: {
        TooltipUserV2,
        TooltipFrame,
    },
    props: {
        user: {
            required: true,
        }
    },
    data(){
        return {
            defaultAvatar: require('@/assets/default-thumbnail/student-image-icon.png'),
            userAvatar: null,
            router: useRouter(),
        }
    },
    mounted(){
        this.refreshUser();
    },
    methods: {
        async refreshUser() {
            try {
                this.userAvatar = this.defaultAvatar;
                let isValid = await Common.isValidImage(this.user.Avatar);
                if (isValid){
                    this.userAvatar = this.user.Avatar;
                }
            } catch (error) {
                console.log(error);
            }
        },

        async resolveClickUser(){
            try {
                if (this.user.Username){
                    this.router.push(`/profile/${this.user.Username}`);
                }
            } catch (error) {
                console.log(error);
            }
        }
    },
    watch: {
        user: {
            deep: true,
            handler(){
                this.refreshUser();
            }
        }
    }
}

</script>

<style scoped>

.p-square-user-item{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-square-user-avatar{
    width: 100%;
    aspect-ratio: 1 / 1;
    background-size: cover;
    background-position: center center;
    cursor: pointer;
    border-radius: 8px;
    overflow: hidden;
}

.p-square-user-item .p-tooltip-container{
    width: 100%;
}

.p-avatar-overlay{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    border-radius: 8px;
    background-color: rgba(0, 0, 0, 0);
}

.p-avatar-overlay:hover{
    background-color: rgba(0, 0, 0, 0.5);
}

.p-username-frame{
    width: fit-content;
    max-width: 100%;
}

.p-username{
    cursor: pointer;
    font-size: 13px;
    font-family: 'ks-font-semibold';
    color: var(--text-color);
    text-align: left;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.p-username:hover{
    text-decoration: underline;
}

</style>

