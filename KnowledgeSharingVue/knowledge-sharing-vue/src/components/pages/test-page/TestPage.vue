<template>
    <TestInputPage />
    <TestButtonsPage />
    <TestContextMenuPage />
    <TestToastPage />
    <TestPopupPage />
    <AutoAdjustSize />
</template>


<script>
import TestInputPage from '@/components/base/inputs/TestInputPage.vue';
import TestButtonsPage from '@/components/base/buttons/TestButtonsPage.vue';
import TestContextMenuPage from '@/components/base/context-menu/TestContextMenuPage.vue';
import TestToastPage from '@/components/base/toast/TestToastPage.vue';
import TestPopupPage from '@/components/base/popup/TestPopupPage.vue';

import AutoAdjustSize from './AutoAdjustSize.vue';

export default {
    name: 'KSTestPage',
    components: { 
        AutoAdjustSize,
        TestButtonsPage, TestContextMenuPage, TestToastPage, TestInputPage, TestPopupPage },
    data(){
        return {
        }
    },
    mounted(){
    },
    methods: {
        
    },
    props: {

    },
    provide(){
        return {} 
    }
}
</script>


<style>
</style>
