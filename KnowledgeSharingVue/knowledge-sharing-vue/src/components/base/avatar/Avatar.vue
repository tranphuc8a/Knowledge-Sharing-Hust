<template>
    <div class="p-avatar">
        <img v-if="src != null" :src="src" :style="iconStyle" :alt="title" class="p-avatar__img" />
        <MIcon v-else fa="user" :style="iconStyle" class="p-avatar__icon" />
    </div>
</template>

<script>

export default {
    name: 'p-avatar',
    data() {
        return {
            // defaultUrl: require('@/assets/images/default-avatar.png'),
            iconStyle: {width: this.size + 'px', height: this.size + 'px'}
        }
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
}
</style>