<template>
    <SuccessToast :description="description"
        :onClose="onClose" :onUndo="onUndo"
        v-if="['success'].includes(type)" />
    <InformToast :description="description"
        :onClose="onClose" :onUndo="onUndo"
        v-else-if="['inform', 'infor', 'info'].includes(type)" />
    <WarningToast :description="description"
        :onClose="onClose" :onUndo="onUndo"
        v-else-if="['warning', 'warn'].includes(type)" />
    <ErrorToast :description="description"
        :onClose="onClose" :onUndo="onUndo" :onHelp="onHelp"
        v-else-if="['error'].includes(type)" />
</template>

<script>
import SuccessToast from './MSuccessToast.vue';
import InformToast from './MInformToast.vue';
import WarningToast from './MWarningToast.vue';
import ErrorToast from './MErrorToast.vue';
import toast from '@/js/components/base/toast';

export default {
    name:'MyToast',
    components: { SuccessToast, InformToast, WarningToast, ErrorToast},
    props: {
        ...toast.props,
        type: {
            type: String,
            default: "success",
            validator: function(type){
                return ["success", "inform", "infor", "info", "warning", "error", "warn"].includes(type);
            }
        }
    }
}
</script>

