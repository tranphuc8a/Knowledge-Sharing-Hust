<template>
    <div>
        <div @:click.prevent="resolveOnclick" class="p-button p-link-button" :state="data.state">
            <div class="p-button-content">
                <a :href="href"> {{ label }} </a>  
            </div>
            <div class="p-loading-container">
                <Spinner color="green" v-show="data.state == buttonState.LOADING"/>
            </div>
        </div>
    </div>
</template>

<script>
import Spinner from '../icons/MSpinner.vue';
import { myEnum } from '@/js/resources/enum';
let button = {
    name: "LinkButton",
    data() {
        return {
            buttonState: myEnum.buttonState,
            data: {
                state: this.state,
            }
        };
    },
    components: {
        Spinner
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
        href: {
            type: String,
            default: ""
        },
        label: {
            type: String,
            default: "Thêm mới",
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


