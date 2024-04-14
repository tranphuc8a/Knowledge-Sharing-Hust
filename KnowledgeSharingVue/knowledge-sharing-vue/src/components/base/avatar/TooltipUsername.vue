<template>
    <TooltipFrame>
        <template #tooltipMask>
            <div class="p-tooltip-username" @:click="resolveClickUsername">
                {{ user?.FullName ?? "User"}}
            </div>
        </template>

        <template #tooltipContent>
            <TooltipUser :user="user" />
        </template>
    </TooltipFrame>
</template>


<script>
import TooltipUser from './TooltipUser.vue';
import TooltipFrame from '../tooltip/TooltipFrame.vue';
export default {
    name: "TooltipUsername",
    data() {
        return {}
    },
    components: {
        TooltipUser, TooltipFrame
    },
    methods: {
        /**
         * Hàm xử lý khi click vào username
         * @param none
         * @returns none
         * @Created PhucTV (15/04/24)
         * @Modified None
        */
        async resolveClickUsername(){
            try{
                if (this.globalData?.router != null){
                    await this.globalData.router.push(`/profile/${this.user?.Username}`);
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    props: {
        user: {
            default: {
                avatar: null,
                fullName: null,
                username: null,
            },
        }
    },
};
</script>

<style scoped>
.p-tooltip-username{
    width: fit-content;
    font-weight: 600;
    cursor: pointer;
}
.p-tooltip-username:hover{
    text-decoration: underline;
}
</style>

