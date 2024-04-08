<template>
    <div v-if="showLoading" class="loading-panel">
        <div class="loading-overlay"></div>
        <div class="loading-content">
            <Spinner class="pi-icon-big" />
        </div>
    </div>
</template>

<script>
import Spinner from "@/components/base/icons/MSpinner.vue";

export default {
    name: 'KSLoadingPanel',
    components: {
        Spinner
    },
    data() {
        return {
            showLoading: false
        };
    },
    methods: {
        showLoadingPanel() {
            this.showLoading = true;
        },
        hideLoadingPanel() {
            this.showLoading = false;
        }
    }
}
</script>

<style scoped>
.loading-panel {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

.loading-overlay {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(255, 255, 255, 0.8);
}

.loading-content {
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>
