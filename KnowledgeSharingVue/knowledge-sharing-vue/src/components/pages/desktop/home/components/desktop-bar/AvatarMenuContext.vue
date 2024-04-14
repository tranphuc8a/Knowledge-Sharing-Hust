<template>
    <TooltipUserAvatar :user="user" size="36" />
</template>

<script>
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
export default {
    name: "AvatarMenuContext",
    components: {
        TooltipUserAvatar
    },
    data(){
        return {
            user: null
        }
    },
    mounted() {
        // Get user data in local storage
        this.user = JSON.parse(localStorage.getItem('user'));
    },
    methods: {
        
    }
}
</script>

<style scoped>

</style>