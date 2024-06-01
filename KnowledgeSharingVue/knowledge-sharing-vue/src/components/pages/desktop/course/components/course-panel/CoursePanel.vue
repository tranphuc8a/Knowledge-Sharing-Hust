


<template>
    <div class="p-course-panel" v-if="!isLoaded">
        <GradientBackground :src="null" />
        <div class="p-course-panel-content">
            <div class="p-cover-image">
                <CoursePanelThumbnail />
            </div>
    
            <div class="p-information-panel">
                <CoursePanelInformation />
    
                <div class="p-course-panel-devider">
                    <div></div>
                </div>
                <div class="p-course-panel-navigations">
                    <CoursePanelNavigation />
                </div>
            </div>
        </div>
    </div>
    
    <div class="p-course-panel" v-if="isLoaded">
        <GradientBackground :src="getCourse()?.Cover" />
        <div class="p-course-panel-content">
            <div class="p-cover-image">
                <CoursePanelThumbnail />
            </div>
    
            <div class="p-course-information-panel">
                <CoursePanelInformation />
    
                <div class="p-course-panel-devider">
                    <div></div>
                </div>
                <div class="p-course-panel-navigations">
                    <CoursePanelNavigation />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import CoursePanelThumbnail from './CoursePanelThumbnail.vue';
import CoursePanelInformation from './CoursePanelInformation.vue';
import CoursePanelNavigation from './CoursePanelNavigation.vue';
import GradientBackground from '@/components/base/image/GradientBackground.vue';

export default {
    name: 'CoursePanel',
    components: {
        CoursePanelThumbnail,
        CoursePanelInformation,
        CoursePanelNavigation,
        GradientBackground
    },
    props: {
    },
    data(){
        return {
            isLoaded: true,
            
        }
    },
    mounted(){

    },
    methods: {
        async forceRender(){
            try {
                let that = this;
                this.isLoaded = false;
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
            forceUpdateCoursePanel: this.forceRender,
        }
    },
    inject: {
        getCourse: {},
        getPopupManager: {},
        getToastManager: {},
    },
}

</script>

<style scoped>

.p-course-panel{
    width: 100%;
    background-color: white;
    position: relative;
}
.p-course-panel-content{
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

.p-course-information-panel{
    width: 100%;
    max-width: var(--course-page-max-width);
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-course-panel-devider{
    width: 100%;
    margin-top: 8px;
    height: 1.75px;
    background-color: var(--primary-color-200);
}

.p-course-panel-navigations{
    width: 100%;
}


</style>

