

<template>
    <div class="p-visualized-total-star">
        <div class="p-vts-star">
            <VisualizedStar :star="averageStar ?? 0" />
        </div>
        <MIcon fa="circle" :style="dotIconStyle" />
        <div class="p-vts-numstar" @:click="showListStar">
            {{ totalStarText }}
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import VisualizedStar from '../visualized-components/VisualizedStar.vue';


export default {
    name: 'VisualizedTotalStar',
    components: {
        VisualizedStar,
    },
    props: {
        averageStar: {
            required: true,
        },
        totalStar: {
            required: true,
        },
    },
    watch: {
        averageStar(){
            this.refresh();
        },
        totalStar(){
            this.refresh();
        }
    },
    data(){
        return {
            dotIconStyle: {fontSize: '3px', color: 'var(--primary-color)'},
            totalStarText: null,
        }
    },
    async mounted(){
        this.refresh();
    },
    methods: {
        refresh(){
            try {
                let totalStarFormatedNumber = Common.formatNumber(this.totalStar);
                this.totalStarText = `${totalStarFormatedNumber} đánh giá`;
            } catch (e){
                console.error(e);
            }
        },
        showListStar(){
        }
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

.p-visualized-total-star{
    max-width: 100%;
    width: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
}

</style>

