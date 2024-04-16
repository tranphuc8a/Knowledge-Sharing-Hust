<template>
    <div ref="iconContainer" class="p-icon-container p-action-icon" :style="containerStyle"
        @:click="resolveOnclick" :state="dstate">
        <MIcon  :fa="fa" 
                :family="family"
                :style="iconStyle" 
                v-if="dstate != 'loading'"/>
        <MSpinner v-else :style="iconStyle" />
    </div>
    
</template>

<script>

let icon = {
    name: "ActionIcon",
    data(){
        return {
            dstate: this.state
        }
    },
    methods: {
        /**
         * Xử lý sự kiện click chuột vào icon
         * @param none
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async resolveOnclick(){
            if (this.dstate != 'normal') return;
            this.dstate = 'loading';
            await this.onclick();
            this.dstate = 'normal';
        }
    },
    props: {
        fa: {},
        family: {
            type: String,
            default: 'fas'
        },
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
            default: async function(){
                await new Promise(t => setTimeout(t, 1000));
            }
        }, 
        state: {
            default: 'normal'
        }
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


