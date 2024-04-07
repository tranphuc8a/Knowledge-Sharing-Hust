<template>
    <div>
        <div @:click="resolveOnclick" class="p-button p-cancel-button" :state="data.state">
            <div class="p-button-content">
                {{ label }}
            </div>
            <div class="p-loading-container">
                <Spinner color="black"/>
            </div>
        </div>
    </div>
</template>

<script>
import Spinner from '../icons/MSpinner.vue';
import { myEnum } from '@/js/resources/enum';
let button = {
    name: "CancelButton",
    data() {
        return {
            buttonState: myEnum,
            data: {
                state: this.state,
            }
        };
    },
    components: {Spinner},
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

<style scoped>
    @import url(@/css/base/button.css);
</style>


