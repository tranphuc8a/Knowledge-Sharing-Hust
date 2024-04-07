
<template>
    <div>
        <div @:click="resolveOnclick" class="p-button p-action-button" :state="data.state">
            <div class="p-button-content">
                <Icon :faClassname="`${faClassname} p-action-icon`" />
            </div>
            <div class="p-loading-container">
                <Spinner color="green" v-show="data.state == buttonState.LOADING"/>
            </div>
                
        </div>
    </div>
</template>

<script>
import Spinner from '../icons/MSpinner.vue';
import Icon from '../icons/MIcon.vue';
import { myEnum } from '@/js/resources/enum';
let button = {
    name: "ActionButton",
    data() {
        return {
            buttonState: myEnum,
            data: {
                state: this.state,
            }
        };
    },
    components: {
        Spinner, Icon
    },
    methods: {
        /**
         * Xử lý sự kiện click chuột vào button
         * @param none
         * @returns none
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        async resolveOnclick() {
            if (this.data.state !== myEnum.buttonState.NORMAL){
                return;
            }
            this.data.state = myEnum.buttonState.LOADING;
            await this.onclick();
            this.data.state = myEnum.buttonState.NORMAL;
        },
    },
    props: {
        faClassname: {
            type: String
        },
        onclick: {
            type: Function,
            default: async function(){
                await new Promise(e => setTimeout(e, 1000));
                console.log(`Click ${this.label}`);
            }
        },
        state: {
            default: myEnum.buttonState.NORMAL
        }
    },
};
export default button;
</script>

<style>
    @import url(@/css/base/button.css);
</style>



