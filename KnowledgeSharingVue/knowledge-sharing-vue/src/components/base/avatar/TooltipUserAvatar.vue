<template>
    <TooltipFrame>
        <template #tooltipMask>
            <UserAvatar :user="user" :size="size" />
        </template>

        <template #tooltipContent>
            <div class="p-tooltip-user-info">
                <div class="p-tooltip-user-info-avatar">
                    <UserAvatar :user="user" :size="48" />
                </div>
                <div class="p-tooltip-user-info-details">
                    <h3 class="p-tooltip-user-info-name">
                        {{ user?.fullName || 'Tên người dùng' }}
                    </h3>
                    <p class="p-tooltip-user-info-username">
                        @{{ user?.username || 'username' }}
                    </p>
                    <p class="p-tooltip-user-info-role" v-if="user?.Role">
                        Vai trò: {{ user.Role }}
                    </p>
                </div>
            </div>
        </template>
    </TooltipFrame>
</template>


<script>
import UserAvatar from './UserAvatar.vue';
import TooltipFrame from '../tooltip/TooltipFrame.vue';
export default {
    name: "TooltipUserAvatar",
    data() {},
    components: {
        UserAvatar, TooltipFrame
    },
    methods: {
        
    },
    props: {
        user: {
            default: {
                avatar: null,
                fullName: null,
                username: null,
            },
        },
        size: {
            type: Number,
            default: 36,
        },
    },
};
</script>

<style scoped>
.p-tooltip-user-info {
    display: flex;
    align-items: center;
    padding: 8px;
}

.p-tooltip-user-info-avatar {
    margin-right: 10px;
    display: flex;
    align-items: center;
}

.p-tooltip-user-info-details {
    flex-grow: 1;
}

.p-tooltip-user-info-name {
    margin: 0;
    font-size: 1.2rem;
    font-weight: bold;
    color: #333;
}

.p-tooltip-user-info-username {
    margin: 4px 0;
    color: #666;
    font-size: 0.9rem;
}

.p-tooltip-user-info-role {
    margin: 0;
    font-size: 0.8rem;
    color: #888;
}
</style>

