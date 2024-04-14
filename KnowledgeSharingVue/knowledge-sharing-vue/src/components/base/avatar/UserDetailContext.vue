<template>
    <UserAvatar :user="user" :size="size"/>
</template>

<script>
import UserAvatar from '@/components/base/avatar/UserAvatar.vue';
export default {
    name: 'HoverableUserAvatar',
    data() {
    },
    components: {
        UserAvatar
    },
    methods: {
    
    },
    props: {
        user: {
            default: {
                avatar: null,
                fullName: null,
                username: null
            }
        },
        size: {
            type: Number,
            default: 36
        },
    },
}
</script>

<style scoped>

</style>