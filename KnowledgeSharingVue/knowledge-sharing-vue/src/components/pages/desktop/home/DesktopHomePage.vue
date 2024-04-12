<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <div class="d-content-subpage__left">
                left d-content
            </div>

            <div class="d-content-subpage__center">
                center d-content
                <!-- Set a router view for this subpage -->
                <!-- Cannot communicate by props, so -->
                <!-- When navigate on bar, how to know if subpage should refresh ??? -->
            </div>

            <div class="d-content-subpage__right">
                right d-content
            </div>
        </div>
        
        
    </DesktopHomeFrame>
</template>

<script>
import DesktopHomeFrame from './DesktopHomeFrame.vue';
export default {
    name: "DesktopHomePage",
    data() {
        return {
            title: "Desktop Home Page"
        }
    },
    components: {
        DesktopHomeFrame
    },
    methods: {

    },
    props: {

    },
    inject: {
        inject: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
@import url(@/css/pages/desktop/home/desktop-homepage.css);
</style>