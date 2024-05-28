<template>
    <div class="p-tooltip-user-avatar-frame">
        <TooltipFrame>
            <template #tooltipMask>
                <UserAvatar :user="user" :size="size" />
            </template>
    
            <template #tooltipContent>
                <TooltipUserV2 :user="user" />
            </template>
        </TooltipFrame>
    </div>
</template>


<script>
import UserAvatar from './UserAvatar.vue';
import TooltipUserV2 from './TooltipUserV2.vue';
import TooltipFrame from '../tooltip/TooltipFrame.vue';
export default {
    name: "TooltipUserAvatar",
    data() {
        return {}
    },
    components: {
        UserAvatar, TooltipFrame, TooltipUserV2
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
.p-tooltip-user-avatar-frame{
    width: fit-content;
}
</style>

