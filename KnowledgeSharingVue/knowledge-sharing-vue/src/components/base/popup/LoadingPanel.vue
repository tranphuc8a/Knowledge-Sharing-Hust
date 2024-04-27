<template>
    <div class="p-loading-panel" v-show="isShow">
        <div class="p-loading-container">
            <MSpinner :style="spinnerStyle" />
        </div>
    </div>
</template>

<script>

export default {
    name: 'LoadingPanel',
    data(){
        return {
            spinnerStyle: {
                width: '36px',
                height: '36px'
            },
            isShow: false,
        }
    },
    methods: {
        /**
         * Ham thuc hien hien thi loading panel
         * @param none
         * @return none
         * @Created PhucTV (25/04/24)
         * @Modified None
         */
        show(){
            this.isShow = true;
        },
        /**
         * Ham thuc hien an loading panel
         * @param none
         * @return none
         * @Created PhucTV (25/04/24)
         * @Modified None
         */
        hide(){
            this.isShow = false;
        }
    }
}

</script>

<style scoped>

.p-loading-panel {
    width: 100vw;
    height: 100vh;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(var(--primary-color-rgb), .16);

    display: flex;
    justify-content: center;
    align-items: center;
}

.p-loading-container{
    display: flex;
    justify-content: center;
    align-items: center;

    padding: 8px;
    border-radius: 100%;
    background-color: white;
}

</style>

