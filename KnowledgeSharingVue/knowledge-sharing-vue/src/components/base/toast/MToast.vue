<template>
    <div>
        <SuccessToast :description="description"
            :onClose="onClose" :onUndo="onUndo"
            v-if="['success'].includes(type)" />
        <InformToast :description="description"
            :onClose="onClose" :onUndo="onUndo"
            v-if="['inform', 'infor', 'info'].includes(type)" />
        <WarningToast :description="description"
            :onClose="onClose" :onUndo="onUndo"
            v-if="['warning', 'warn'].includes(type)" />
        <ErrorToast :description="description"
            :onClose="onClose" :onUndo="onUndo" :onHelp="onHelp"
            v-if="['error'].includes(type)" />
    </div>
</template>

<script>
import SuccessToast from './MSuccessToast.vue';
import InformToast from './MInformToast.vue';
import WarningToast from './MWarningToast.vue';
import ErrorToast from './MErrorToast.vue';

export default {
    name:'MyToast',
    components: { SuccessToast, InformToast, WarningToast, ErrorToast},
    props: {
        type: {
            type: String,
            default: "success",
            validator: function(type){
                return ["success", "inform", "infor", "info", "warning", "error", "warn"].includes(type);
            }
        },
        description: {
            type: String,
            default: "Đây là toast báo lỗi"
        },
        onClose: {
            type: Function,
            default: async function(){
                console.log("Close Warning Toast");
            }
        },
        onUndo: {
            type: Function,
            default: async function(){
                console.log("Undo Warning Toast");
            }
        },
        onHelp: {
            type: Function,
            default: async function(){
                console.log("Helo Error Toast");
            }
        }
    }
}
</script>

<style>
@import url(@/css/base/toast.css);
</style>

