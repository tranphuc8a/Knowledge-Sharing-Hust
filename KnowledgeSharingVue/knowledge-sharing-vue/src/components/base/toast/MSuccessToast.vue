<template>
    <div ref="toast" class="p-toast p-toast-success p-toast-transition">
        <div class="p-toast-content">
            <div class="p-toast-icon">
                <Icon faClassname="pi-sprite-success" />
            </div>
            <div class="p-toast-description">
                <span class="p-toast-title">
                    {{ getLabel().success }}
                </span>
                <span class="p-toast-description-text">
                    {{ description }}
                </span>
            </div>
        </div>
        
        <div class="p-toast-action">
            <LinkButton :label="getLabel().undo" :onclick="resolveOnUndo" />
            <ActionIcon faClassname="pi-sprite-times" :onclick="resolveOnClose" />
        </div>
    </div>
</template>

<script>
import Icon from '@/components/base/icons/MIcon.vue';
import ActionIcon from '@/components/base/icons/MActionIcon.vue';
import LinkButton from '@/components/base/buttons/MLinkButton.vue';
export default {
    data(){
        return{
            label: null
        }
    },
    mounted(){
        this.getLabel();
    },
    components: {
        Icon, ActionIcon, LinkButton
    },
    methods: {
        /**
         * Lấy về label chứa các nhãn trong language resource
         * @param none
         * @return label
         * Created: PhucTV (30/1/24)
         * Modified: None
        */
        getLabel(){
            if (this.label === null || this.label === undefined){
                this.label = this.lang.components.toast;
            }
            return this.label;
        },
        /**
         * Các hàm xử lý sự kiện tương ứng trên toast
         * @param  none
         * @returns none
         * Created: PhucTV (28/1/24)
         * Modified: None
        */
        async resolveOnUndo(){
            await this.onUndo();
        },
        async resolveOnClose(){
            this.$refs.toast.style.display = 'none';
            await this.onClose();
        }
    },
    props: {
        description: {
            type: String,
            default: "Công việc đã bị xóa thành công"
        },
        onClose: {
            type: Function,
            default: async function(){
                console.log("Close Success Toast");
            }
        },
        onUndo: {
            type: Function,
            default: async function(){
                console.log("Undo Success Toast");
            }
        }
    }
}
</script>

<style>
@import url(@/css/base/toast.css);
</style>

