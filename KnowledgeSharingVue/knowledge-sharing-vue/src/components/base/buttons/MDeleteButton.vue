<template>
    <div @:click="resolveOnclick" class="p-button p-delete-button" :style="data.buttonStyle" :state="data.state">
        <div class="p-button-content">
            <MIcon :fa="fa" :family="iconFamily" :style="data.iconStyle" v-if="fa != null" />
            <div v-if="label != null" > {{ label }} </div>
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
                buttonStyle: {
                    color: 'var(--white-color)',
                    ...this.buttonStyle
                },
                iconStyle: {
                    color: 'var(--white-color)',
                    ...this.iconStyle
                },
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
    @import url(@/css/base/button/button.css);
</style>


