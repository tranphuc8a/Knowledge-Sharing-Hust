<template>
    <TestButtonsPage />

    <ImageIcon :src="require('@/assets/logo.png')" />
    <ImageIcon :src="require('@/assets/logo.png')" state="disabled"/>

    <ActionIcon fa="close" />
    <ActionIcon fa="close" state="disabled"/>
</template>


<script>
import TestButtonsPage from '@/components/base/buttons/TestButtonsPage.vue';
import ImageIcon from '@/components/base/icons/MImageIcon.vue';
import ActionIcon from '@/components/base/icons/MActionIcon'

export default {
    name: 'KSTestPage',
    components: { TestButtonsPage, ImageIcon, ActionIcon },
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
