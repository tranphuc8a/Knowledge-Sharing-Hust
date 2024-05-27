<template>
    <div class="p-postcard-toolbar" v-if="isShowComponent">
        <div class="p-postcard-toolbar__info">
            <div class="p-pct-leftinfo">
                <div class="p-pct-star">
                    <VisualizedStar :star="getPost()?.AverageStar ?? 0" />
                </div>
                <MIcon fa="circle" :style="dotIconStyle" />
                <div class="p-pct-numstar" @:click="showListStar">
                    {{ getNumStar() }}
                </div>
            </div>
            <div class="p-pct-rightinfo">
                <div class="p-pct-numcomment">
                    <MIcon fa="comment" family="far" :style="tinyIconStyle" />
                    {{ getComment() }}
                </div>
                <MIcon fa="circle" :style="dotIconStyle"/>
                <div class="p-pct-numview">
                    <MIcon fa="eye" family="far" :style="tinyIconStyle" />
                    {{ getView() }}
                </div>
            </div>
        </div>
        <div class="p-postcard-toolbar__devide">
        </div>
        <div class="p-postcard-toolbar__actions">
            <div class="p-pct-star">
                <PostCardStarButton />
            </div>
            <div class="p-pct-comment">
                <MEmbeddedButton fa="comment" iconFamily="far"
                    :label="getLabel()?.comment" 
                    :onclick="resolveOnClickComment" 
                    :iconStyle="iconStyle"
                    :buttonStyle="buttonStyle"/>
            </div>
            <div class="p-pct-star">
                <MEmbeddedButton fa="share"
                    :label="getLabel()?.viewDetail" 
                    :onclick="resolveClickViewDetail" 
                    :iconStyle="iconStyle"
                    :buttonStyle="buttonStyle"
                />
            </div>
        </div>
    </div>
</template>

<script>
import PostCardStarButton from './PostCardStarButton.vue';
import VisualizedStar from '@/components/base/visualized-components/VisualizedStar.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton'
import Common from '@/js/utils/common';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';

export default {
    name: "PostCardToolbar",
    data(){
        return {
            isShowComponent: true,
            label: null,
            dotIconStyle: {fontSize: '3px', color: 'var(--grey-color-600)'},
            iconStyle: {color: 'var(--grey-color-600)'},
            buttonStyle: {color: 'var(--grey-color-600)'},
            tinyIconStyle: {},
            router: useRouter(),
        }
    },
    components: {
        MEmbeddedButton, VisualizedStar, PostCardStarButton
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
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.subpages?.feedpage?.postcard;
            }
            return this.label;
        },

        getStar(){
            try {
                let numstar = Number(this.getPost()?.Star ?? 0);
                return Math.round(numstar * 10) / 10;
            } catch (e) {
                console.error(e);
            }
        },

        getNumStar(){
            try {
                let numstar = Number(this.getPost()?.TotalStar ?? 0);
                let beautyNumber = Common.formatNumber(numstar);
                return this.getLabel()?.numberStar(beautyNumber);
            } catch (e) {
                console.error(e);
            }
        },

        getComment(){
            try {
                let comment = Number(this.getPost()?.NumberComments ?? 0);
                let beautyNumber = Common.formatNumber(comment);
                // return this.getLabel()?.numberComment(beautyNumber);
                return beautyNumber;
            } catch (e) {
                console.error(e);
            }
        },

        getView(){
            try {
                let view = Number(this.getPost()?.Views ?? 0);
                let beautyNumber = Common.formatNumber(view);
                // return this.getLabel()?.numberView(beautyNumber);
                return beautyNumber;
            } catch (e) {
                console.error(e);
            }
        },

        async showListStar(){

        },

        async resolveOnClickComment(){
            try {
                if (this.resolveClickComment != null){
                    await this.resolveClickComment();
                }
            } catch (error){
                console.error(error);
            }
        },

        async resolveClickViewDetail(){
            try {
                let userItemId = this.getPost().UserItemId;
                if (userItemId == null) return;
                let knowledgeType = this.getPost().KnowledgeType;
                if (knowledgeType == myEnum.EKnowledgeType.Course){
                    return this.router.push('/course/' + userItemId.toString());
                }
                let postType = this.getPost().PostType;
                if (postType == myEnum.EPostType.Lesson){
                    return this.router.push('/lesson/' + userItemId.toString());
                }
                return this.router.push('/question/' + userItemId.toString());
            } catch (error){
                console.error(error);
            }
        },
        
        async forceRender(){
            let that = this;
            that.isShowComponent = false;
            that.$nextTick(() => {
                that.isShowComponent = true;
            });
        }
    },
    provide(){
        return {
            forceUpdateToolbar: this.forceRender
        }
    },
    inject: {
        getLanguage: {},
        getPost: {},
        resolveClickComment: { default: null, }
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
    box-sizing: border-box;
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

.p-pct-leftinfo, .p-pct-rightinfo{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    gap: 8px;
}

.p-pct-numstar,
.p-pct-numcomment,
.p-pct-numview,
.p-pct-numstar svg,
.p-pct-numcomment svg,
.p-pct-numview svg{
    cursor: pointer;
    font-size: 14px;
    font-family: 'ks-font-semibold';
    color: var(--grey-color);
}

.p-pct-numstar:hover,
.p-pct-numcomment:hover,
.p-pct-numview:hover,
.p-pct-numstar:hover svg,
.p-pct-numcomment:hover svg,
.p-pct-numview:hover svg{
    color: var(--primary-color);
}

.p-postcard-toolbar__devide{
    margin: 8px 0 4px 0;
    width: 100%;
    height: 1px;
    background-color: var(--primary-color-200);
}

.p-postcard-toolbar__actions{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    box-sizing: border-box;
    gap: 4px;
    width: 100%;
    flex-grow: 0;
}

.p-postcard-toolbar__actions > div {
    width: 0;
    flex: 1 0;
}

.p-pct-star .p-tooltip-container,
.p-pct-star .p-tooltip-container .p-tooltip-mask{
    width: 100%;
}
</style>
