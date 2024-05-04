<template>
    <TooltipFrame>
        <template #tooltipMask>
            <UserAvatar :user="user" :size="size" />
        </template>

        <template #tooltipContent>
            <TooltipUser :user="user" />
        </template>
    </TooltipFrame>
</template>


<script>
import UserAvatar from './UserAvatar.vue';
import TooltipUser from './TooltipUser.vue';
import TooltipFrame from '../tooltip/TooltipFrame.vue';
export default {
    name: "TooltipUserAvatar",
    data() {
        return {}
    },
    components: {
        UserAvatar, TooltipFrame, TooltipUser
    },
    methods: {
        
    },
    props: {
        user: {
            required: true,
            default: null
        },
        size: {
            type: Number,
            default: 36,
        },
    },
};
</script>

<style scoped>
</style>

