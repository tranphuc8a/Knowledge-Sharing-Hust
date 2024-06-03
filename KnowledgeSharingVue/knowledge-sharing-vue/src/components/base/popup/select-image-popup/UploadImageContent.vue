

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
        <div class="p-uic-captcha">
            <div class="g-recaptcha" :data-sitekey="siteKey" ref="grecaptcha"></div>
        </div>
    </div>
</template>



<script>
/* global grecaptcha */
import MImageInput from './../../inputs/MImageInput.vue';
import Common from '@/js/utils/common';
import appConfig from '@/app-config';
import { MyRandom } from '@/js/utils/myrandom';

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
                width: '300px', 
                height: '300px',
            },
            siteKey: appConfig.getCaptchaSiteKey(),
            captchaId: MyRandom.generateUUID()
        }
    },
    async mounted(){
        let that = this;
        this.$nextTick(() => {
            Common.loadRecaptchaScript();
            that.recaptchaWidgetId = grecaptcha.render(that.$refs.grecaptcha, {
                'sitekey' : that.siteKey,
                'callback' : function(response) {
                    console.log(response);
                }
            });
        });
    },
    methods: {
        async getSelectedImage(){
            try {
                let image = await this.$refs.imageInput.getValue();
                let captcha = grecaptcha.getResponse(this.recaptchaWidgetId);
                return { image, captcha };
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
    justify-content: space-around;
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

.p-uic-captcha{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
}

</style>

