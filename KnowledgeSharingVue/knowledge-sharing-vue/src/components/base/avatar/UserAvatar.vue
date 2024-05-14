<template>
    <Avatar :src="user?.Avatar" :size="size" :title="user?.fullName" @:click="resolveClickUserAvatar" />
</template>

<script>
import Avatar from '@/components/base/avatar/Avatar.vue';
import { useRouter } from 'vue-router';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'UserAvatar',
    data() {
        return {
            router: useRouter()
        }
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
                let username = this.user?.Username;
                let userId = this.user?.UserId;
                let userIdentifier = username ?? userId;
                if (Validator.isEmpty(userIdentifier))
                    return;
                this.router.push('/profile/' + userIdentifier);
            } catch (e){
                console.error(e);
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