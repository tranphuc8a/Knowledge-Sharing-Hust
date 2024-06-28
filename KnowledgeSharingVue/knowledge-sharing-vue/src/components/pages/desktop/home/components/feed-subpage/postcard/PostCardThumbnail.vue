<template>
    <div class="p-postcard-thumbnail"
        @:click="resolveClickThumbnail"
        :style="{
            backgroundImage: `url('${this.imgSrc}')`,
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            backgroundRepeat: 'no-repeat',
            cursor: 'pointer'
        }"
    >
        <!-- <img :src="imgSrc" class="p-postcard-thumbnail__img" 
            @:click="resolveClickThumbnail"/> -->
        <PreviewImage :src="imgSrc" ref="preview" v-if="imgSrc != null"/>
    </div>
</template>

<script>
import Common from '@/js/utils/common';
import { myEnum } from '@/js/resources/enum';
import PreviewImage from '@/components/base/image/PreviewImage.vue';

export default {
    name: "PostCardThumbnail",
    components: {
        PreviewImage
    },
    data(){
        return {
            imgSrc: null
        }
    },
    mounted(){
        this.updateImageSrc();
    },
    methods: {
        /**
         * Get the thumbnail
         * 
         * @param none
         * @returns none
         * @Created PhucTV (15/04/2024)
         * @Modified None
         */
        async updateImageSrc(){
            try {
                let imgSrc = this.getPost()?.Thumbnail;
                if (! (await Common.isValidImage(imgSrc))){
                    if (this.getPost()?.PostType == myEnum.EPostType.Lesson){
                        imgSrc = require('@/assets/default-thumbnail/lesson-image-icon.png');
                    } else {
                        imgSrc = require('@/assets/default-thumbnail/discussion-image-icon.jpg');
                    }
                } 
                this.imgSrc = imgSrc;
            } catch (e) {
                console.error(e);
            }
        },


        /**
         * Resolve click thumbnail
         * 
         * @param none
         * @returns none
         * @Created PhucTV (15/04/2024)
         * @Modified None
         */
        async resolveClickThumbnail(){
            try {
                this.$refs.preview.show();
            } catch (error) {
                console.error(error);
            }
        }
    },
    inject: {
        getPost: {}
    }
}

</script>

<style scoped>

.p-postcard-thumbnail{
    width: 100%;
    margin: 12px 0px;
    overflow: hidden;
    border-top: solid var(--grey-color-300) 1px;
    border-bottom: solid var(--grey-color-300) 1px;
    cursor: pointer;
    aspect-ratio: 16 / 9;
}

.p-postcard-thumbnail__img{
    width: 100%;
    height: auto;
    cursor: pointer;
/*    object-fit: cover; */
}
</style>
