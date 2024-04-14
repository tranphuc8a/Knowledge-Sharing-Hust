<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <div class="d-content-subpage__left">
                left d-content
                <TooltipFrame>
                    <template #tooltipMask>
                        <MIcon fa="book" />
                    </template>
                    <template #tooltipContent>
                        <div class="p-tooltip-content">
                            <h3 class="p-tooltip-title">Tooltip Title</h3>
                            <p class="p-tooltip-description">Tooltip Description</p>
                        </div>
                    </template> 
                </TooltipFrame>
            </div>

            <div class="d-content-subpage__center">
                <!-- Set a router view for this subpage -->
                <!-- Cannot communicate by props, so -->
                <!-- When navigate on bar, how to know if subpage should refresh ??? -->
                <router-view></router-view>
            </div>

            <div class="d-content-subpage__right">
                <!-- right d-content -->
                <TooltipFrame>
                    <template #tooltipMask>
                        <MIcon fa="book" />
                    </template>
                    <template #tooltipContent>
                        <div class="p-tooltip-content">
                            <h3 class="p-tooltip-title">Tooltip Title</h3>
                            <p class="p-tooltip-description">Tooltip Description</p>
                        </div>
                    </template> 
                </TooltipFrame>
            </div>
        </div>
        
        
    </DesktopHomeFrame>
</template>

<script>
import TooltipFrame from '@/components/base/tooltip/TooltipFrame.vue';
import DesktopHomeFrame from './DesktopHomeFrame.vue';
export default {
    name: "DesktopHomePage",
    data() {
        return {
            title: "Desktop Home Page"
        }
    },
    components: {
        DesktopHomeFrame, TooltipFrame
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