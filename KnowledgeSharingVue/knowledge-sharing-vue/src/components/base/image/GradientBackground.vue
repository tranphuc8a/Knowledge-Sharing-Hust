<template>
    <div class="gradient-background-image" :style="{ backgroundImage: `url(${dSrc})` }">
        <div class="overlay" :style="{ background: dGradientStyle }"></div>
    </div>
</template>

<script>
import Common from '@/js/utils/common';

export default {
    data() {
        return {
            isValidImage: false,
            dSrc: this.src,
            dGradientDirection: this.gradientDirection,
            dGradientStyle: this.gradientStyle(),
            defaultImage: require('@/assets/default-thumbnail/default-cover.jpg')
        }
    },
    props: {
        src: {
            type: String,
            default: ''
        },
        primaryColor: {
            type: String,
            default: 'white'
        },
        gradientDirection: {
            type: String,
            default: 'to bottom'
        }
    },
    async mounted() {
        await this.validateProps();
    },
    methods: {
        gradientStyle() {
            return `linear-gradient(${this.dGradientDirection}, rgba(255, 255, 255, .16), ${this.primaryColor}, ${this.primaryColor})`;
        },


        async validateProps() {
            const validDirections = ['to top', 'to right', 'to bottom', 'to left'];
            if (!validDirections.includes(this.dGradientDirection)) {
                console.warn(`Invalid gradientDirection prop value: ${this.gradientDirection}. Using default value 'to bottom'.`);
                this.dGradientDirection = 'to bottom';
            }
            this.dGradientStyle = this.gradientStyle();

            let isValidImage = await Common.isValidImage(this.src);
            if (isValidImage) {
                this.isValidImage = true;
            } else {
                this.dSrc = this.defaultImage;
                this.isValidImage = false;
            }
        }
    }
};
</script>

<style scoped>

.gradient-background-image {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center center;
    filter: blur(20px);
    z-index: 0;
}

.overlay {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    mix-blend-mode: screen;
    pointer-events: none;
}
</style>

