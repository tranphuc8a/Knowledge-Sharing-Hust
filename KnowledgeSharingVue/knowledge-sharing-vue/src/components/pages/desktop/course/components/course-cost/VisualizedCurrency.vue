

<template>
    <div class="p-visualized-currency">
        <div class="p-free-currency" v-if="!(money > 0)">
            Miễn phí
        </div>
        <div class="p-currency" v-if="money > 0">
            {{ visualizedCurrency }}
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';


export default {
    name: 'VisualizedCurrency',
    props: {
        money: {
            type: Number,
            required: true
        }
    },
    data(){
        return {
            visualizedCurrency: "",
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {
        async refresh(){
            this.visualizedCurrency = await Common.visualizeCurrency(this.money);
        }
    },
    watch: {
        money(){
            this.refresh();
        }
    }
}

</script>

<style scoped>
.p-visualized-currency{
    width: 100%;
    text-align: center;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 16px;
    font-family: 'ks-font-semibold';
    border-radius: 8px;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    cursor: pointer;
}

.p-free-currency{
    width: 100%;
    
    padding: 8px 16px;
    color: white;
    background-color: var(--green-color-400);
    
}
.p-free-currency:hover{
    background-color: var(--green-color-500);
}

.p-currency{
    width: 100%;
    
    padding: 8px 16px;
    color: var(--primary-color);
    background-color: var(--primary-color-100);
}
.p-currency:hover{
    background-color: var(--primary-color-200);
}

</style>

