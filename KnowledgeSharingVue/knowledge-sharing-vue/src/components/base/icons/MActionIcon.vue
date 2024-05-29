<template>
    <div ref="iconContainer" class="p-icon-container p-action-icon" :style="containerStyle"
        @:click="resolveOnclick" :state="dState">
        <MIcon  :fa="fa" 
                :family="family"
                :style="iconStyle" 
                v-if="dState != 'loading'"/>
        <MSpinner v-else :style="iconStyle" />
    </div>
    
</template>

<script>

let icon = {
    name: "ActionIcon",
    data(){
        return {
            dState: this.state
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
            if (this.dState != 'normal') return;
            try {
                this.dState = 'loading';
                await this.onclick?.();
            } catch (e) {
                console.error(e);
            }
            finally {
                this.dState = 'normal';
            }
        }
    },
    props: {
        fa: {
            required: true,
        },
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
                // await new Promise(t => setTimeout(t, 1000));
            }
        }, 
        state: {
            default: 'normal'
        }
    },
    watch: {
        state: {
            handler: function(){
                this.dState = this.state;
            },
            immediate: true
        }
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


