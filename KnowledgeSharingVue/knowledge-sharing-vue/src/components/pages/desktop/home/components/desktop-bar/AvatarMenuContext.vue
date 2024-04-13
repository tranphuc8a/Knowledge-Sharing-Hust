<template>
    <div class="p-avatar-menu-context" tabindex="1">
        <Avatar :src="src" :title="name" size="36" />
    </div>
</template>

<script>
import Avatar from '@/components/base/avatar/Avatar.vue';
export default {
    name: "AvatarMenuContext",
    components: {
        Avatar
    },
    data(){
        return {
            src: null,
            name: null
        }
    },
    mounted() {
        // Get avatar url from user data in local storage
        let userContext = localStorage.getItem("user");
        userContext = JSON.parse(userContext);
        this.src = userContext?.avatar;
        this.name = userContext?.fullName;
    },
    methods: {
        
    }
}
</script>

<style scoped>

</style>