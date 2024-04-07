<template>
    <div class="p-spinner-container">
        <FontAwesomeIcon ref="icon" :icon="['fas', fa]" :style="style" class="p-icon" spin/>
    </div>
</template>

<script>

let icon = {
    name: "Icon",
    data() {
        return {};
    },
    props: {
        fa: { default: "spinner" },
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


