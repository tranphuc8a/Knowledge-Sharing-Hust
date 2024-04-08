<template>
    <TestInputPage />
    <TestButtonsPage />
    <TestContextMenuPage />
    <TestToastPage />
</template>


<script>
import TestInputPage from '@/components/base/inputs/TestInputPage.vue';
import TestButtonsPage from '@/components/base/buttons/TestButtonsPage.vue';
import TestContextMenuPage from '@/components/base/context-menu/TestContextMenuPage.vue';
import TestToastPage from '@/components/base/toast/TestToastPage.vue';

export default {
    name: 'KSTestPage',
    components: { TestButtonsPage, TestContextMenuPage, TestToastPage, TestInputPage },
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
