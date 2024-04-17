<template>
    <TooltipFrame ref="tooltip" :delay-hiding="500" :delay-showing="500">
        <template #tooltipMask>
            <div class="p-star-button-frame">
                <div v-if="myStar != null" @:click="resolveUnStar" class="p-stared-button"> 
                    <MIcon fa="star" :style="{fontSize: '12px'}" />
                    {{ getStaredLabel() }} 
                </div>
                <div v-else @:click="resolveCommitFiveStar" class="p-not-stared-button"> {{ getStaredLabel() }} </div>
            </div>
        </template>
        <template #tooltipContent>
            <div class="p-star-button-tooltip">
                <div class="p-star-button-tooltip-star"
                    :style="{paddingRight: '12px'}"
                    @:click="resolveClickStar(0)"
                    @:mouseenter="resolveMouseEnter(0)"
                    @:mouseleave="resolveMouseLeave(0)"
                    >
                    <MIcon v-if="currentStar==0" fa="star" family="fas"
                        :style="selectZeroStarIconStyle"/>
                    <MIcon v-else fa="star" family="far" 
                        :style="selectZeroStarIconStyle"/> 
                </div>
                <template v-for="(index) in [1, 2, 3, 4, 5]" :key="`${currentStar}-${index}`">
                    <div class="p-star-button-tooltip-star"
                        @:click="resolveClickStar(index)"
                        @:mouseenter="resolveMouseEnter(index)"
                        @:mouseleave="resolveMouseLeave(index)"
                        >
                        <MIcon v-if="currentStar >= index" fa="star" family="fas"
                            :style="selectStarIconStyle"/>
                        <MIcon v-else fa="star" family="far" 
                            :style="selectStarIconStyle"/> 
                    </div>
                </template>
            </div>
        </template>
    </TooltipFrame>
</template>

<script>
import TooltipFrame from '@/components/base/tooltip/TooltipFrame.vue';

export default {
    name: 'VisualizedStar',
    data() {
        return {
            myStar: this.commentProvider?.MyStar,
            currentUser: null,
            selectStarIconStyle:{
                fontSize: '28px'
            },
            selectZeroStarIconStyle: {
                color: 'var(--red-color)',
                fontSize: '28px'
            },
            buttonStyle: {color: 'var(--grey-color-800)'},
            currentStar: 0,

            tooltip: null,
        }
    },
    mounted() {
        this.tooltip = this.$refs?.tooltip;
    },
    components: {
        TooltipFrame
    },
    props: {
        star: {
            required: true,
        },
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.inject?.language != null){
                this.label = this.inject?.language?.subpages?.feedpage?.postcard;
            }
            return this.label;
        },

        /**
         * Hàm lấy ve label cho button Star
         * @param none
         * @returns none
         * @Created PhucTV (16/04/24)
         * @Modified None
         */
        getStaredLabel(){
            try {
                let myStar = this.myStar;
                if (myStar != null){
                    myStar = Math.round(myStar * 10) / 10;
                }
                return this.getLabel().valueStar(myStar);
            } catch (e){
                console.error(e);
            }
        },


        resolveMouseEnter(index){
            this.currentStar = index;
        },
        resolveMouseLeave(){
            this.currentStar = this.myStar;
        },
        resolveClickStar(index){
            try {
                this.myStar = index;
                this.tooltip.hideTooltip(0);
            } catch (e){
                console.error(e);
            }
        },

        async resolveUnStar(){
            try {
                this.myStar = null;
                this.currentStar = null;
                this.tooltip.hideTooltip(0);
            } catch (e){
                console.error(e);
            }
        },

        async resolveCommitFiveStar(){
            try {
                this.myStar = 5;
                this.currentStar = 5;
                this.tooltip.hideTooltip(0);
            } catch (error) {
                console.error(error);
            }
        }
    },
    
    inject: {
        inject: {},
        commentProvider: {}
    }
}

</script>

<style scoped>

.p-star-button-frame{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 12px;
    width: 64px;
}


.p-star-button-tooltip{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    padding: 12px 12px;
}

.p-star-button-tooltip-star{
    cursor: pointer;
}

.p-not-stared-button{
    color: var(--grey-color);
    font-family: 'ks-font-semibold';
}

.p-stared-button{
    color: var(--primary-color);
    font-family: 'ks-font-semibold';
}

.p-not-stared-button:hover{
    color: var(--grey-color-800);
}

</style>

