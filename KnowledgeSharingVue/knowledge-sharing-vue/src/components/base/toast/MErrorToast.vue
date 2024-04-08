<template>
    <div class="p-toast p-toast-error" ref="toast">
        <div class="p-toast-content">
            <div class="p-toast-icon">
                <MIcon fa="circle-xmark" :style="{color: 'var(--red-color)'}" />
            </div>
            <div class="p-toast-description">
                <span class="p-toast-title">
                    {{ getLabel()?.error }}
                </span>
                <span class="p-toast-description-text">
                    {{ description }}
                </span>
                <!--  <LinkButton :label="getLabel()?.help" :onclick="resolveClickHelp" />  --> 
            </div>
        </div>
        <div class="p-toast-action">
            <!-- <LinkButton :label="getLabel()?.undo" :onclick="resolveOnUndo" /> -->
            <ActionIcon fa="times" :onclick="resolveOnClose" />
        </div>
    </div>
    
</template>

<script>
import ActionIcon from '@/components/base/icons/MActionIcon.vue';
//import LinkButton from '@/components/base/buttons/MLinkButton.vue';
import toast from '@/js/components/base/toast';

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
        ActionIcon
    },
    methods: {
        ...toast.methods
    },
    props: {
        ...toast.props
    }
}
</script>

<style>
@import url(@/css/base/toast/toast.css);
</style>

