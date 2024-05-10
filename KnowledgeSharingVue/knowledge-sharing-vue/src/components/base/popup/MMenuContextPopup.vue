<template>
    <m-context-popup ref="menu">
        <template #popupContextMask>
            <slot> </slot>
        </template>

        <template #popupContextContent>
            <div class="p-popup-menu-context-content">
                <template v-for="(option, index) in listOptions" :key="option.key ?? index">
                    <div class="p-popup-menu-context-option" @:click="resolveClickItem(option.onclick)">
                        <MIcon :style="iconStyle" :fa="option.fa" :family="option.family" />
                        <span> {{ option.label }} </span>
                    </div>
                </template>
            </div>
        </template>

    </m-context-popup>
</template>

<script>

import MContextPopup from '@/components/base/popup/MContextPopup.vue';


export default{
    name: 'MenuContextPopup',
    components: {
        MContextPopup
    },
    data(){
        return {
            iconStyle: {
                fontSize: '18px',
                color: 'var(--primary-color)'
            },
            listOptions: this.options,
            isWaiting: false,
        }
    },
    props: {
        options: {
            type: Array,
            default: () => [{
                fa: 'book',
                family: 'fas',
                label: 'Book',
                onclick: async function(){}
            }]
        },
    },
    methods: {
        async resolveClickItem(callback){
            if (this.isWaiting) return;
            try {
                this.isWaiting = true;
                if (callback != null){
                    await callback.call();
                }
                this.$refs['menu']?.hidePopup?.(0);
            } catch (error){
                console.error(error);
            } finally {
                this.isWaiting = false;
            }
        }
    }
}

</script>

<style scoped>

.p-popup-menu-context-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-popup-menu-context-option{
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

.p-popup-menu-context-option :first-child{
    width: 20px;
}
.p-popup-menu-context-option :last-child{
    width: fit-content;
}

.p-popup-menu-context-option:hover{
    background-color: var(--primary-color-100);
}

.p-popup-menu-context-option:active{
    background-color: var(--primary-color-200);
}


</style>