

<template>
    <div class="p-enter-image-url-content">
        <div class="p-eiuc-textfield">
            <div class="p-textfield-frame">
                <MTextfieldButton 
                    placeholder="Nhập URL ảnh của bạn vào đây"
                    :is-show-icon="true" 
                    :is-show-title="false" 
                    :is-show-error="false" 
                    :is-obligate="false"
                    :onclick-icon="resolveClickSubmitUrl" 
                    :validator="null"
                    :oninput="resolveInputUrl"
                    state="normal"
                />
            </div>

        </div>
        <div class="card-description">
            Ảnh của bạn sẽ được hiển thị ở đây
        </div>
        <div class="p-eius-view-image">
            <div class="p-eius-image" v-show="true"
                :style="{ backgroundImage: `url(${imageUrl})` }"
            >
            </div>
            <div class="p-eius-image-invalid" v-show="false">
                Url không hợp lệ
            </div>
        </div>
    </div>
</template>



<script>
import MTextfieldButton from './../../inputs/MTextfieldButton.vue'

export default {
    name: 'EnterImageUrlContent',
    components: {
        MTextfieldButton
    },
    props: {
    },
    data(){
        return {
            imageUrl: null,
            isImageUrlValid: false,
        }
    },
    async mounted(){
    },
    methods: {
        async resolveClickSubmitUrl(){
        },
        async resolveInputUrl(value){
            this.imageUrl = value;
        },
        async getSelectedImage(){
            return this.imageUrl;
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

.p-enter-image-url-content{
    width: 100%;
    max-width: 100%;
    height: 450px;
    overflow: auto;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

.p-eiuc-textfield{
    width: 100%;
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.p-textfield-frame{
    width: 100%;
    max-width: 400px;
}

.card-description{
    margin-top: 20px;
    font-size: 14px;
    color: #666;
}

.p-eius-view-image{
    width: 100%;
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.p-eius-image{
    width: 300px;
    height: 300px;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    background-color: var(--primary-color-100);
    border-radius: 8px;
}

.p-eius-image-invalid{
    width: 300px;
    height: 300px;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #f0f0f0;
    color: #666;
    font-size: 14px;
}

</style>

