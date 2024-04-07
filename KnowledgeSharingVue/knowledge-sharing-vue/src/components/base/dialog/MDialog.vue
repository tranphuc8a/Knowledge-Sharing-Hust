<template>
    <div class="p-dialog-container" v-show="dIsShow">
        <div class="p-dialog">
            <div class="p-dialog-header">
                <div class="p-dialog-title">
                    {{ header }}
                </div>
                <ActionIcon faClassname="pi-sprite-times p-red-icon" :onclick="resolveClickClose" 
                            :style="iconStyle" :iconStyle="iconStyle"/>
            </div>
            <div class="p-dialog-content">
                <div class="p-dialog-content-icon">
                    <i class="fa pi-sprite-dialog-big p-icon p-red-icon"> </i>
                </div>
                <div class="p-dialog-description">
                    {{ description }}
                </div>
            </div>
            <div class="p-dialog-actions">
                <div class="p-dialog-buttons">
                    <CancelButton :label="cancelButtonLabel" :onclick="onCancel" />
                    <NormalButton :label="okayButtonLabel" :onclick="onOkay" />
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
    name: "MyDialog",
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
    components: {NormalButton, CancelButton, ActionIcon},
    mounted() {

    },
    methods: {
        /*
        * Hai phương thức giúp ẩn hiện dialog
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
        /*
        * Thực hiện xử lý đóng dialog
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveClickClose(){
            await this.onCancel();
        }
    },
    props: {
        header: {
            type: String,
            default: "Đây là tiêu đề"
        }, description: {
            type: String,
            default: "Đây là mô tả dialog",
        },
        cancelButtonLabel: {
            type: String,
            default: "Hủy bỏ"
        },
        okayButtonLabel: {
            type: String,
            default: "Đồng ý"
        },
        onCancel: {
            type: Function,
            default: async function() {
                console.log("Click cancel dialog");
            }
        },
        onOkay: {
            type: Function,
            default: async function() {
                console.log("Click okay dialog");
            }
        }
    }
};

</script>

<style scoped>
    @import url(@/css/base/dialog.css);
</style>


