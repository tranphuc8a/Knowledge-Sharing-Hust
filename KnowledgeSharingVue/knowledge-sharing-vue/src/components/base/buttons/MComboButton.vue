<template>
    
    <div class="p-fitcontent-button-container">
        <div @:click="resolveOnclick" class="p-button p-combo-button" :state="data.state">
            <div class="p-button-content p-content-combo-button">
                <div class="p-combo-button-left">
                    <MIcon :fa="fa" :style="data.style" />
                    <div> {{ label }} </div>
                </div>
                <div class="p-combo-button-right">
                    <MIcon fa="chevron-down" :style="data.style" />
                </div>
            </div>
            <div class="p-loading-container">
                <MSpinner :style="data.style"/>
            </div>
        </div>
    </div>
</template>

<script>
import buttonScript from '@/js/components/base/button';

let button = {
    name: "MComboButton",
    data() {
        return {
            data: {
                state: this.state,
                style: { color: 'white'}
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


