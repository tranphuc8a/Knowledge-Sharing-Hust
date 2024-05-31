

<template>
    <MPopup  ref="popup"
            :isShowDescription="true"
            :isShowPreviousButton="false"
            :isShowCancelButton="true"
            :isShowOkayButton="true"
            :isAutoHide="false"
            v-show="dIsShow"
        
            :on-okay="callbacks.onOkay"
            :on-close="callbacks.onClose"
            :on-cancel="callbacks.onCancel"
            :on-previous="callbacks.onPrevious"

            header="Chọn ảnh"
            description="Bạn có thể phiêu lưu vào thư viện ảnh của mình như một nhà thám hiểm, tìm kiếm kho báu bị lãng quên. Nếu bạn muốn, hãy nhập URL và kéo ảnh từ đại dương Internet bao la như một ngư dân thời công nghệ. Hoặc nếu bạn cảm thấy nghệ sĩ trong mình đang trỗi dậy, hãy tải lên một kiệt tác mới toanh từ điện thoại hay máy tính của bạn."
            cancelButtonLabel="Thôi không chọn nữa"
            okayButtonLabel="Chốt lấy cái này"
    >
        <div class="p-image-popup-content">
            <div class="p-sip-nav">
                <SelectImagePopupNavigation :on-change-tab="changeTabIndex" />
            </div>
            <div class="p-sip-content">

                <SelectImageFromGalleryContent 
                    v-show="tabIndex === tabEnum.Gallery" 
                    v-if="isListImageCreated" 
                    ref="selectImageFromGalleryContent"
                    />

                <EnterImageUrlContent 
                    v-show="tabIndex === tabEnum.EnterUrl" 
                    ref="enterImageUrlContent"
                    />

                <UploadImageContent
                    v-show="tabIndex === tabEnum.Upload" 
                    ref="uploadImageContent"
                    />

            </div>
        </div>
    </MPopup>
</template>



<script>
import MPopup from './../MPopup.vue'
import SelectImagePopupNavigation from './SelectImagePopupNavigation.vue'
import SelectImageFromGalleryContent from './SelectImageFromGalleryContent.vue'
import EnterImageUrlContent from './EnterImageUrlContent.vue'
import UploadImageContent from './UploadImageContent.vue'
import { PostRequest, Request } from '@/js/services/request'



export default {
    name: 'SelectImagePopup',
    components: {
        MPopup,
        SelectImagePopupNavigation,
        SelectImageFromGalleryContent,
        EnterImageUrlContent,
        UploadImageContent,
    },
    props: {
        onOkay: {
            type: Function,
            default: () => {}
        },
        isShow: {
            type: Boolean,
            default: false
        },
    },
    data(){
        return {
            callbacks: {
                onOkay: this.resolveClickOkay.bind(this),
                onClose: async () => { await this.hide() },
                onCancel: async () => { await this.hide() },
                onPrevious: () => {}
            },
            tabEnum: {
                Gallery: 0,
                EnterUrl: 1,
                Upload: 2
            },
            tabIndex: 0,
            isListImageCreated: false,
            dIsShow: this.isShow
        }
    },
    async mounted(){
    },
    methods: {
        async show(){
            this.dIsShow = true;
            this.isListImageCreated = true
        },
        async hide(){
            this.dIsShow = false;
        },

        changeTabIndex(index){
            this.tabIndex = index;
            if (index === this.tabEnum.Gallery){
                this.isListImageCreated = true;
            }
        },

        async resolveClickOkay(){
            let result = null;
            if (this.tabIndex === this.tabEnum.Gallery){
                result = await this.$refs.selectImageFromGalleryContent.getSelectedImage();
                let imageUrl = result?.ImageUrl;
                if (imageUrl == null){
                    await this.resolveImageInvalid();
                    return;
                }
                await this.onOkay(imageUrl);
            } else if (this.tabIndex === this.tabEnum.EnterUrl){
                result = await this.$refs.enterImageUrlContent.getSelectedImage();
                if (result == null){
                    await this.resolveImageInvalid();
                    return;
                }
                await this.onOkay(result);
            } else if (this.tabIndex === this.tabEnum.Upload){
                let { image, captcha } = await this.$refs.uploadImageContent.getSelectedImage();
                if (image == null){
                    await this.resolveImageInvalid();
                    return;
                }
                let imageUrl = await this.resolveUploadImage(image, captcha);
                if (imageUrl == null){
                    await this.resolveImageInvalid();
                    return;
                }
                await this.onOkay(imageUrl);
            }
        },

        async resolveImageInvalid(){
            try {
                this.getToastManager().error('Upload ảnh không thành công. Vui lòng thử lại.');
            } catch (error){
                console.error(error);
            }
        },

        async resolveUploadImage(image, recaptchaToken){
            try {
                let res = await new PostRequest('Images')
                    .setRecaptchaResponseToken(recaptchaToken)
                    .prepareFormData()
                    .addFormData('Image', image)
                    .execute();
                let body = await Request.tryGetBody(res);
                let imageUrl = body?.ImageUrl;
                this.isListImageCreated = false;
                return imageUrl;
            } catch (error){
                Request.resolveAxiosError(error);
                return null;
            }
        }
    },
    inject: {
        getToastManager: {}
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-image-popup-content {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
    gap: 16px;
}

.p-sip-nav {
    width: 100%;
}

.p-sip-content {
    width: 100%;
}



</style>

