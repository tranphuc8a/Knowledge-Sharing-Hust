<template>
    <FontAwesomeIcon ref="icon" :icon="[family, fa]" :style="style" class="p-icon" spin/>
</template>

<script>

let icon = {
    name: "Icon",
    data() {
        return {};
    },
    props: {
        fa: { default: "spinner" },
        family: {
            type: String,
            default: 'fas'
        },
        style: {
            type: Object,
            default: {}
        }
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


