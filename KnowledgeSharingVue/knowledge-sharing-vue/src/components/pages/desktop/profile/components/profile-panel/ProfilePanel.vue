


<template>
    <div class="p-profile-panel" v-if="!isLoaded">
        <GradientBackground :src="null" />
        <div class="p-profile-panel-content">
            <div class="p-cover-image">
                <ProfilePanelCoverImage />
            </div>
    
            <div class="p-user-panel">
                <ProfilePanelInformation />
    
                <div class="p-profile-panel-devider">
                    <div></div>
                </div>
                <div class="p-profile-panel-navigations">
                    <ProfilePanelNavigation />
                </div>
            </div>
        </div>
    </div>
    
    <div class="p-profile-panel" v-if="isLoaded">
        <GradientBackground :src="getUser()?.Cover" />
        <div class="p-profile-panel-content">
            <div class="p-cover-image">
                <ProfilePanelCoverImage />
            </div>
    
            <div class="p-user-panel">
                <ProfilePanelInformation />
    
                <div class="p-profile-panel-devider">
                    <div></div>
                </div>
                <div class="p-profile-panel-navigations">
                    <ProfilePanelNavigation />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import ProfilePanelCoverImage from './ProfilePanelCoverImage.vue';
import ProfilePanelInformation from './ProfilePanelInformation.vue';
import ProfilePanelNavigation from './ProfilePanelNavigation.vue';
import GradientBackground from '@/components/base/image/GradientBackground.vue';

export default {
    name: 'ProfilePanel',
    components: {
        ProfilePanelCoverImage,
        ProfilePanelInformation,
        ProfilePanelNavigation,
        GradientBackground
    },
    props: {
    },
    data(){
        return {
            isLoaded: true,
            title: '',
            isLessonExisted: false,
            lesson: null
        }
    },
    mounted(){

    },
    methods: {
        async forceRender(){
            try {
                this.isLoaded = false;
                let that = this;
                this.$nextTick(() => {
                    that.isLoaded = true;
                });
            } catch (e) {
                console.error(e);
            }
        
        }
    },
    provide(){
        return {
            forceUpdateProfilePanel: this.forceRender,
        }
    },
    inject: {
        getUser: {},
        getPopupManager: {},
        getToastManager: {},
    },
}

</script>

<style scoped>

.p-profile-panel{
    width: 100%;
    background-color: white;
    position: relative;
}
.p-profile-panel-content{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    position: relative;
}
.p-cover-image{
    width: 100%;
    max-width: 1250px;
}

.p-user-panel{
    width: 100%;
    max-width: var(--profile-page-max-width);
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-profile-panel-devider{
    width: 100%;
    margin-top: 8px;
    height: 1.75px;
    background-color: var(--primary-color-200);
}

.p-profile-panel-navigations{
    width: 100%;
}


</style>

