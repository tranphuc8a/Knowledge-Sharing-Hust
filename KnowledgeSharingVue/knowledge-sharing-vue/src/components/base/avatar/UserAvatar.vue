<template>
    <div class="skeleton" :style="{
        width: size + 'px',
        height: size + 'px',
        borderRadius: (size ?? 40) + 'px',
    }" v-if="!isLoaded"></div>

    <Avatar :src="user?.Avatar" :size="size" :title="user?.fullName" @:click="resolveClickUserAvatar" v-if="isLoaded" />
</template>

<script>
import Avatar from '@/components/base/avatar/Avatar.vue';
import { useRouter } from 'vue-router';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'UserAvatar',
    data() {
        return {
            router: useRouter(),
            isLoaded: this.user != null
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
    watch: {
        user: {
            handler: function(){
                this.isLoaded = this.user != null;
            },
            immediate: true
        }
    }
}
</script>

<style scoped>

</style>