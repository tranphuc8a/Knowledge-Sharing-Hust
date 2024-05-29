

<template>
    <div class="d-navigation-item" @:click="resolveClickItem">
        <div class="d-item-icon" v-show="user != null">
            <UserAvatar :user="user" :size="32" />
        </div>
        <div class="d-item-label">
            {{ user?.FullName ?? "Người dùng hệ thống" }}
        </div>
    </div>
</template>



<script>

import { useRouter } from 'vue-router';
import UserAvatar from '@/components/base/avatar/UserAvatar.vue';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'HomeNavigationItem',
    props: {
    },
    components: {
        UserAvatar
    },
    data(){
        return {
            title: '',
            router: useRouter(),
            user: null
        }
    },
    async mounted(){
        try {
            this.user = await CurrentUser.getInstance();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async resolveClickItem(){
            try {
                this.router.push('profile/' + this.user?.Username);
            } catch (e) {
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>

.d-navigation-item{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 12px;
    cursor: pointer;
    padding: 8px;
    border-radius: 4px;
}

.d-navigation-item:hover{
    background-color: var(--primary-color-100);
}

.d-navigation-item:active{
    background-color: var(--primary-color-200);
}

.d-item-label{
    font-family: 'ks-font-semibold';
    font-size: 15px;
    color: var(--primary-color);

    width: 100%;
    align-self: stretch;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
}

</style>

