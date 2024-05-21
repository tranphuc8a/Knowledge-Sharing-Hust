<template>
    <TooltipFrame ref="tooltip" :delay-hiding="500" :delay-showing="500">
        <template #tooltipMask>
            <div class="p-star-button-frame">
                <MEmbeddedButton v-if="myStar != null" :iconStyle="staredIconStyle" :buttonStyle="staredButtonStyle" 
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
import CurrentUser from '@/js/models/entities/current-user';
import { PutRequest } from '@/js/services/request';

export default {
    name: 'PostCardStarButton',
    data() {
        return {
            myStar: this.getPost()?.MyStars,
            currentUser: null,
            notStaredIconStyle: {
                color: 'var(--grey-color-600)',
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
            buttonStyle: {color: 'var(--grey-color-600)'},
            staredButtonStyle: {color: 'var(--primary-color)'},
            currentStar: 0,

            tooltip: null,
        }
    },
    async mounted() {
        try {
            this.tooltip = this.$refs?.tooltip;
            this.currentUser = await CurrentUser.getInstance();
            this.myStar = this.getPost()?.MyStars;
        } catch (e) {
            console.error(e);
        }
    },
    components: {
        TooltipFrame, MEmbeddedButton
    },
    props: {
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
                this.submitStar(index);
            } catch (e){
                console.error(e);
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
                this.submitStar(5);
            } catch (error) {
                console.error(error);
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
            } catch (e){
                console.error(e);
            }
        },

        async submitStar(star){
            try {
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                }
                let postId = this.getPost().UserItemId;
                this.updateToolbar(star);
                await new PutRequest('Stars').setBody({
                    UserItemId: postId,
                    Score: star
                }).execute();
                
            } catch (e){
                this.myStar = this.getPost()?.MyStars;
                this.currentStar = this.myStar;
                console.error(e);
            }
        },

        async updateToolbar(star){
            try {
                let numStars = this.getPost().TotalStar ?? 0;
                let averageStars = this.getPost().AverageStar ?? 0;
                if (this.getPost().MyStars == null){
                    averageStars = (averageStars * numStars + star) / (numStars + 1);
                    numStars += 1;
                    this.getPost().TotalStar = numStars;
                    this.getPost().AverageStar = averageStars;
                } else if (numStars > 0) {
                    averageStars = (averageStars * numStars + star - this.getPost().MyStars) / numStars;
                    this.getPost().AverageStar = averageStars;
                }  
                this.getPost().MyStars = star;
                this.forceUpdateToolbar();
            } catch (e){
                console.error(e);
            }
        }
    },
    
    inject: {
        forceUpdateToolbar: {},
        getLanguage: {},
        getPost: {},
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

