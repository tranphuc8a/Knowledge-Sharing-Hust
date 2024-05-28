<template>
    <!-- <transition-group name="popup" > -->
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
        <div class="p-popup-manager-content">
            <div class="p-popup-icon">
                <MIcon v-if="['info', 'infor', 'inform', 'information'].includes(popup.type)"
                    fa="circle-info" :style="{color: 'blue', fontSize: fontSize}"
                />
                <MIcon v-else-if="['warn', 'warning'].includes(popup.type)"
                    fa="triangle-exclamation" :style="{ color: 'orange', fontSize: fontSize}"
                />
                <MIcon v-else-if="popup.type=='success'"
                    fa="circle-check" :style="{color: 'green', fontSize: fontSize}" 
                />
                <MIcon v-else-if="popup.type=='error'"
                    fa="circle-xmark" :style="{color: 'var(--red-color)', fontSize: fontSize}"             
                />
            </div>
            <div class="p-popup-manager-content-text">
                {{ popup.content }}
            </div>
        </div>
    </Popup>
    <!-- </transition-group> -->

</template>


<script>
import { Validator } from '@/js/utils/validator';
import { MyRandom } from '@/js/utils/myrandom';
import Popup from './MPopup.vue';
// import appConfig from '@/app-config';
// import Common from '@/js/utils/common';
import { useRouter } from 'vue-router';

export default {
    data(){
        return {
            router: useRouter(),
            fontSize: '24px',
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
        Popup
    },
    methods: {
        /**
         * Hàm tổng quát để thực hiện hiển thị popup thông báo
         * @param {*} type - Loại popup (inform, warning, success, error)
         * @param {*} content - Nội dung thông báo của popup
         * @param {options} onOkay - Hàm callback xử lý khi nhấn ok
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
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
         * @Created PhucTV (24/2/24)
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
         * @Created PhucTV (28/1/24)
         * @Modified None
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

        async requiredLogin(){
            try {
                let currentPath = this.router.currentRoute.path;
                if (currentPath != null){
                    localStorage.setItem("redirect-to", currentPath);
                }
                // let homepage = appConfig.getHomePageUrl();
                // homepage = Common.removeTrailingSlash(homepage);
                let router = this.router;
                this.inform("Bạn cần đăng nhập để thực hiện chức năng này!",
                    async function(){
                        // window.location.href = homepage + "/login";
                        router.push("/login");
                    }
                );
            } catch (error){
                console.error(error);
            }
        }
    }
}

</script>


<style scoped>

@import url(@/css/base/popup/popup-manager.css);

</style>