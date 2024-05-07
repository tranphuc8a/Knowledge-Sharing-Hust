

<template>
    <div class="p-profile-panel-user-avatar">
        <div class="p-avatar-circle">
            <div class="p-avatar-circle_in">
                <div class="p-avatar-image">
                    <img 
                        :src="imageSrc"
                    />
                </div>
                <div class="p-avatar-image-hover">            
                </div>
            </div>
        </div>
        
        <div class="p-avatar-camera" @:click="resolveClickIcon">
            <MIcon 
                fa="camera"
                :iconStyle="iconStyle"
            />
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';


export default {
    name: 'ProfilePanelUserAvatar',
    props: {
    },
    data(){
        return {
            imageSrc: '',
            defaultImageSrc: require('@/assets/default-thumbnail/student-image-icon.png'),
            iconStyle: {
            },
        }
    },
    async mounted(){
        try {
            this.imageSrc = this.defaultImageSrc;
            let avatar = this.getUser()?.Avatar;
            if (await Common.isValidImage(avatar)){
                this.imageSrc = avatar;
            }
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async resolveClickIcon(){
            try {
                console.log("click avatar icon");
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
    }
}

</script>

<style scoped>

.p-profile-panel-user-avatar{
    width: 180px;
    height: 100px;
    position: relative;
}

.p-avatar-circle{
    width: 180px;
    height: 180px;
    border-radius: 100%;
    overflow: hidden;
    cursor: pointer;

    position: absolute;
    bottom: 0;
    left: 0;

    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-avatar-circle_in,
.p-avatar-image,
.p-avatar-image-hover{
    width: 168px;
    height: 168px;
    border-radius: 50%;
    position: absolute;
    overflow: hidden;
}

.p-avatar-image > img{
    width: 168px;
    height: 168px;
}

.p-avatar-image-hover{
    transition: background-color 0.3s ease;
    background-color: rgba(var(--primary-color-400-rgb), 0);
}

.p-avatar-circle_in:hover .p-avatar-image-hover{
    background-color: rgba(var(--primary-color-400-rgb), .24);
}

.p-avatar-camera{
    position: absolute;
    bottom: 0;
    right: 0;
    margin: 12px;
    cursor: pointer;
    width: 36px;
    height: 36px;
    border-radius: 50%;
    background-color: var(--primary-color-100);

    display: flex;
    flex-flow:  row nowrap;
    justify-content: center;
    align-items: center;
}

.p-avatar-camera:hover{
    background-color: var(--primary-color-200);
}

</style>

