

<template>

    <div class="p-cover-image-frame">
        <img class="p-cover-image"
            @:click="resolveClickCover"
            :src="coverImage" alt="CoverImage" />
        <div class="p-edit-cover-image-button" v-if="isMySelf">
            <MCancelButton 
                label="Chỉnh sửa ảnh bìa"
                :onclick="resolveClickButton"
                :buttonStyle="buttonStyle"
                fa="camera" family="fas" :iconStyle="iconStyle"
            />
        </div>
        <PreviewImage :src="coverImage" ref="preview" />
    </div>
</template>



<script>
import CurrentUser from '@/js/models/entities/current-user';
import MCancelButton from '@/components/base/buttons/MCancelButton';
import Common from '@/js/utils/common';
import PreviewImage from '@/components/base/image/PreviewImage.vue';

export default {
    name: 'ProfilePanelCoverImage',
    components: {
        MCancelButton, PreviewImage
    },
    props: {
    },
    data(){
        return {
            defaultCoverImage: require('@/assets/default-thumbnail/default-cover.jpg'),
            coverImage: null,
            iconStyle: {

            },
            buttonStyle: {

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
        async resolveClickButton(){
            try {
                console.log("click edit cover image button");
            } catch (e) {
                console.error(e);
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
        forceUpdateProfilePanel: {}
    }
}

</script>

<style scoped>

.p-cover-image-frame{
    width: 100%;
    max-height: 450px;
    border-radius: 4px;
    overflow: hidden;
    position: relative;
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

