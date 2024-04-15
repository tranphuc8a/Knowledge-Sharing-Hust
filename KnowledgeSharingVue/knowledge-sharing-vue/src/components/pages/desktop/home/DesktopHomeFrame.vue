<template>
    <div class="d-home">
        <slot />

        <DesktopBar />
    </div>
</template>

<script>
import DesktopBar from "./components/DesktopBar.vue";
export default {
    name: "DesktopHomeFrame",
    data() {
        return {
            title: "Home Frame"
        }
    },
    components: {
        DesktopBar
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
@import url(@/css/pages/desktop/home/desktop-home.css);
</style>