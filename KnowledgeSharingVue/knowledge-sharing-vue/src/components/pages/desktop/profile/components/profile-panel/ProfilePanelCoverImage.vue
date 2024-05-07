

<template>

    <div class="p-cover-image-frame">
        <img class="p-cover-image"
            :src="coverImage" alt="CoverImage" />
        <div class="p-edit-cover-image-button">
            <MCancelButton 
                label="Chỉnh sửa ảnh bìa"
                :onclick="resolveClickButton"
                :buttonStyle="buttonStyle"
                fa="camera" family="fas" :iconStyle="iconStyle"
            />
        </div>
    </div>
</template>



<script>

import MCancelButton from '@/components/base/buttons/MCancelButton';
import Common from '@/js/utils/common';

export default {
    name: 'ProfilePanelCoverImage',
    components: {
        MCancelButton
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

            }
        }
    },
    async created(){
        try {
            this.coverImage = this.defaultCoverImage;
            let cover = this.getUser()?.Cover;
            if (await Common.isValidImage(cover)){
                this.coverImage = cover;
            }
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
}

.p-edit-cover-image-button{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 16px;
}

</style>

