<template>
    <div>
        <div @:click="resolveOnclick" class="p-button p-icon-button" :state="data.state">
            <div class="p-button-content p-content-button">
                <Icon :faClassname="faClassname" color="white"  />
                <div> {{ label }} </div>
            </div>
            <div class="p-loading-container">
                <Spinner color="white" v-show="data.state==buttonState.LOADING" />
            </div>
        </div>
    </div>
</template>

<script>
import Spinner from '../icons/MSpinner.vue';
import Icon from '../icons/MIcon.vue';
import { myEnum } from '@/js/resources/enum';
let button = {
    name: "IconButton",
    data() {
        return {
            buttonState: myEnum.buttonState,
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

        /**
         * Lấy về className của icon
         * @param none
         * @returns string - className của icon cần lấy
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        getIconClassName(){
            return `fa ${this.faClassname} p-icon p-white-icon`;
        }
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
        faClassname: {
            type: String
        },
        colorIcon: {
            type: String,
            default: "white"
        }, 
        state: {
            default: "normal"
        }
    },
};
export default button;
</script>

<style scoped>
    @import url(@/css/base/button.css);
</style>


