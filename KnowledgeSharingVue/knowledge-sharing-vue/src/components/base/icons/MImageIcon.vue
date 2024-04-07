<template>
    <!-- Coi Image Icon như một ActionIcon -->
    <div>
        <div ref="iconContainer" class="p-icon-container" :style="containerStyle"
            @:click="resolveOnclick" :state="state">
            <img ref="imageIcon" :src="src" alt="Icon from image" :style="iconStyle" class="p-image-icon">
        </div>
    </div>
</template>

<script>
let icon = {
    name: "ImageIcon",
    data() {
        return {};
    },
    methods: {
        /**
         * Xử lý sự kiện click chuột vào icon
         * @param none
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        resolveOnclick(){
            this.onclick();
        }
    },
    props: {
        state: {},
        src: {
            type: String,
            default: ""
        },
        onclick: {
            type: Function,
            default: () => {}
        },
        containerStyle: {
            type: Object,
            default: {}
        },
        iconStyle: {
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


