<template>
    <div class="p-toast p-toast-error" ref="toast">
        <div class="p-toast-content">
            <div class="p-toast-icon">
                <Icon faClassname="pi-sprite-error" />
            </div>
            <div class="p-toast-description">
                <span class="p-toast-title">
                    {{ getLabel().error }}
                </span>
                <span class="p-toast-description-text">
                    {{ `${description} ` }}
                </span>
                <LinkButton :label="getLabel().help" :onclick="resolveClickHelp" />
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
         * @Created PhucTV (30/1/24)
         * @Modified None
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
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async resolveOnUndo(){
            await this.onUndo();
        },
        async resolveOnClose(){
            this.$refs.toast.style.display = 'none';
            await this.onClose();
        },
        async resolveClickHelp(){
            await this.onHelp();
        }
    },
    props: {
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

