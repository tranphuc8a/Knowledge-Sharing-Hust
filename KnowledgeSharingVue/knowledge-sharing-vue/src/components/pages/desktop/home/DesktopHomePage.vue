<template>
    <DesktopHomeFrame>
        
        <div class="d-content"
            @:scroll="throttleResolveOnScroll"
            ref="page"
        >
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
                <div class="p-right-page">
                    <HomeRightSubpage />
                </div>
            </div>

            <ScrollToTopPopupButton :onclick="scrollToTop" />
        </div>
        
    </DesktopHomeFrame>
</template>

<script>
import HomeNavigationSubpage from './sub-pages/HomeNavigationSubpage.vue';
import DesktopHomeFrame from './DesktopHomeFrame.vue';
import Debounce from '@/js/utils/debounce';
import ScrollToTopPopupButton from '@/components/base/popup/ScrollToTopPopupButton.vue';
import HomeRightSubpage from './sub-pages/HomeRightSubpage.vue';

export default {
    name: "DesktopHomePage",
    data() {
        return {
            title: "Desktop Home Page",
            handler: [],
            throttleResolveOnScroll: Debounce.throttle(this.resolveOnScroll.bind(this), 1000),
        }
    },
    components: {
        DesktopHomeFrame, HomeNavigationSubpage,
        ScrollToTopPopupButton,
        HomeRightSubpage,
    },
    methods: {
        registerScrollHandler(handler){
            this.handler.push(handler);
        },
        async resolveOnScroll(){
            try {
                await Promise.all(this.handler.map(handler => handler(this.$refs.page)));
            } catch (error) {
                console.error(error);
            }
        },

        async scrollToTop(){
            try {
                /* Manual */
                this.$refs.page.scrollTop = 0; // Set the scrollTop of the element to 0

                // If the above does not work, try using scrollIntoView
                this.$refs.page.scrollIntoView({ behavior: 'smooth', block: 'start' });

                /* First lib */
                // this.$scrollTo(this.$refs.page, 0, {
                //     duration: 500,
                //     // easing: 'easeInOutQuad',
                // })

                /* Second Lib */
                // this.$smoothScroll({
                //     scrollTo: this.$refs.page,
                //     duration: 1000,
                //     offset: 16,
                // });
            } catch (error) {
                console.error(error);
            }
        }
    },
    props: {

    },
    provide(){
        return {
            registerScrollHandler: this.registerScrollHandler
        }
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
.p-right-page{
    width: 100%;
    height: 100%;
    max-width: 100%;
    max-height: 100%;
}
</style>