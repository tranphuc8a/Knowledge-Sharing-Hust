<template>
   <Avatar :src="user?.avatar" :size="size" :title="user?.fullName" @:click="resolveClickUserAvatar" />
</template>

<script>
import Avatar from '@/components/base/avatar/Avatar.vue';
export default {
    name: 'UserAvatar',
    data() {
        return {}
    },
    components: {
        Avatar
    },
    methods: {
        /**
         * Resolve click event on user avatar
         * @param none
         * @returns none
         * @Created PhucTV (13/04/24)
         * @Modified None
         */
        async resolveClickUserAvatar() {
            try {
                let router = this.globalData?.router;
                // navigate to user profile page
                router.push({name: 'profile', params: {username: this.user?.username}});
            } catch (error) {
                console.log(error);
            }
        }
    },
    props: {
        user: {
            required: true,
            default: null
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