<template>
    <div class="p-content">
        <slot>
            This is page content
        </slot>
    </div>
</template>

<script>


export default{
    name: "ContentKSCukcuk",
    components: {  },
    inject: {
        parent: {},
        getToastManager: {}
    },
    data(){
        return {
        }
    },
    mounted(){
    },
    methods: {
        
    }
};

</script>

<style>
</style>
