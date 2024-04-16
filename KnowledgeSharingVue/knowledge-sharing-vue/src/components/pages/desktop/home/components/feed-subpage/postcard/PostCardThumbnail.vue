<template>
    <div class="p-postcard-thumbnail">
        <img :src="imgSrc" class="p-postcard-thumbnail__img" @:click="resolveClickThumbnail"/>
    </div>
</template>

<script>
import Common from '@/js/utils/common';
import { myEnum } from '@/js/resources/enum';

export default {
    name: "PostCardThumbnail",
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
                let imgSrc = this.post?.Thumbnail;
                if (! (await Common.isValidImage(imgSrc))){
                    if (this.post?.PostType == myEnum.EPostType.Lesson){
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
                let router = this.globalData.router;
                router.push({name: 'post-detail', params: {id: this.post.Id}});
            } catch (error) {
                console.log(error);
            }
        }
    },
    injects: {
        post: {
            default: null
        }
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
}

.p-postcard-thumbnail__img{
    width: 100%;
    height: auto;
    cursor: pointer;
/*    object-fit: cover; */
}
</style>
