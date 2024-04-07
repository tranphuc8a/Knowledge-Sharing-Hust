<template>
    <div>
        <div ref="icon" :class="getClassname()"></div>
    </div>
</template>

<script>

let icon = {
    name: "Spinner",
    methods: {
        /**
         * Lấy className của spinner tùy vào color
         * @param none
         * @returns className của spinner
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        getClassname(){
            return `pi-icon pi-spinner pi-spinner-${this.color} pi-icon-${this.size} ${this.class}`;
        }
    },
    props: {
        color: {
            type: String,
            default: "green"
        },
        size: {
            type: String,
            default: "small"
        },
        class: {}
    },
};
export default icon;
</script>

<style scoped>
    @import url(@/css/base/icon/icon.css);
</style>


