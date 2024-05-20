

<template>
    <div class="p-profile-subpage">
        <div class="pps-image-card card">
            <div class="pps-image-card__header">
                <div class="pps-header__left">
                    <span>Ảnh</span>
                </div>
                <div class="pps-header__right">
                    <MSecondaryButton 
                        label="Xem tất cả ảnh"
                        :onclick="resolveClickViewAllImages"
                        :buttonStyle="buttonStyle"
                    />
                </div>
            </div>
            <div class="pps-list-images">
                <div class="pps-list-images-container">
                    <div class="pps-image-item"
                        v-for="(image, index) in filteredImages"
                        :key="image.ImageId ?? index"
                        @:click="resolveClickImage(index)"
                    >
                        <div class="pps-image-container"
                            :style="{backgroundImage: `url(${image.ImageUrl})`}"
                        > </div>
    
                        <div class="pps-image-overlay"> </div>
                    </div>
                </div>
            </div>
            <PreviewImageGroup 
                :srcs="(filteredImages ?? []).map(it => it.ImageUrl)" 
                ref="preview"
            />
        </div>
    </div>
</template>



<script>
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';
import PreviewImageGroup from '@/components/base/image/PreviewImageGroup.vue';
import Image from '@/js/models/entities/image';
import { GetRequest, Request } from '@/js/services/request';
import { MyDate } from '@/js/utils/mydate';
import { useRouter } from 'vue-router';
import CurrentUser from '@/js/models/entities/current-user';


export default {
    name: 'ProfileHomeImageSubpage',
    components: {
        MSecondaryButton,
        PreviewImageGroup
    },
    props: {
    },
    data(){
        return {
            filteredImages: [],
            router: useRouter(),
            buttonStyle: {
                
            }
        }
    },
    async created(){
        await this.refreshImages();
    },
    mounted(){

    },
    methods: {
        async resolveClickImage(index){
            try {
                this.$refs.preview.show(index);
            } catch (e){
                console.error(e);
            }
        },

        async refreshImages(){
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
                    }).slice(0, 9);
                this.filteredImages = listImages;
            } catch (e){
                Request.resolveAxiosError(e);
            }
        },

        async resolveClickViewAllImages(){
            try {
                let currentUser = await CurrentUser.getInstance();
                let username = currentUser?.Username;
                if (username == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.router.push(`/profile/${username}/image`);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getPopupManager: {},
        getIsMySelf: {}
    }
}

</script>

<style scoped>

.pps-image-card{
    padding: 16px;
    gap: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    width: 100%;
}

.pps-image-card__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.pps-header__left{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.pps-list-images{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    width: 100%;
    border-radius: 8px;
    overflow: hidden;
}

.pps-list-images-container{
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: flex-start;
    width: 100%;
    gap: 6px;
}

.pps-image-item{
    width: calc((100% - 12px) / 3);
    flex-shrink: 1;
    flex-grow: 0;
    aspect-ratio: 1 / 1;
    cursor: pointer;
    position: relative;
}

.pps-image-overlay{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0);
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.pps-image-item:hover .pps-image-overlay{
    background-color: rgba(0, 0, 0, 0.5);
}

.pps-image-container{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center center;
}


</style>

