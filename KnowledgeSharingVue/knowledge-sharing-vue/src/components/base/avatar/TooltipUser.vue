<template>
    <div class="p-tooltip-user-info">
        <div class="p-tooltip-user-info-avatar">
            <UserAvatar :user="user" :size="48" />
        </div>
        <div class="p-tooltip-user-info-details">
            <h3 class="p-tooltip-user-info-name">
                {{ user?.FullName || 'Tên người dùng' }}
            </h3>
            <p class="p-tooltip-user-info-username">
                @{{ user?.Username || 'username' }}
            </p>
            <p class="p-tooltip-user-info-role" v-if="user?.Role">
                Vai trò: {{ user.Role }}
            </p>
        </div>
    </div>
</template>


<script>
import UserAvatar from './UserAvatar.vue';
export default {
    name: "TooltipUser",
    data() {
        return {}
    },
    components: {
        UserAvatar
    },
    methods: {
        
    },
    props: {
        user: {
            required: true,
            default: null
        }
    },
};
</script>

<style scoped>
.p-tooltip-user-info {
    display: flex;
    align-items: center;
    padding: 16px;
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

