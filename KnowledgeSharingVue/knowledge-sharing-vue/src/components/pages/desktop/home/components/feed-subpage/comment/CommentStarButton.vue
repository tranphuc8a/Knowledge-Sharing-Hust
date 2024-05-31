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
                    title="0 sao"
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
                        :title="index + ' sao'"
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
import { DeleteRequest, PutRequest } from '@/js/services/request';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'CommentStarButton',
    data() {
        return {
            myStar: this.getComment()?.MyStars,
            currentUser: null,
            selectStarIconStyle:{
                fontSize: '28px'
            },
            selectZeroStarIconStyle: {
                color: 'var(--grey-color)',
                fontSize: '28px'
            },
            buttonStyle: {color: 'var(--grey-color-800)'},
            currentStar: 0,

            tooltip: null,
        }
    },
    async mounted() {
        this.tooltip = this.$refs?.tooltip;
        this.myStar = this.getComment().MyStars;
        this.currentStar = this.myStar;
        this.currentUser = await CurrentUser.getInstance();
    },
    components: {
        TooltipFrame
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
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.myStar = index;
                this.tooltip.hideTooltip(0);
                this.sendStar(index);
            } catch (e){
                console.error(e);
            }
        },

        async resolveUnStar(){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.myStar = null;
                this.currentStar = null;
                this.tooltip.hideTooltip(0);
                await this.submitUnstar();
            } catch (e){
                console.error(e);
            }
        },

        async submitUnstar(){
            try {
                let comment = this.getComment();
                if (comment?.MyStars == null) return;
                
                let numStars = this.getComment().TotalStar ?? 0;
                if (numStars <= 0) return;
                let averageStars = this.getComment().AverageStar ?? 0;
                
                // update star
                let myStar = Number(this.getComment().MyStars);
                averageStars = (averageStars * numStars - myStar) / (numStars - 1);
                if (isNaN(averageStars)) averageStars = null;
                numStars -= 1;
                this.getComment().TotalStar = numStars;
                this.getComment().AverageStar = averageStars;                
                this.getComment().MyStars = null;
                this.forceUpdateInformationBar?.();

                await new DeleteRequest('Stars/' + comment.UserItemId).execute();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },

        async resolveCommitFiveStar(){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.myStar = 5;
                this.currentStar = 5;
                this.tooltip.hideTooltip(0);
                this.sendStar(5);
            } catch (error) {
                console.error(error);
            }
        },

        async sendStar(star){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                let comment = this.getComment();
                if (comment != null){
                    let numStars = this.getComment().TotalStar ?? 0;
                    let averageStars = this.getComment().AverageStar ?? 0;
                    if (this.getComment().MyStars === null || 
                        this.getComment().MyStars === undefined){ // add new star
                        averageStars = (averageStars * numStars + star) / (numStars + 1);
                        numStars += 1;
                        this.getComment().TotalStar = numStars;
                        this.getComment().AverageStar = averageStars;
                    } else { // update star
                        let myStar = Number(this.getComment().MyStars);
                        averageStars = (averageStars * numStars + star - myStar) / numStars;
                        this.getComment().AverageStar = averageStars;
                    }  
                    this.forceUpdateInformationBar?.();
                    this.getComment().MyStars = star;
                }
                await new PutRequest('Stars')
                    .setBody({
                        UserItemId: comment.UserItemId,
                        Score: star
                    }).execute();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        }
    },
    
    inject: {
        getLanguage: {},
        getComment: {},
        forceUpdateInformationBar: {},
        getPopupManager: {}
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

