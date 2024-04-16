<template>
    <TooltipFrame>
        <template #tooltipMask>
            <div class="p-visualized-star">
                <div class="p-visualized-stars">
                    <template v-for="(index) in [1, 2, 3, 4, 5]">
                        <MIcon :style="iconStyle" v-if="dStar > index" fa="star" :key="`${index}-full`"/>
                        <MIcon :style="iconStyle" v-else-if="dStar + 1 > index" fa="star-half-stroke" :key="`${index}-half`" family="far" />
                        <MIcon :style="iconStyle" v-else fa="star" :key="`${index}-empty`" family="far" />
                    </template>
                </div>
            </div>
        </template>
        <template #tooltipContent>
            <div class="p-visualized-star-numbers">
                {{ dStar }}
                <MIcon :style="iconStyle" fa="star" />
            </div>
        </template>
    </TooltipFrame>
</template>

<script>
import TooltipFrame from '../tooltip/TooltipFrame.vue';
// import { MyRandom } from '@/js/utils/myrandom';

export default {
    name: 'VisualizedStar',
    data() {
        return {
            dStar: null,
            iconStyle: {
                color: 'var(--primary-color)',
                fontSize: '13px',
            }
        }
    },
    mounted() {
        this.updateStar();
    },
    components: {
        TooltipFrame,
    },
    props: {
        star: {
            required: true,
        },
    },
    methods: {
        /**
         * Hàm lam tron so sao
         * @param none
         * @returns string
         * @Created PhucTV (15/04/24)
         * @Modified None
        */
        async updateStar(){
            try {
                let round = Math.round(this.star * 10) / 10;
                this.dStar = round;
            } catch (e){
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>

.p-visualized-star{
    display: flex;
    flex-flow: row nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    width: fit-content;
    gap: 4px; 
    height: 100%;
}

.p-visualized-stars{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: flex-start;
    width: fit-content;
    gap: 2px; 
}

.p-visualized-star-numbers{
    padding: 6px 24px;
    font-size: 16px;
    font-weight: 600;
    color: var(--primary-color-600);
    background-color: white;
}

</style>

