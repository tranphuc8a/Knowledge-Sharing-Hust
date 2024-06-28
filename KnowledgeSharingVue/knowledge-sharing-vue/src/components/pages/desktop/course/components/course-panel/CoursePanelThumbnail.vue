

<template>

    <div class="p-thumbnail-frame"
        @:click="resolveClickThumbnail"
        :style="{
            backgroundImage: `url('${this.thumbnail}')`,
            backgroundSize: 'cover',
            backgroundPosition: 'center center'
        }"
    >
        <!-- <img class="p-thumbnail"
            @:click="resolveClickThumbnail"
            :src="thumbnail" alt="thumbnail" /> -->
        <div class="p-edit-thumbnail-button" v-if="isMyCourse">
            <MCancelButton 
                label="Chỉnh sửa ảnh bìa khóa học"
                :onclick="resolveClickButton"
                :buttonStyle="buttonStyle"
                fa="camera" family="fas" :iconStyle="iconStyle"
            />
        </div>
        <PreviewImage :src="thumbnail" ref="preview" v-if="thumbnail != null"/>
    </div>
</template>



<script>
// import CurrentUser from '@/js/models/entities/current-user';
import MCancelButton from '@/components/base/buttons/MCancelButton';
import Common from '@/js/utils/common';
import PreviewImage from '@/components/base/image/PreviewImage.vue';

export default {
    name: 'CoursePanelthumbnail',
    components: {
        MCancelButton, PreviewImage
    },
    props: {
    },
    data(){
        return {
            defaultThumbnail: require('@/assets/default-thumbnail/course-image-icon.png'),
            thumbnail: null,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            currentUser: null,
            isMyCourse: false
        }
    },
    async created(){
        try {
            this.thumbnail = this.defaultThumbnail;
            let thumbnail = this.getCourse()?.Thumbnail;
            if (await Common.isValidImage(thumbnail)){
                this.thumbnail = thumbnail;
            }
            this.isMyCourse = await this.getIsMyCourse();
        } catch (e) {
            console.error(e);
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickButton(){
        },

        async resolveClickThumbnail(){
            try {
                this.$refs.preview.show();
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getIsMyCourse: {},
        getCourse: {},
        forceUpdateCoursePanel: {}
    }
}

</script>

<style scoped>

.p-thumbnail-frame{
    width: 100%;
    height: 444px;
    max-height: 450px;
    border-radius: 4px;
    overflow: hidden;
    position: relative;
    cursor: pointer;
}

.p-thumbnail{
    width: 100%;
    cursor: pointer;
}

.p-edit-thumbnail-button{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 16px;
}

</style>

