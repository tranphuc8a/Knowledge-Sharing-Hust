

<template>
    <div class="p-profile-panel-user-avatar">
        <div class="p-avatar-circle">
            <div class="p-avatar-circle_in"
                @click="resolveClickAvatar"
            >
                <div class="p-avatar-image"
                    :style="{backgroundImage: `url(${imageSrc})`}"    
                >
                    <!-- <img 
                        :src="imageSrc"
                    /> -->
                </div>
                <div class="p-avatar-image-hover">            
                </div>
            </div>
        </div>
        
        <div class="p-avatar-camera" @:click="resolveClickEditAvatar"
            v-if="isMySelf"
        >
            <MIcon 
                fa="camera"
                :iconStyle="iconStyle"
            />
        </div>
        <PreviewImage :src="imageSrc" ref="preview"/>
        <div>
            <SelectImagePopup ref="select-image-popup" :on-okay="resolveSubmitAvatar" :isShow="false"/>
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import CurrentUser from '@/js/models/entities/current-user';
import PreviewImage from '@/components/base/image/PreviewImage.vue';
import SelectImagePopup from '@/components/base/popup/select-image-popup/SelectImagePopup.vue';
import { Validator } from '@/js/utils/validator';
import { PatchRequest, Request } from '@/js/services/request';

export default {
    name: 'ProfilePanelUserAvatar',
    components: {
        PreviewImage,
        SelectImagePopup
    },
    props: {
    },
    data(){
        return {
            visiblePreview: false,
            imageSrc: null,
            defaultImageSrc: require('@/assets/default-thumbnail/student-image-icon.png'),
            iconStyle: {
            },
            currentUser: null,
            isMySelf: false,
        }
    },
    async mounted(){
        try {
            this.$refs['select-image-popup'].hide();
            this.imageSrc = this.defaultImageSrc;
            let avatar = this.getUser()?.Avatar;
            if (await Common.isValidImage(avatar)){
                this.imageSrc = avatar;
            }
            this.currentUser = await CurrentUser.getInstance();
            if (this.currentUser != null){
                this.isMySelf = this.currentUser.UserId == this.getUser()?.UserId;
            }
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async resolveClickEditAvatar(){
            try {
                this.$refs['select-image-popup'].show();
            } catch (e) {
                console.error(e);
            }
        },

        async resolveClickAvatar(){
            try {
                this.$refs.preview.show();
            } catch (e) {
                console.error(e);
            }
        },

        async resolveSubmitAvatar(image){
            try {
                this.$refs['select-image-popup'].hide();
                if (Validator.isEmpty(image)){
                    return;
                }
                // update image:
                await new PatchRequest('Users/me/update-avatar-url')
                    .setBody(image).execute();
                // success:
                this.getToastManager().success('Cập nhật ảnh đại diện thành công');
                let currentUser = await CurrentUser.getInstance();
                currentUser.Avatar = image;
                await CurrentUser.setInstance(currentUser);
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            } catch (e) {
                Request.resolveAxiosError(e);
            }
        }
    },
    inject: {
        getUser: {},
        getToastManager: {},
    }
}

</script>

<style scoped>

.p-profile-panel-user-avatar{
    width: 178px;
    height: 100px;
    position: relative;
}

.p-avatar-circle{
    width: 178px;
    height: 178px;
    border-radius: 100%;
    overflow: hidden;
    cursor: pointer;
    background-color: white;

    position: absolute;
    bottom: 0;
    left: 0;

    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-avatar-circle_in,
.p-avatar-image,
.p-avatar-image-hover{
    width: 168px;
    height: 168px;
    border-radius: 50%;
    position: absolute;
    overflow: hidden;
}

.p-avatar-image{
    background-size: cover;
    background-position: center center;
}

.p-avatar-image > img{
    width: 168px;
    height: 168px;
}

.p-avatar-image-hover{
    transition: background-color 0.3s ease;
    background-color: rgba(var(--primary-color-400-rgb), 0);
}

.p-avatar-circle_in:hover .p-avatar-image-hover{
    background-color: rgba(var(--primary-color-400-rgb), .24);
}

.p-avatar-camera{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 12px;
    cursor: pointer;
    width: 36px;
    height: 36px;
    border-radius: 50%;
    background-color: var(--primary-color-100);

    display: flex;
    flex-flow:  row nowrap;
    justify-content: center;
    align-items: center;
}

.p-avatar-camera:hover{
    background-color: var(--primary-color-200);
}

</style>

