<template>
    <div class="p-visual-privacy-mask-container">
        <TooltipFrame>
            <template #tooltipMask>
                <div class="p-visual-privacy-mask">
                    <MIcon v-if="isPrivate()" fa="lock" :style="iconStyle"/>
                    <MIcon v-else fa="globe-americas" :style="iconStyle"/>
                </div>
            </template>
            <template #tooltipContent>
                <div class="p-visual-privacy-content">
                    {{ isPrivate() ? 'Riêng tư' : 'Công khai'}}
                </div>
            </template>
        </TooltipFrame>
    </div>
</template>

<script>
import TooltipFrame from '../tooltip/TooltipFrame.vue';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'VisualizedDatetime',
    data() {
        return {
            iconStyle: {
                color: 'var(--primary-color)',
                fontSize: '12px'
            }
        }
    },
    mounted() {
    },
    components: {
        TooltipFrame,
    },
    props: {
        privacy: {
            required: true,
        },
    },
    methods: {

        /**
         * Hàm kiểm tra xem có phải là private không
         * @param none
         * @returns boolean
         * @Created PhucTV (15/04/24)
         * @Modified None
        */
        isPrivate(){
            return this.privacy == myEnum.EPrivacy.Private;
        }
    }
}

</script>

<style scoped>
.p-visual-privacy-mask-container{
    width: fit-content;
}
.p-visual-privacy-mask{
    width: fit-content;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    color: var(--grey-color-700);
}

.p-visual-privacy-content{
    width: fit-content;
    font-weight: 600;
    color: var(--grey-color-900);
    font-size: 13px;
    padding: 5px 10px;
}
</style>

