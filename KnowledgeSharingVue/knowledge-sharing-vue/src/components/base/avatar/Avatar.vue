<template>
    <div class="p-avatar" 
        :style="{
            ...iconStyle,
            backgroundImage: `url(${validatedSrc})`,
        }">
        <!-- <img :src="validatedSrc" :style="iconStyle" :alt="title" class="p-avatar__img" /> -->
        <!-- <MIcon v-else fa="user" :style="iconStyle" class="p-avatar__icon" /> -->
    </div>
</template>

<script>
import Common from '@/js/utils/common';
export default {
    name: 'p-avatar',
    data() {
        return {
            defaultAvatar: require('@/assets/default-thumbnail/student-image-icon.png'),
            // defaultUrl: require('@/assets/images/default-avatar.png'),
            iconStyle: {width: this.size + 'px', height: this.size + 'px'},
            validatedSrc: this.src
        }
    },
    async mounted() {
        this.refresh();
    },
    props: {
        src: {
            type: String,
            default: null
        },
        size: {
            type: Number,
            default: 36
        },
        title: {
            type: String,
            default: null
        }
    },
    watch: {
        src(){
            this.refresh();
        }
    },
    methods: {
        async refresh(){
            try {
                this.validatedSrc = this.defaultAvatar;
                this.validatedSrc = (await Common.isValidImage(this.src)) ? this.src : this.defaultAvatar;
            } catch (e){
                this.validatedSrc = this.defaultAvatar;
                console.error(e);
            }
        }
    }
}
</script>

<style scoped>
.p-avatar {
    border-radius: 100%;
    flex-shrink: 0;
    flex-grow: 0;
    box-sizing: border-box;
    overflow: hidden;
    cursor: pointer;
    background-size: cover;
    background-position: center center;
}
</style>