<template>
    <TooltipUserAvatar :user="user" :size="36" />
</template>

<script>
import CurrentUser from '@/js/models/entities/current-user';
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
        this.user = CurrentUser.getInstance();
    },
    methods: {
        
    }
}
</script>

<style scoped>

</style>