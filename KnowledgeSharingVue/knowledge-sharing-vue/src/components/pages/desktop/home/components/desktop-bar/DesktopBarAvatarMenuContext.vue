<template>
    <div class="p-desktop-bar-avatar-menu-context">
        <AnonymousAvatarContext v-if="!isLogedIn" />
        <LogedInAvatarContext v-else />
    </div>
</template>

<script>
import AnonymousAvatarContext from './AnonymousAvatarContext.vue';
import LogedInAvatarContext from './LogedInAvatarContext.vue';

export default {
    name: "DesktopBarAvatarMenuContext",
    components: {
        AnonymousAvatarContext,
        LogedInAvatarContext
    },
    data(){
        return {
            isLogedIn: false,
        }
    },
    async mounted() {
        this.isLogedIn = (await this.getCurrentUser()) != null;
    },
    methods: {
    },
    inject: {
        getCurrentUser: {},
    }
}
</script>

<style scoped>

.p-desktop-bar-avatar-menu-context{
    width: fit-content;
    height: fit-content;
}

</style>