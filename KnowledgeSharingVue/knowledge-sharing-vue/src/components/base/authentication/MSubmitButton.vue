<template>
    <div>
        <div @:click="resolveOnclick" class="p-button p-normal-button" :state="data.state">
            <div class="p-button-content">
                {{ label }}
            </div>
            <div class="p-loading-container">
                <Spinner color="white" v-show="data.state == buttonState.LOADING"/>
            </div>
            
        </div>
    </div>
</template>

<script>
import Spinner from '@/components/base/icons/MSpinner.vue';
import { myEnum } from '@/js/resources/enum';
let button = {
    name: "MSubmitButton",
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
         * @Created PhucTV (28/1/24)
         * @Modified None
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
            default: async function(){}
        },
        state: {
            default: myEnum.buttonState.NORMAL
        }
    },
};
export default button;
</script>


<style scoped>
@import url(@/css/base/authentication/submit-button.css);
.p-button {
    width: 100%;
    color: #fff;
    line-height: 40px;
    font-weight: 500;
    font-size: 14px;
    height: 40px;
}
</style>