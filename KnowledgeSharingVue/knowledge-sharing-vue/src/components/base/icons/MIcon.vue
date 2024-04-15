<template>
    <FontAwesomeIcon ref="icon" :icon="[family, fa]" :style="style" class="p-icon"/>
</template>

<script>

let icon = {
    name: "Icon",
    data() {
        return {};
    },
    props: {
        fa: {},
        style: {
            type: Object,
            default: {}
        },
        family: {
            type: String,
            default: 'fas'
        }
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


