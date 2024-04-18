<template>
    <div class="phone-bar__top">
        top phone bar
    </div>
</template>

<script>

export default {
    name: "TopPhoneBar",
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
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
@import url(@/css/pages/home/components/phone-bar.css);
</style>