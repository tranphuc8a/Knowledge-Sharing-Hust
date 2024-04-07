<template>
    <div>
        <i ref="icon" :class="getClassname()"></i>
    </div>
</template>

<script>
import { Validator } from '@/js/utils/validator';
let icon = {
    name: "Icon",
    data() {
        return {};
    },
    mounted() {
        // setup CSS for icon from style prop
        this.iconElement = this.$refs.icon;
        for (let key in this.style){
            this.iconElement.style.setProperty(`--icon-${key}`, String(this.style[key]));
        }
        if (Validator.isNotEmpty(this.color)){
            this.iconElement.style.setProperty(`--icon-color`, String(this.color));
        }
    },
    methods: {
        /**
         * Trả về className của icon dựa vào faClassname
         * @param none
         * @returns className của icon
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        getClassname(){
            let className = `fa ${this.faClassname} ${this.faSpin} p-icon`;
            return className;
        }
    },
    props: {
        faClassname: {
            type: String,
            default: "pi-icon pi-plus"
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
        }
    },
};
export default icon;
</script>

<style>
    @import url(@/css/base/icon/icon.css);
</style>


