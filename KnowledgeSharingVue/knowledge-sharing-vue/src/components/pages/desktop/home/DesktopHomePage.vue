<template>
    <DesktopHomeFrame>
        
        <div class="d-content">
            <!-- <div class="d-content-subpage__left_mask">
            </div> -->
            <div class="d-content-subpage__left_content">
                <div class="p-left-page">
                    <HomeNavigationSubpage />
                </div>
            </div>

            <div class="d-content-subpage__center">
                <!-- Set a router view for this subpage -->
                <!-- Cannot communicate by props, so -->
                <!-- When navigate on bar, how to know if subpage should refresh ??? -->
                <router-view></router-view>
            </div>

            <!-- <div class="d-content-subpage__right_mask">
            </div> -->
            <div class="d-content-subpage__right_content">
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
import HomeNavigationSubpage from './sub-pages/HomeNavigationSubpage.vue';
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
        DesktopHomeFrame, TooltipFrame, HomeNavigationSubpage
    },
    methods: {

    },
    props: {

    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
@import url(@/css/pages/desktop/home/desktop-homepage.css);

.d-content-subpage__left_content{
    padding-left: 16px;
}
.p-left-page{
    width: 100%;
    padding: 12px 8px;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1),
                0 2px 4px 0 rgba(0, 0, 0, 0.06);
    width: 100%;
}
</style>