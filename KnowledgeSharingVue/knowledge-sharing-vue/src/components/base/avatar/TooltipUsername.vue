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
import { useRouter } from 'vue-router';
import { Validator } from '@/js/utils/validator';

export default {
    name: "TooltipUsername",
    data() {
        return {
            router: useRouter(),
        }
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
            try {
                let username = this.user?.Username;
                if (Validator.isEmpty(username))
                    return;
                this.router.push('/profile/' + username);
            } catch (e){
                console.error(e);
            }
        }
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
.p-tooltip-username{
    width: fit-content;
    font-weight: 600;
    cursor: pointer;
}
.p-tooltip-username:hover{
    text-decoration: underline;
}
</style>

