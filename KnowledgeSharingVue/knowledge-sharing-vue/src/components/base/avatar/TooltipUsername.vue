<template>
    <div class="p-tooltip-username-frame" v-if="!isLoaded">
        <div class="p-tooltip-username skeleton" style="width: 150px; height: 24px;" >
        </div>
    </div>

    <div class="p-tooltip-username-frame" v-if="isLoaded">
        <TooltipFrame>
            <template #tooltipMask>
                <div class="p-tooltip-username" @:click="resolveClickUsername"
                    :style="style">
                    <EllipsisText :text="user?.FullName ?? 'User'" :max-line="2" />
                    <!-- {{ user?.FullName ?? "User"}} -->
                </div>
            </template>
    
            <template #tooltipContent>
                <TooltipUserV2 :user="user" />
            </template>
        </TooltipFrame>
    </div>
</template>


<script>
import TooltipUserV2 from './TooltipUserV2.vue';
import TooltipFrame from '../tooltip/TooltipFrame.vue';
import EllipsisText from '../text/EllipsisText.vue';
import { useRouter } from 'vue-router';
import { Validator } from '@/js/utils/validator';

export default {
    name: "TooltipUsername",
    data() {
        return {
            router: useRouter(),
            isLoaded: this.user != null
        }
    },
    components: {
        TooltipUserV2, TooltipFrame,
        EllipsisText
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
                let userId = this.user?.UserId;
                let userIdentifier = username ?? userId;
                if (Validator.isEmpty(userIdentifier))
                    return;
                this.router.push('/profile/' + userIdentifier);
            } catch (e){
                console.error(e);
            }
        }
    },
    props: {
        user: {
            required: true,
            default: null
        }, 
        style: {
            default: {}
        }
    },
    watch: {
        user: {
            handler: function(){
                this.isLoaded = this.user != null;
            },
            immediate: true
        }
    }
};
</script>

<style scoped>
.p-tooltip-username-frame{
    width: fit-content;
}
.p-tooltip-username{
    width: fit-content;
    font-weight: 600;
    cursor: pointer;
}
.p-tooltip-username:hover{
    text-decoration: underline;
}
</style>

