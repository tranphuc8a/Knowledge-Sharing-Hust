<template>
    <div ref="iconContainer" class="p-icon-container" :style="containerStyle"
        @:click="resolveOnclick" :state="state">
        <MIcon  :fa="fa" 
                :style="iconStyle" />
    </div>
    
</template>

<script>

let icon = {
    name: "ActionIcon",
    methods: {
        /**
         * Xử lý sự kiện click chuột vào icon
         * @param none
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async resolveOnclick(){
            await this.onclick();
        }
    },
    props: {
        fa: {},
        containerStyle: {
            type: Object,
            default: {}
        },
        iconStyle: {
            type: Object,
            default: {}
        },
        onclick: {
            type: Function,
            default: async function(){}
        }, 
        state: {}
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


