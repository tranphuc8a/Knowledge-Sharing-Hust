<template>
    <div class="p-feedcard">
        <slot></slot>
    </div>
</template>

<script>
export default {
    name: "FeedCardFrame",
    data() {
        return {
            
        }
    },
    mounted() {
        
    },
    components: {
        
    },
    methods: {
        
    }
}
</script>

<style scoped>
.p-feedcard{
    background-color: white;
    border-radius: 8px;
    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1),
                0 2px 4px 0 rgba(0, 0, 0, 0.06);
    width: 100%;
    color: var(--blue-grey-color-900);
}
</style>