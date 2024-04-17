<template>
    <TooltipFrame>
        <template #tooltipMask>
            <div class="p-visual-datetime-mask">
                {{ getTimeSince() }}
            </div>
        </template>
        <template #tooltipContent>
            <div class="p-visual-datetime-content">
                {{ getFullDateTime() }}
            </div>
        </template>
    </TooltipFrame>
</template>

<script>
import TooltipFrame from '../tooltip/TooltipFrame.vue';
import { MyDate } from '@/js/utils/mydate';

export default {
    name: 'VisualizedDatetime',
    data() {
        return {
            myDateTime: null
        }
    },
    mounted() {
        this.myDateTime = new MyDate(this.datetime);
    },
    components: {
        TooltipFrame,
    },
    props: {
        datetime: {
            required: true,
        },
    },
    methods: {
        /**
         * Hàm lấy thời gian đã trôi qua
         * @param none
         * @returns string
         * @Created PhucTV (15/04/24)
         * @Modified None
        */
        getTimeSince() {
            try {
                return this.myDateTime?.toTimeSince();
            } catch (e) {
                console.error(e);
            }
        },

        /**
         * Hàm lấy day du thoi gian
         * @param none
         * @returns string
         * @Created PhucTV (15/04/24)
         * @Modified None
         */
        getFullDateTime() {
            try {
                return this.myDateTime?.toFullyText();
            } catch (e) {
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>
.p-visual-datetime-mask{
    width: fit-content;
    color: var(--grey-color-700);
    cursor: pointer;
    font-size: 12px;
}
.p-visual-datetime-mask:hover{
    text-decoration: underline;
}
.p-visual-datetime-content{
    width: fit-content;
    font-weight: 600;
    color: var(--grey-color-900);
    font-size: 13px;
    padding: 5px 10px;
}
</style>

