

<template>
    <div class="p-tooltip-user-avatar-and-username">
        <TooltipUserAvatar :user="dUser" :size="dSize" />
        <TooltipUsername :user="dUser" :style="style" />
    </div>
</template>



<script>

import TooltipUserAvatar from './TooltipUserAvatar.vue';
import TooltipUsername from './TooltipUsername.vue';



export default {
    name: 'TooltipUserAvatarAndUsername',
    components: {
        TooltipUserAvatar,
        TooltipUsername,
    },
    props: {
        user: {
            required: true,
            type: Object,
        },
        size: {
            type: Number,
            default: 36,
        },
        style: {
            type: Object,
            default: null,
        }
    },
    watch: {
        user(){
            this.dUser = this.user;
        },
        size(){
            this.dSize = this.size;
        }
    },
    data(){
        return {
            dUser: this.user,
            dSize: this.size,
        }
    },
    async mounted(){
    },
    methods: {

    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-tooltip-user-avatar-and-username{
    width: fit-content;
    max-width: 100%;
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: flex-start;
    gap: 16px;
}

</style>

