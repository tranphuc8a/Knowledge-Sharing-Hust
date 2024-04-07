<template>
    <div>
        <div ref="iconContainer" class="p-icon-container" @:click="resolveOnclick" :state="state">
            <Icon   :faClassname="faClassname" 
                    :faSpin="faSpin" 
                    :color="color" 
                    :style="iconStyle" />
        </div>
    </div>
    
</template>

<script>
import Icon from './MIcon.vue';
let icon = {
    name: "ActionIcon",
    data() {
        return {};
    },
    mounted() {
        // setup CSS for icon from style prop
        this.iconElement = this.$refs.iconContainer;
        for (let key in this.style){
            this.iconElement.style.setProperty(`--icon-container-${key}`, String(this.style[key]));
        }
        for (let key in this.containerStyle){
            this.iconElement.style.setProperty(`--icon-container-${key}`, String(this.containerStyle[key]));
        }
    },
    components: {Icon},
    methods: {
        /**
         * Xử lý sự kiện click chuột vào icon
         * @param none
         * @returns none
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        async resolveOnclick(){
            await this.onclick();
        }
    },
    props: {
        faClassname: {
            type: String
        },
        faSpin: {
            type: String,
            default: ""         // no spin
        },
        color: {
            type: String,
            default: null
        },
        style: {
            type: Object,
            default: {}
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
            default: async function(){}
        }, state: {}
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


