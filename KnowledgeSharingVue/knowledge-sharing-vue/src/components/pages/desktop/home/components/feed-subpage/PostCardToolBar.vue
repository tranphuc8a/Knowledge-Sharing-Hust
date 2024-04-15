<template>
    <div class="p-postcard-toolbar">
        <div class="p-postcard-toolbar__info">
            <div class="p-pct-leftinfo">
                <div class="p-pct-star">
                    <VisualizedStar :star="post?.Star ?? 3.68" />
                </div>
                <MIcon fa="circle" :style="dotIconStyle" />
                <div class="p-pct-numstar" @:click="showListStar">
                    {{ getNumStar() }}
                </div>
            </div>
            <div class="p-pct-rightinfo">
                <div class="p-pct-numcomment">
                    {{ getComment() }}
                </div>
                <MIcon fa="circle" :style="dotIconStyle"/>
                <div class="p-pct-numview">
                    {{ getView() }}
                </div>
            </div>
        </div>
        <div class="p-postcard-toolbar__devide">
        </div>
        <div class="p-postcard-toolbar__actions">
            <div class="p-pct-star">
                
            </div>
            <div class="p-pct-comment">
                <MEmbeddedButton fa="comment" :label="getLabel()?.comment" 
                    :onclick="resolveClickComment" 
                    :iconStyle="iconStyle"
                    :buttonStyle="buttonStyle"/>
            </div>
            <div class="p-pct-star">
                <MEmbeddedButton fa="share" :label="getLabel()?.viewDetail" 
                    :onclick="resolveClickViewDetail" 
                    :iconStyle="iconStyle"
                    :buttonStyle="buttonStyle"
                />
            </div>
        </div>
    </div>
</template>

<script>
import VisualizedStar from '@/components/base/visualized-components/VisualizedStar.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton'
import Common from '@/js/utils/common';

export default {
    name: "PostCardToolbar",
    data(){
        return {
            label: null,
            dotIconStyle: {fontSize: '3px', color: 'var(--grey-color-600)'},
            iconStyle: {color: 'var(--grey-color)'},
            buttonStyle: {color: 'var(--grey-color-800)'}
        }
    },
    components: {
        MEmbeddedButton, VisualizedStar
    },
    mounted(){
        this.getLabel();
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
            console.log(this.inject);
            return this.label;
        },

        getStar(){
            try {
                let numstar = Number(this.post?.Star ?? 0);
                return Math.round(numstar * 10) / 10;
            } catch (e) {
                console.error(e);
            }
        },

        getNumStar(){
            try {
                let numstar = Number(this.post?.NumberStar ?? 0);
                let beautyNumber = Common.formatNumber(numstar);
                return this.getLabel()?.numberStar(beautyNumber);
            } catch (e) {
                console.error(e);
            }
        },

        getComment(){
            try {
                let comment = Number(this.post?.NumberComment ?? 0);
                let beautyNumber = Common.formatNumber(comment);
                return this.getLabel()?.numberComment(beautyNumber);
            } catch (e) {
                console.error(e);
            }
        },

        getView(){
            try {
                let view = Number(this.post?.Views ?? 0);
                let beautyNumber = Common.formatNumber(view);
                return this.getLabel()?.numberView(beautyNumber);
            } catch (e) {
                console.error(e);
            }
        },

        async showListStar(){

        }
    },
    inject: {
        inject: {},
        post: {
            default: null
        }
    }
}

</script>

<style scoped>
.p-postcard-toolbar{
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: fit-content;
    padding: 0px 16px;
}

.p-postcard-toolbar > * {
    width: 100%;
}

.p-postcard-toolbar__info,
.p-postcard-toolbar__info > *{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    gap: 4px;
}

.p-pct-numstar,
.p-pct-numcomment,
.p-pct-numview{
    cursor: pointer;
}

.p-pct-numstar:hover,
.p-pct-numcomment:hover,
.p-pct-numview:hover{
    text-decoration: underline;
}

.p-postcard-toolbar__devide{
    margin: 8px 0 4px 0;
    width: 100%;
    height: 1px;
    background-color: var(--grey-color-300);
}

.p-postcard-toolbar__actions{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    gap: 4px;
}

.p-postcard-toolbar__actions > * {
    flex: 1;
}
</style>
