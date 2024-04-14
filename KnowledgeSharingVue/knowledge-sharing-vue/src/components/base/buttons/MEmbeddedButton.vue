<template>
    <div @:click="resolveOnclick" :style="buttonStyle" class="p-button p-embedded-button" :state="data.state">
        <div class="p-button-content">
            <MIcon :fa="fa" :style="iconStyle" v-if="fa != null" />
            <div v-if="label != null"> {{ label }} </div>
        </div>
        <div class="p-loading-container">
            <MSpinner/>
        </div>
    </div>
</template>

<script>
import buttonScript from '@/js/components/base/button';
let button = {
    name: "EmbeddedButton",
    data() {
        return {
            data: {
                state: this.state
            }
        };
    },
    methods: {
        ...buttonScript.methods
    },
    watch: {
        ...buttonScript.watch
    },
    props: {
        ...buttonScript.props
    },
};
export default button;
</script>

<style scoped>
    @import url(@/css/base/button/button.css);
</style>


