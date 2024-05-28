

<template>
    <div class="p-upload-image-content">
        <div class="p-uic-upload-frame">
            <MImageInput 
                :style="inputImageStyle"
                title="Chọn một ảnh phù hợp"
                :is-show-title="false" :is-show-error="false" :is-obligate="false"
                :validator="null"
                state="normal" ref="imageInput"
            />
        </div>
    </div>
</template>



<script>
import MImageInput from './../../inputs/MImageInput.vue'


export default {
    name: 'UploadImageContent',
    components: {
        MImageInput
    },
    props: {
    },
    data(){
        return {
            inputImageStyle: { 
                width: '350px', 
                height: '350px',
            }
        }
    },
    async mounted(){
    },
    methods: {
        async getSelectedImage(){
            try {
                let image = await this.$refs.imageInput.getValue();
                return image;
            } catch (error) {
                console.error(error);
                return null;
            }
        }
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-upload-image-content{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 450px;
}

.p-uic-upload-frame{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: fit-content;
    height: 100%;
}

</style>

