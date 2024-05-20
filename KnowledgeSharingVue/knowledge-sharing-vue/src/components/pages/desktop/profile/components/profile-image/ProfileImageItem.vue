

<template>
    <div class="p-profile-image-item"
        :style="{backgroundImage: `url(${image.ImageUrl})`}"
    >
        <div class="p-image-overlay"> </div>

        <div class="p-edit-image">
            <MMenuContextPopup :options="getOptions()">
                <MActionIcon fa="ellipsis" :onclick="null"  />
            </MMenuContextPopup>
        </div>
    </div>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import { DeleteRequest } from '@/js/services/request';

export default {
    name: 'ProfileImageItem',
    components: {
        MMenuContextPopup
    },
    props: {
        image: {
            required: true,
        },
        onImageDeleted: {
            type: Function,
            default: async () => {}
        }
    },
    data(){
        return {
        }
    },
    async mounted(){
    },
    methods: {
        getOptions(){
            return [
                {
                    fa: 'trash-can',
                    onclick: this.resolveDeleteImage.bind(this),
                    label: 'Xóa ảnh'
                },
            ]
        },

        async resolveDeleteImage(){
            try {
                let alertMsg = "Bạn có chắc chắn muốn xóa ảnh này không?";
                this.getPopupManager().inform(alertMsg, this.submitDeleteImage.bind(this));
            } catch (error) {
                console.error(error);
            }
        },


        async submitDeleteImage(){
            try {
                let imageId = this.image?.ImageId;
                if (imageId == null) return;
                await new DeleteRequest('Images/' + imageId)
                    .execute();
                this.getToastManager().success("Xóa ảnh thành công");
                if (this.onImageDeleted != null){
                    await this.onImageDeleted();
                }
            } catch (e){
                Request.resolveAxiosError(e);
            }
        }

    },
    inject: {
        getPopupManager: {},
        getToastManager: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-image-item{
    width: 100%;
    aspect-ratio: 1 / 1;
    border-radius: 8px;
    background-size: cover;
    background-position: center center;
    position: relative;
}

.p-image-overlay{
    width: 100%;
    height: 100%;
    position: absolute;
    top: 0;
    left: 0;
    border-radius: 8px;
    background-color: rgba(0, 0, 0, 0.5);
    cursor: pointer;
    opacity: 0;
    transition: opacity 0.3s ease-in-out;
}

.p-profile-image-item:hover .p-image-overlay,
.p-profile-image-item:hover .p-edit-image{
    opacity: 1;
}

.p-edit-image{
    position: absolute;
    opacity: 0;
    top: 0;
    right: 0;
    margin: 8px;
}

</style>

