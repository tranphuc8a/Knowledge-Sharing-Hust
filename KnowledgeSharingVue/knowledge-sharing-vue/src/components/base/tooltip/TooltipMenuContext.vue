<template>
    <TooltipFrame ref="menu">
        <template #tooltipMask>
            <slot></slot>
        </template>

        <template #tooltipContent>
            <div class="p-tooltip-menu-context-content">
                <template v-for="(option, index) in listOptions" :key="option.key ?? index">
                    <div class="p-tooltip-menu-context-option" @:click="resolveClickItem(option.onclick)">
                        <MIcon :style="iconStyle" :fa="option.fa" :family="option.family" />
                        <span> {{ option.label }} </span>
                    </div>
                </template>
            </div>
        </template>
    </TooltipFrame>
</template>


<script>

import TooltipFrame from '../tooltip/TooltipFrame.vue';
export default {
    name: "TooltipMenuContext",
    data() {
        return {
            iconStyle: {
                fontSize: '18px',
                color: 'var(--primary-color)'
            },
            listOptions: this.options,
            isWaiting: false,
        }
    },
    components: {
        TooltipFrame
    },
    methods: {
        async resolveClickItem(callback){
            try {
                if (this.isWaiting) return;
                this.isWaiting = true;
                if (callback != null){
                    await callback.call();
                }
                this.$refs['menu']?.hideTooltip?.(0);
            } catch (error){
                console.error(error);
            } finally {
                this.isWaiting = false;
            }
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
};
</script>

<style scoped>

.p-tooltip-menu-context-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-tooltip-menu-context-option{
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

.p-tooltip-menu-context-option :first-child{
    width: 20px;
}

.p-tooltip-menu-context-option:hover{
    background-color: var(--primary-color-100);
}

.p-tooltip-menu-context-option:active{
    background-color: var(--primary-color-200);
}


</style>

