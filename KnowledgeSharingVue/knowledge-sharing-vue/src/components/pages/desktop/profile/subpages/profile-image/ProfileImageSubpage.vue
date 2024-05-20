

<template>
    <div class="p-profile-image-subpage p-profile-main-subpage">
        <div class="p-profile-image-card card">
            <div class="p-profile-image-header">
                <div class="p-pfh-heading">
                    Hình ảnh
                </div>
                <div class="p-pfh-search-button">
                    <!-- Upload Image Button -->
                    <MButton 
                        fa="plus" 
                        label="Thêm ảnh"
                        :onclick="resolveClickAddImage"/>
                </div>
            </div>


            <!-- SKELETON -->
            <div class="p-profile-image-content" v-if="!isLoaded">
                <div class="p-pi-image-list">
                    <div class="ppi-image-item skeleton"
                        v-for="(index) in [1, 2, 3, 4, 5]" :key="index"
                        style="border-radius: 8px; aspect-ratio: 1 / 1;"
                    >      
                    </div>
                </div>
            </div>

            <div class="p-profile-empty-image" v-if="isLoaded && !(listImage.length > 0)">
                <NotFoundPanel 
                    text="Chưa có ảnh nào"
                />
            </div>

            <!-- CONTENT -->
            <div class="p-profile-image-content" v-if="isLoaded">
                <div class="p-pi-image-list">
                    <div class="ppi-image-item"
                        v-for="(image, index) in filteredImage"
                        :key="image.ImageId ?? index"
                        @:click="resolveClickImage(index)"
                    >      
                        <ProfileImageItem 
                            :image="image"
                            :onImageDeleted="resolveDeletedImageFunction(index)"
                        />
                    </div>

                </div>
                <div class="p-pi-pagination">
                    <a-pagination
                        v-model:current="current"
                        v-model:pageSize="pageSize"
                        :total="total" />
                </div>
            </div>
        </div>

        <!-- Preview images -->
        <PreviewImageGroup 
            :srcs="(filteredImage ?? []).map(it => it.ImageUrl)" 
            ref="preview"
        />

        <!-- Upload image popup -->
        <ProfileUploadImagePopup :is-show="false" ref="upload-popup" />

    </div>
</template>



<script>
// import CurrentUser from '@/js/models/entities/current-user';
import PreviewImageGroup from '@/components/base/image/PreviewImageGroup.vue';
import ProfileImageItem from '../../components/profile-image/ProfileImageItem.vue';
import MButton from './../../../../../base/buttons/MButton.vue'
import ProfileUploadImagePopup from '../../components/profile-image/ProfileUploadImagePopup.vue';
import { GetRequest, Request } from '@/js/services/request';
import { MyDate } from '@/js/utils/mydate';
import Image from '@/js/models/entities/image';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';

export default {
    name: 'ProfileImageSubpage',
    components: {
        PreviewImageGroup,
        ProfileImageItem,
        MButton, NotFoundPanel,
        ProfileUploadImagePopup
    },
    props: {
    },
    data(){
        return {
            listImage: [],
            filteredImage: [],
            isLoaded: false,

            // a pagination:
            current: 1,
            total: 0,
            pageSize: 18,
        }
    },
    watch: {
        current(){
            this.resolveChangePage();
        }
    },
    async created(){
        this.refresh();
    },
    async mounted(){
    },
    methods: {
        async refresh(){
            try {
                this.isLoaded = false;
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

                // pagintion updating:
                this.current = 1;
                this.total = listImages.length;
                this.pageSize = 18;
                this.filteredImage = listImages.slice(0, this.pageSize);
            } catch (error){
                Request.resolveAxiosError(error);
            }
        },


        async resolveClickImage(index){
            try {
                this.$refs.preview.show(index);
            } catch (e){
                console.error(e);
            }
        
        },

        async resolveChangePage(){
            try {
                this.total = this.listImage.length;
                let start = (this.current - 1) * this.pageSize;
                let end = start + this.pageSize;
                this.filteredImage = this.listImage.slice(start, end);
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickAddImage(){
            try {
                this.$refs['upload-popup'].show();
            } catch (e){
                console.error(e);
            }
        },

        resolveDeletedImageFunction(index){
            let that = this;
            return async function(){
                try {
                    let image = that.filteredImage[index];
                    let indexInTotal = that.listImage.indexOf(image);
                    if (indexInTotal >= 0){
                        that.listImage.splice(indexInTotal, 1);
                    }
                    await that.resolveChangePage();
                } catch (e){
                    console.error(e);
                }
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

@import url(@/css/pages/desktop/profile/profile-image-subpage/profile-image-subpage.css);

</style>

