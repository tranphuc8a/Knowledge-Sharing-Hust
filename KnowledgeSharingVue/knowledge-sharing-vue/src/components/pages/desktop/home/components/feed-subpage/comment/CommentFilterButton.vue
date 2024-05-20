<template>
    <div class="p-crb-frame">
        <m-context-popup :delay-hiding="0" :delay-showing="0">
            <template #popupContextMask>
                <div class="p-cfb-mask">
                    <span> {{ options[selected]?.label }} </span>
                    <MIcon :style="iconStyle" fa="chevron-down"> </MIcon>
                </div>
            </template>
    
            <template #popupContextContent>
                <div class="p-cfb-content">
                    <template v-for="(option, index) in listOptions" :key="index">
                        <div class="p-cfb-option" @:click="options[option].onClick">
                            <MIcon :style="iconStyle" :fa="options[option].fa"> </MIcon>
                            <span> {{ options[option].label }} </span>
                        </div>
                    </template>
                </div>
            </template>
    
        </m-context-popup>
    </div>
</template>

<script>

import MContextPopup from '@/components/base/popup/MContextPopup.vue';
import { myEnum } from '@/js/resources/enum';


export default{
    name: 'p-comment-filter-button',
    components: {
        MContextPopup
    },
    data(){
        return {
            selected: 0,
            iconStyle: {
                fontSize: '18px',
                color: 'var(--primary-color)'
            },
            listOptions: [
                myEnum.commentFilterType.Best,
                myEnum.commentFilterType.Recent,
                myEnum.commentFilterType.All
            ],
            options: {
                [myEnum.commentFilterType.Best]: {
                    label: 'Phù hợp nhất',
                    fa: 'check',
                    onClick: this.resolveClickOption(myEnum.commentFilterType.Best),
                }, 
                [myEnum.commentFilterType.Recent]: {
                    label: 'Mới nhất',
                    fa: 'clock',
                    onClick: this.resolveClickOption(myEnum.commentFilterType.Recent),
                },
                [myEnum.commentFilterType.All]: {
                    label: 'Tất cả bình luận',
                    fa: 'list-ol',
                    onClick: this.resolveClickOption(myEnum.commentFilterType.All),
                }
            }
        }
    },
    props: {
        onChange: {
            type: Function,
            default: async () => {}
        }
    },
    methods: {
        
        /**
         * Resolve on click to the option
         * 
         * @param {Number} option 
         * @returns {Function} - callback function
         * @Created PhucTV (16/04/24)
         * @Modified None
         */
        resolveClickOption(option){
            let that = this;
            return async function(){
                try {
                    that.selected = option;
                    await that.onChange(option);
                } catch (error){
                    console.error(error);
                }
            }
        }
    }
}

</script>

<style scoped>
.p-crb-frame{
    width: fit-content;
}

.p-cfb-mask{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    font-family: 'ks-font-semibold';
    gap: 4px;
}

.p-cfb-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-cfb-option{
    width: 100%;
    padding: 8px 28px 8px 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-cfb-option :first-child{
    width: 20px;
}

.p-cfb-option:hover{
    background-color: var(--primary-color-100);
}

.p-cfb-option:active{
    background-color: var(--primary-color-200);
}


</style>