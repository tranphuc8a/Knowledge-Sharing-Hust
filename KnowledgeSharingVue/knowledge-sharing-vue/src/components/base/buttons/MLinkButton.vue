<template>
    <div class="p-fitcontent-button-container">
        <div @:click.prevent="resolveOnclick" class="p-button p-link-button" :state="data.state">
            <div class="p-button-content">
                <a :href="href"> {{ label }} </a>  
            </div>
            <div class="p-loading-container">
                <MSpinner />
            </div>
        </div>
    </div>
</template>

<script>
import buttonScript from '@/js/components/base/button';
import { myEnum } from '@/js/resources/enum';

let button = {
    name: "LinkButton",
    data() {
        return {
            data: {
                state: this.state,
            }
        };
    },
    methods: {
        ...buttonScript.methods,
        // Override the default method
        async resolveOnclick(){
            if (this.data.state !== myEnum.buttonState.NORMAL){
                return;
            }
            try {
                this.data.state = myEnum.buttonState.LOADING;
                if (this.href != null){
                    location.href = this.href;
                } else if (this.onclick != null){
                    await this.onclick();
                }
            } catch (error){
                console.error(error);
            } finally {
                this.data.state = myEnum.buttonState.NORMAL;
            }
        }
    },
    watch: {
        ...buttonScript.watch
    },
    props: {
        ...buttonScript.props,
        href: {}
    },
};
export default button;
</script>

<style>
    @import url(@/css/base/button/button.css);
</style>


