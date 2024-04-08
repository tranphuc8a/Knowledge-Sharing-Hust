
import { myEnum } from "@/js/resources/enum";

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
        }
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
        }
    },

    watch: {
        state(value){ this.data.state = value; }
    },
}


export default button;