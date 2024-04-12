<template>
    <div class="phone-bar__bottom">
        bottom phone bar
    </div>
</template>

<script>

export default {
    name: "BottomPhoneBar",
    data() {
        return {
            title: "Home Page"
        }
    },
    components: {
        
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
@import url(@/css/pages/home/components/phone-bar.css);
</style>