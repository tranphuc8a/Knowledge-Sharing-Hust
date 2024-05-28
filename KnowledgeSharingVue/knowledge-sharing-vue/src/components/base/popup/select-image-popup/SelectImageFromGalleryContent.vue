

<template>
    <div class="p-select-image-from-gallery-content">
        <div class="p-sifg-not-found"
            v-show="isLoaded && !(listImage.length > 0)"
        >
            <NotFoundPanel text="Không có ảnh nào"/>
        </div>

        <div class="p-sifg-list-image">
            
            <div class="p-image-item-frame p-radio-button"
                v-for="(image, index) in listImage"
                :key="image.ImageId ?? index"
            >
                <label class="p-image-item-label">
                    <div class="p-image-item"
                        :style="{ backgroundImage: `url(${image.ImageUrl})` }"
                    >
                        <div class="p-image-item-overlay"></div>
                        <div class="p-radio-button-mask-frame">
                            <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                        </div>
                    </div>

                    <input name="p-select-image-popup-radio-button-group" :value="image" 
                        v-model="value" type="radio" class="p-radio-button-org" />
                </label>
            </div>

            <div class="p-image-item-frame skeleton" v-show="!isLoaded"></div>
            <div class="p-image-item-frame skeleton" v-show="!isLoaded"></div>
            <div class="p-image-item-frame skeleton" v-show="!isLoaded"></div>
        </div>
    </div>

    
</template>



<script>
import { GetRequest, Request } from '@/js/services/request';
import Image from '@/js/models/entities/image';
import NotFoundPanel from '../NotFoundPanel.vue';
import { MyDate } from '@/js/utils/mydate';

export default {
    name: 'SelectImageFromGalleryContent',
    components: {
        NotFoundPanel
    },
    props: {
    },
    data(){
        return {
            value: null,
            listImage: [],
            isLoaded: false,
        }
    },
    async created(){
        await this.reloadImage();
    },
    async mounted(){
    },
    methods: {
        async reloadImage(){
            try {
                let res = await new GetRequest('Images').execute();
                let body = await Request.tryGetBody(res);
                let listImages = body
                    .map(function(image){
                        return new Image().copy(image);
                    }).sort(function(a, b){
                        let createdTimeA = a.CreatedTime != null ? new MyDate(a.CreatedTime) : 0;
                        let createdTimeB = b.CreatedTime != null ? new MyDate(b.CreatedTime) : 0;
                        return createdTimeB - createdTimeA;
                    });
                this.listImage = listImages;
                this.isLoaded = true;
            } catch (error) {
                console.error(error);
            }
        },

        async getSelectedImage(){
            return this.value;
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
.p-select-image-from-gallery-content{
    max-width: 100%;
    width: 100%;
    padding: 0;
    height: 450px;
    overflow: scroll;
}

.p-sifg-list-image{
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-image-item-frame{
    width: calc((100% - 5*16px) / 6);
    aspect-ratio: 1 / 1;
    background-color: #f0f0f0;
    border-radius: 8px;
    position: relative;
    overflow: hidden;
}

.p-image-item-label{
    display: block;
    width: 100%;
    height: 100%;
    position: relative;
}

.p-image-item{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    border-radius: 8px;
    position: relative;
}

.p-image-item-overlay{
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.1);
    position: absolute;
    top: 0;
    left: 0;
}

.p-image-item-overlay:hover{
    background-color: rgba(0, 0, 0, 0.3);
}

.p-radio-button-mask-frame{
    width: fit-content;
    height: fit-content;
    position: absolute;
    top: 0;
    left: 0;
    margin: 16px;
    display: flex;
    justify-content: center;
    align-items: center;
}

.p-radio-button-mask{
    width: 24px;
    height: 24px;
    background-color: rgba(0, 0, 0, 0.5);
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
}

.p-radio-button-dot{
    width: 12px;
    height: 12px;
    background-color: white;
    border-radius: 50%;
}

.p-radio-button-org{
    display: none;
}

.p-radio-button-org:checked + .p-image-item .p-radio-button-mask-frame .p-radio-button-mask{
    background-color: #007bff;
}

.p-radio-button-org:checked + .p-image-item .p-radio-button-mask-frame .p-radio-button-mask .p-radio-button-dot{
    background-color: white;
}

.p-sifg-not-found{
    width: 100%;
    height: 100%;
    min-height: 200px;
    display: flex;
    justify-content: center;
    align-items: center;
}



</style>

