<template>
    <div class="p-toast p-toast-inform" ref="toast">
        <div class="p-toast-content">
            <div class="p-toast-icon">
                <MIcon fa="circle-info" :style="{color: 'blue'}" />
            </div>
            <div class="p-toast-description">
                <span class="p-toast-title">
                    {{ getLabel()?.inform }}
                </span>
                <span class="p-toast-description-text">
                    {{ description }}
                </span>
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
// import LinkButton from '@/components/base/buttons/MLinkButton.vue';
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

