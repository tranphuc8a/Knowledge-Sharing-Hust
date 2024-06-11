

<template>

    <div class="p-cover-image-frame"
        @:click="resolveClickCover"
        :style="{
            backgroundImage: `url('${this.coverImage}')`,
            backgroundSize: 'cover',
            backgroundPosition: 'center center'
        }"
    >
        <!-- <img class="p-cover-image"
            @:click="resolveClickCover"
            :src="coverImage" alt="CoverImage" /> -->
        <div class="p-edit-cover-image-button" v-if="isMySelf">
            <MCancelButton 
                label="Chỉnh sửa ảnh bìa"
                :onclick="resolveClickChangeCoverImage"
                :buttonStyle="buttonStyle"
                fa="camera" family="fas" :iconStyle="iconStyle"
            />
        </div>
        <PreviewImage :src="coverImage" ref="preview" />
    </div>
    <SelectImagePopup ref="select-image-popup" :on-okay="resolveSubmitCoverImage" :isShow="false"/>

</template>



<script>
import CurrentUser from '@/js/models/entities/current-user';
import MCancelButton from '@/components/base/buttons/MCancelButton';
import Common from '@/js/utils/common';
import PreviewImage from '@/components/base/image/PreviewImage.vue';
import SelectImagePopup from '@/components/base/popup/select-image-popup/SelectImagePopup.vue';
import { PatchRequest, Request } from '@/js/services/request';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'ProfilePanelCoverImage',
    components: {
        MCancelButton, PreviewImage,
        SelectImagePopup
    },
    props: {
    },
    data(){
        return {
            defaultCoverImage: require('@/assets/default-thumbnail/default-cover.jpg'),
            coverImage: null,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            currentUser: null,
            isMySelf: false
        }
    },
    async created(){
        try {
            this.currentUser = await CurrentUser.getInstance();
            this.coverImage = this.defaultCoverImage;
            let cover = this.getUser()?.Cover;
            if (await Common.isValidImage(cover)){
                this.coverImage = cover;
            }
            this.isMySelf = 
                this.currentUser != null && 
                this.currentUser.UserId == this.getUser()?.UserId;
        } catch (e) {
            console.error(e);
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickChangeCoverImage(){
            try {
                this.$refs['select-image-popup'].show();
            } catch (e) {
                console.error(e);
            }
        },

        async resolveSubmitCoverImage(image){
            try {
                this.$refs['select-image-popup'].hide();
                if (Validator.isEmpty(image)){
                    return;
                }
                await new PatchRequest('Users/me/update-cover-url')
                    .setBody(image)
                    .execute();
                this.getToastManager().success('Cập nhật ảnh bìa thành công');
                let currentUser = await CurrentUser.getInstance();
                currentUser.Cover = image;
                await CurrentUser.setInstance(currentUser);
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            } catch (e) {
                Request.resolveAxiosError(e);
            }
        },

        async resolveClickCover(){
            try {
                this.$refs.preview.show();
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
        forceUpdateProfilePanel: {},
        getToastManager: {},
    }
}

</script>

<style scoped>

.p-cover-image-frame{
    width: 100%;
    height: 444px;
    max-height: 450px;
    border-radius: 4px;
    overflow: hidden;
    position: relative;
    cursor: pointer;
}

.p-cover-image{
    width: 100%;
    cursor: pointer;
}

.p-edit-cover-image-button{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 16px;
}

</style>

