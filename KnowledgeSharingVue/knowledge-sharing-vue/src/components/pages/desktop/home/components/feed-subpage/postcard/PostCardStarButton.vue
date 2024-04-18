<template>
    <TooltipFrame ref="tooltip" :delay-hiding="500" :delay-showing="500">
        <template #tooltipMask>
            <div class="p-star-button-frame">
                <MEmbeddedButton v-if="myStar != null" :iconStyle="staredIconStyle" :buttonStyle="buttonStyle" 
                    fa="star" iconFamily="fas" :label="getStaredLabel()"
                    :onclick="resolveUnStar"/>
                <MEmbeddedButton v-else :iconStyle="notStaredIconStyle" :buttonStyle="buttonStyle"
                    fa="star" iconFamily="far" :label="getStaredLabel()"
                    :onclick="resolveCommitFiveStar"/>
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
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton';

export default {
    name: 'VisualizedStar',
    data() {
        return {
            myStar: this.getPost()?.MyStar,
            currentUser: null,
            notStaredIconStyle: {
                color: 'var(--grey-color)',
                fontSize: '18px'
            },
            staredIconStyle: {
                color: 'var(--primary-color)',
                fontSize: '18px'
            },
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
        TooltipFrame, MEmbeddedButton
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
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.subpages?.feedpage?.postcard;
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
        getLanguage: {},
        getPost: {}
    }
}

</script>

<style scoped>

.p-star-button-frame{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
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

</style>

