

<template>
    <div class="p-profile-edit-general-content">
        <div class="p-peg-card card">
            <div class="p-peg-header">
                <div class="p-peg-heading">
                    Chỉnh sửa ảnh đại diện, ảnh bìa và tên hiển thị 
                </div>
                <div class="p-profile-card-description">
                    Ảnh đại diện và ảnh bìa giúp bạn dễ dàng được nhận diện hơn.
                    Hãy chọn ảnh đại diện và ảnh bìa phù hợp với bạn nhất. <br/>
                    Tên hiển thị sẽ được hiển thị trên trang cá nhân của bạn.
                    Bạn có thể thay đổi tên hiển thị bất cứ lúc nào.
                </div>
            </div>

            <div class="p-peg-avatar">
                <div class="p-peg-sub-header">
                    <div class="p-peg-sub-heading p-profile-card-subheading">
                        Ảnh đại diện
                    </div>
                    <div class="p-peg-button">
                        <MEmbeddedButton 
                            label="Chỉnh sửa"
                            :onclick="resolveOnChangeAvatar"
                        />
                    </div>
                </div>
                <div class="p-peg-content">
                    <ProfilePanelUserAvatar />
                </div>
            </div>

            <div class="p-peg-cover">
                <div class="p-peg-sub-header">
                    <div class="p-peg-sub-heading p-profile-card-subheading">
                        Ảnh bìa
                    </div>
                    <div class="p-peg-button">
                        <MEmbeddedButton 
                            label="Chỉnh sửa"
                            :onclick="resolveOnChangeCover"
                        />
                    </div>
                </div>
                <div class="p-peg-content">
                    <div class="p-peg-cover-image"
                        :style="{backgroundImage: `url(${coverImageUrl})`}"
                        @:click="previewCoverImage"
                    >
                        <div class="p-cover-image-overlay">
                        </div>
                        <div class="p-cover-image-button">
                            <MSecondaryButton 
                                label="Chỉnh sửa ảnh bìa"
                                :buttonStyle="buttonStyle"
                                fa="camera" family="fas" :iconStyle="iconStyle"
                                :onclick="resolveOnChangeCover"
                            />
                        </div>
                        <PreviewImage :src="coverImageUrl" ref="cover"/>
                    </div>
                </div>
            </div>

            <div class="p-peg-fullname">
                <div class="p-peg-sub-header">
                    <div class="p-peg-sub-heading p-profile-card-subheading">
                        Tên hiển thị
                    </div>
                    <div class="p-peg-button">
                        <MEmbeddedButton v-show="true"
                            label="Chỉnh sửa"
                            :onclick="resolveOnChangeFullName"
                        />
                    </div>
                </div>
                <div class="p-peg-content">
                    <div class="p-peg-fullnam-display">
                        {{ fullname }}
                    </div>
                </div>
            </div>
        </div>
        <SelectImagePopup ref="select-image-popup" :on-okay="resolveSubmitImage" :isShow="false"/>
        <ChangeFullnamePopup ref="change-fullname-popup" :isShow="false"/>
    </div>
</template>



<script>
import PreviewImage from '@/components/base/image/PreviewImage.vue';
import ProfilePanelUserAvatar from '../../components/profile-panel/ProfilePanelUserAvatar.vue';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'
import MEmbeddedButton from './../../../../../base/buttons/MEmbeddedButton.vue'
import Common from '@/js/utils/common';
import CurrentUser from '@/js/models/entities/current-user';
import SelectImagePopup from '@/components/base/popup/select-image-popup/SelectImagePopup.vue';
import { Validator } from '@/js/utils/validator';
import { PatchRequest } from '@/js/services/request';
import ChangeFullnamePopup from './ChangeFullnamePopup.vue';

export default {
    name: 'ProfileEditGeneralContent',
    components: {
        ProfilePanelUserAvatar, PreviewImage,
        SelectImagePopup,
        MEmbeddedButton, MSecondaryButton,
        ChangeFullnamePopup
    },
    props: {
    },
    data(){
        return {
            defaultCoverImg: require('@/assets/default-thumbnail/default-cover.jpg'),
            coverImageUrl: null,
            fullname: null,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            imageEnum: {
                AVATAR: 0,
                COVER: 1
            },
            focusImage: -1,
        }
    },
    async mounted(){
        try {
            await this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async refresh(){
            try {
                let currentUser = await CurrentUser.getInstance();
                this.fullname = currentUser.FullName;
                let coverImage = currentUser.Cover;
                if (await Common.isValidImage(coverImage)){
                    this.coverImageUrl = coverImage;
                } else {
                    this.coverImageUrl = this.defaultCoverImg;
                }
            } catch (error){
                console.error(error);
            }
        },


        async startDynamicValidate(){
        },
        async stopDynamicValidate(){
        },
        async getValue(){
        },
        async setValue(){
        },
        async focusError(){
        },
        async validate(){
            return true;
        },

        async resolveOnChangeAvatar(){
            try {
                this.focusImage = this.imageEnum.AVATAR;
                this.$refs['select-image-popup'].show();
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnChangeCover(){
            try {
                this.focusImage = this.imageEnum.COVER;
                this.$refs['select-image-popup'].show();
            } catch (e){
                console.error(e);
            }
        },

        async resolveSubmitImage(image){
            try {
                this.$refs['select-image-popup'].hide();
                if (Validator.isEmpty(image)){
                    return;
                }
                let currentUser = await CurrentUser.getInstance();
                if (this.focusImage == this.imageEnum.AVATAR){
                    await new PatchRequest('Users/me/update-avatar-url')
                        .setBody(image)
                        .execute();
                    this.getToastManager().success('Cập nhật ảnh đại diện thành công');
                    currentUser.Avatar = image;
                } else if (this.focusImage == this.imageEnum.COVER){
                    await new PatchRequest('Users/me/update-cover-url')
                        .setBody(image)
                        .execute();
                    this.getToastManager().success('Cập nhật ảnh bìa thành công');
                    currentUser.Cover = image;
                }
                await CurrentUser.setInstance(currentUser);
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            } catch (e){
                Request.resolveAxiosError(e);
            }
        },

        async resolveOnChangeFullName(){
            try {
                this.$refs['change-fullname-popup'].show();
            } catch (e){
                console.error(e);
            }
        },
        async previewCoverImage(){
            try {
                this.$refs.cover.show();
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getToastManager: {},
    },
    provide(){
        return{}
    }
}

</script>

<style>

.p-profile-edit-subpage .p-profile-card-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.p-profile-edit-subpage .p-profile-card-subheading{
    font-family: 'ks-font-semibold';
    font-size: 20px;
}

.p-profile-edit-subpage .p-profile-card-description{
    font-family: 'ks-font-regular';
    font-size: 15px;
    line-height: 24px;
    color: var(--grey-color-700);
    text-align: justify;
}
</style>


<style scoped>

.p-profile-edit-general-content{
    width: 100%;
}

.p-peg-card{
    padding: 16px;
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 64px;
}

.p-peg-card{
    text-align: left;
}

.p-peg-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
    padding-bottom: 16px;
}

.p-peg-sub-header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
}

.p-peg-button{
    width: fit-content;
}

.p-peg-content{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
}

.p-peg-avatar .p-peg-content{
    margin-top: 64px;
}

.p-peg-avatar, .p-peg-cover{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

.p-peg-cover-image{
    width: 70%;
    aspect-ratio: 16/9;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;

    border-radius: 8px;
    cursor: pointer;
    position: relative;
}

.p-cover-image-overlay{
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.1);
    position: absolute;
    top: 0;
    left: 0;
    border-radius: 8px;
}
.p-cover-image-overlay:hover{
    background-color: rgba(0, 0, 0, 0.5);
}

.p-cover-image-button{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 16px;
}

.p-peg-fullnam-display{
    font-family: 'ks-font-semibold';
    font-size: 32px;
    color: var(--grey-color-700);
    padding-top: 32px;
    padding-bottom: 64px;
}

</style>

