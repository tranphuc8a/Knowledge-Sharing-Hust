<template>
    <Popup  v-for="popup in this.listPopup"
            :key="popup.id"
            :isShowDescription="false"
            :isShowPreviousButton="false"
            :isShowCancelButton="true"
            :isShowOkayButton="true"
        
            :on-okay="popup?.callbacks?.onOkay"
            :on-close="hidePopup(popup)"
            :on-cancel="hidePopup(popup)"
            :on-previous="popup?.callbacks?.onPrevious"

            :header="lang?.header"
            :previousButtonLabel="lang?.previousButtonLabel"
            :cancelButtonLabel="lang?.cancelButtonLabel"
            :okayButtonLabel="lang?.okayButtonLabel"
    >
        <div class="p-popup-content">
            <div class="p-popup-icon">
                <Icon v-show="['info', 'infor', 'inform', 'information'].includes(type)"
                    faClassname="pi-sprite-info-big"
                />
                <Icon v-show="['warn', 'warning'].includes(type)"
                    faClassname="pi-sprite-warn-big"
                />
                <Icon v-show="type=='success'"
                    faClassname="pi-sprite-success-big" 
                />
                <Icon v-show="type=='error'"
                    faClassname="pi-sprite-error-big"                
                />
            </div>
            <div class="p-popup-content-text">
                {{ popup.content }}
            </div>
        </div>
    </Popup>
</template>


<script>
import { Validator } from '@/js/utils/validator';
import { MyRandom } from '@/js/utils/myrandom';
import Popup from './MPopup.vue';
import Icon from '../icons/MIcon.vue';

export default {
    data(){
        return {
            listPopup: [],
            lang: this.lang.components.popupManager,
            type: "inform",
            content: "",
            acceptedTypes: ["inform", "info", "infor", "information", 
                            "success", "error", "warn", "warning"],
        }
    },
    mounted(){
    },
    components: {
        Popup, Icon
    },
    methods: {
        /**
         * Hàm tổng quát để thực hiện hiển thị popup thông báo
         * @param {*} type - Loại popup (inform, warning, success, error)
         * @param {*} content - Nội dung thông báo của popup
         * @param {options} onOkay - Hàm callback xử lý khi nhấn ok
         * @returns none
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        async show(type, content, onOkay = null){
            try {
                if (! this.acceptedTypes.includes(type)){
                    throw new Error(this.lang?.formatError);
                }
                if (Validator.isEmpty(content)){
                    throw new Error(this.lang?.emptyError);
                }
                let that = this;
                let popupInfor = {
                    type: type,
                    content: content,
                    callbacks: {
                        onOkay: null,
                    },
                    id: MyRandom.generateUUID()
                };
                popupInfor.callbacks.onOkay = async function(){
                    if (onOkay) {
                        await onOkay();
                    }
                    await that.hidePopup(popupInfor)();
                }
                this.listPopup.push(popupInfor);
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Ẩn một popup khỏi danh sách popup
         * @param {*} popup - popup cần ẩn đi
         * @returns none
         * Created: PhucTV (24/2/24)
         * Modifie: None 
        */
        hidePopup(popup){
            let that = this;
            return async function(){
                try {
                    that.listPopup = that.listPopup.filter(function(p){
                        return p.id !== popup.id;
                    });
                } catch (error){
                    console.error(error);
                }
            }
        },

        /**
         * Các hàm thực hiện show popup theo từng loại cụ thể
         * @param {*} content - Nội dung thông báo của popup
         * @param {options} onOkay - Hàm callback xử lý khi nhấn ok
         * @returns none
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        async success(content, onOkay = null){
            await this.show("success", content, onOkay);
        },
        async error(content, onOkay = null){
            await this.show("error", content, onOkay);
        },
        async info(content, onOkay = null){
            await this.show("inform", content, onOkay);
        },
        async infor(content, onOkay = null){
            await this.show("inform", content, onOkay);
        },
        async inform(content, onOkay = null){
            await this.show("inform", content, onOkay);
        },
        async information(content, onOkay = null){
            await this.show("inform", content, onOkay);
        },
        async warn(content, onOkay = null){
            await this.show("warn", content, onOkay);
        },
        async warning(content, onOkay = null){
            await this.show("warning", content, onOkay);
        },
    }
}

</script>


<style scoped>

.p-popup-content{
    display: flex;
    justify-content: space-between;
    flex-flow: row nowrap;
    align-items: center;
    width: 100%;
    margin: 0;
    padding: 12px 12px 24px 12px;
    gap: 8px;
}

.p-popup-content > div{
    flex: 0 0 auto;
}

.p-popup-content .p-popup-content-text{
    flex: 1 0 auto;
}

</style>