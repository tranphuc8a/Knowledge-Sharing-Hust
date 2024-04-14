
<template>
    <div class="d-bar">
        <div class="d-bar__left">
            <LeftDesktopBar />
        </div>
        <div class="d-bar__center">
            <CenterDesktopBar />
        </div>
        <div class="d-bar__right">
            <RightDesktopBar />
        </div>
    </div>
</template>

<script>
import LeftDesktopBar from './desktop-bar/LeftDesktopBar.vue';
import RightDesktopBar from './desktop-bar/RightDesktopBar';
import CenterDesktopBar from './desktop-bar/CenterDesktopBar.vue';
export default {
    name: "DesktopBar",
    data() {
        return {
            title: ""
        }
    },
    components: {
        LeftDesktopBar, RightDesktopBar, CenterDesktopBar
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
@import url(@/css/pages/desktop/home/components/desktop-bar.css);
</style>