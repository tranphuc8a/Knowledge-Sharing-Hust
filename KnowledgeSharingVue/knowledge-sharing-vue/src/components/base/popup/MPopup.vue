<template>
    <div class="p-popup-background" v-show="true">
        <div class="p-popup">
            <div class="p-popup-header">
                <div class="p-popup-title">
                    {{ header }}
                </div>
                <ActionIcon faClassname="pi-sprite-times p-red-icon" :onclick="resolveOnClose" 
                        :style="iconStyle" :iconStyle="iconStyle"/>
            </div>

            <div class="p-popup-content">
                <div class="p-popup-description" v-show="isShowDescription">
                    {{ description }}
                </div>
                <slot>
                        
                </slot>
            </div>
            <div class="p-popup-footer">
                <div class="p-popup-buttons">
                    <div class="p-popup-left-buttons">
                        <CancelButton :label="previousButtonLabel" :onclick="resolveOnPrevious" 
                                      v-show="isShowPreviousButton" />
                    </div>
                    <div class="p-popup-right-buttons">
                        <CancelButton :label="cancelButtonLabel" :onclick="resolveOnCancel" 
                                        v-show="isShowCancelButton"/>
                        <NormalButton :label="okayButtonLabel" :onclick="resolveOnOkay" 
                                        v-show="isShowOkayButton"/>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</template>

<script>
import NormalButton from '@/components/base/buttons/MButton.vue';
import CancelButton from '@/components/base/buttons/MCancelButton.vue';
import ActionIcon from '@/components/base/icons/MActionIcon.vue';

export default {
    name: "MyPopup",
    data() {
        return {
            dIsShow: true,
            iconStyle: {
                width: "32px",
                height: "32px",
                'font-size': "24px"
            }
        };
    },
    components: { NormalButton, CancelButton, ActionIcon },
    mounted() {

    },
    methods: {
        /**
        * Hai phương thức ẩn/hiện popup
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async show(){
            this.dIsShow = true;
        },
        async hide(){
            this.dIsShow = false;
        }, 
        /**
        * Xử lý logic khi close popup
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnClose(){
            try {
                if (this.onClose){
                    await this.onClose();
                }
                await this.hide();
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Xử lý logic khi click previous popup
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnPrevious(){
            try {
                if (this.onPrevious){
                    await this.onPrevious();
                }
                if (this.isAutoHide){
                    await this.hide();
                }
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Xử lý logic khi click Okay
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnOkay(){
            try {
                if (this.onOkay){
                    await this.onOkay();
                }
                if (this.isAutoHide){
                    await this.hide();
                }
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Xử lý logic khi click cancel button
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnCancel(){
            try {
                if (this.onCancel){
                    await this.onCancel();
                }
                if (this.isAutoHide){
                    await this.hide();
                }
            } catch (error){
                console.error(error);
            }
        }
    },
    props: {
        // labels
        header: { default: "Đây là tiêu đề" }, 
        description: { default: "Đây là mô tả popup", },
        previousButtonLabel: { default: "Quay lại" },
        cancelButtonLabel: { default: "Hủy bỏ" },
        okayButtonLabel: { default: "Đồng ý"},
        // events
        onPrevious: {
            type: Function,
            default: async function() {}
        },
        onCancel: {
            type: Function,
            default: async function() {}
        },
        onOkay: {
            type: Function,
            default: async function() {}
        },
        onClose: {
            type: Function,
            default: async function() {}
        },
        // is show element:
        isAutoHide: { default: false },
        isShow: { default: true },
        isShowDescription: { default: true },
        isShowPreviousButton: { default: true },
        isShowCancelButton: { default: true },
        isShowOkayButton: { default: true }
    }
};

</script>

<style scoped>
    @import url(@/css/base/popup/popup.css);
</style>


