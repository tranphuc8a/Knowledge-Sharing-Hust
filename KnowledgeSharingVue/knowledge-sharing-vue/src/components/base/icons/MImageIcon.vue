<template>
    <div>
        <div ref="iconContainer" class="p-icon-container" @:click="resolveOnclick">
            <img ref="iconImage" :src="src" alt="Icon from image" class="p-image-icon">
        </div>
    </div>
</template>

<script>
let icon = {
    name: "ImageIcon",
    data() {
        return {};
    },
    mounted() {
        // css for icon container
        this.iconContainer = this.$refs.iconContainer;
        for (let key in this.style){
            this.iconContainer.style.setProperty(`--icon-container-${key}`, String(this.style[key]));
        }
        for (let key in this.containerStyle){
            this.iconContainer.style.setProperty(`--icon-container-${key}`, String(this.containerStyle[key]));
        }
        // CSS for icon image
        this.iconImage = this.$refs.iconImage;
        for (let key in this.iconStyle){
            this.iconImage.style.setProperty(`--image-icon-${key}`, String(this.iconStyle[key]));
        }
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
        style: {
            type: Object,
            default: {}
        },
        src: {
            type: String,
            default: ""
        },
        onclick: {
            type: Function,
            default: () => {}
        },
        style: {
            type: Object,
            default: {}
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


