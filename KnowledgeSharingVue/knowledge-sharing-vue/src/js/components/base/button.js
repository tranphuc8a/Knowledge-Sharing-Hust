
import { myEnum } from "@/js/resources/enum";
import { Validator } from "@/js/utils/validator";

const buttonStateEnum = myEnum.buttonState;
const acceptState = [buttonStateEnum.NORMAL, buttonStateEnum.LOADING, buttonStateEnum.DISABLED];

let button = {
    methods: {
        /**
         * Xử lý sự kiện click chuột vào button
         * @param none
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async resolveOnclick() {
            try {
                if (this.data.state !== myEnum.buttonState.NORMAL){
                    return;
                }
                this.data.state = myEnum.buttonState.LOADING;
                await this.onclick();
                this.data.state = myEnum.buttonState.NORMAL;
            } catch (e) {
                console.error(e);
            }
        },

        /**
         * Xử lý sự kiện thay đổi trạng thái của button
         * @param state - trạng thái của button
         * @returns none
         * @Created PhucTV (7/5/24)
         * @Modified None
         */
        async setState(state) {
            try {
                if (Validator.isEmpty(state)){
                    return;
                }
                if (!acceptState.includes(state)){
                    return;
                }
                this.data.state = state;
            } catch (error){
                console.error(error);
            }
        },
        async loading(){
            try {
                await this.setState(buttonStateEnum.LOADING);
            } catch (error){
                console.error(error);
            }
        },
        async normal(){
            try {
                await this.setState(buttonStateEnum.NORMAL);
            } catch (error){
                console.error(error);
            }
        },
    },

    props: {
        onclick: {
            type: Function,
            default: async function(){
                await new Promise(e => setTimeout(e, 1000));
                console.log(`Click ${this.label}`);
            }
        },
        label: {
            type: String,
            default: null,
        },
        state: {
            default: myEnum.buttonState.NORMAL
        },
        fa: {
            default: null
        },
        iconFamily: {
            type: String,
            default: 'fas'
        },
        buttonStyle: {},
        iconStyle: {
            color: 'white'
        },
    },

    watch: {
        state(value){ this.data.state = value; }
    },
}


export default button;