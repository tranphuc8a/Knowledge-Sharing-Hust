<template>
    <div class="p-icon-container">
        <FontAwesomeIcon ref="icon" :icon="['fas', fa]" :style="style" class="p-icon"/>
    </div>
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
        }
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


