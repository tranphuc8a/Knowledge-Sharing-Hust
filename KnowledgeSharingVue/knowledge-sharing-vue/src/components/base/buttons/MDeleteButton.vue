<template>
    <div @:click="resolveOnclick" class="p-button p-delete-button" :state="data.state">
        <div class="p-button-content">
            <MIcon :fa="fa" :style="data.iconStyle" v-if="fa != null" />
            <div v-if="label != null"> {{ label }} </div>
        </div>
        <div class="p-loading-container">
            <MSpinner :style="data.iconStyle"/>
        </div> 
    </div>
</template>

<script>
import buttonScript from '@/js/components/base/button';

let button = {
    name: "DeleteButton",
    data() {
        return {
            data: {
                state: this.state,
                iconStyle: { color: 'white' }
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

<style>
    @import url(@/css/base/button.css);
</style>


