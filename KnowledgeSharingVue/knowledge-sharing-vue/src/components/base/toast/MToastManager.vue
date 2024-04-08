<template>
    <div class="p-toast-manager">
        <transition-group name="toast">
            <Toast v-for="(toast) in listToast" :key="toast.index" :description="toast.description" ref="toasts" :type="toast.type"/>
        </transition-group>
    </div>
</template>


<script>
import Toast from './MToast.vue';
export default {
    data(){
        return {
            listToast: [],
            index: 0
        }
    },
    components: {Toast},
    methods: {
        /**
         * Hàm thực hiện thêm một toast mới vào khung toast hiện tại
         * @param {*} type - loại toast cần thêm
         * @param {*} description - mô tả của toast
         * @param {*} duration - thời gian của toast
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async addToast(type = "success", description = "This is message", duration = 3000){
            this.index += 1;
            this.listToast.push({
                type: type,
                description: description,
                index: this.index
            });
            let that = this;
            let id = this.index;
            setTimeout(function(){
                that.listToast = that.listToast.filter(function(toast){
                    return toast.index != id;
                });
            }, duration);
        },

        /**
         * Hàm thực hiện thêm một toast thuộc 1 loại Cụ thể
         * @param {*} description - mô tả của toast
         * @param {*} duration - thời gian của toast
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async success(description = "This is message", duration = 3000){
            await this.addToast("success", description, duration);
        },
        async information(description = "This is message", duration = 3000){
            await this.addToast("inform", description, duration);
        },
        async inform(description = "This is message", duration = 3000){
            await this.addToast("inform", description, duration);
        },
        async infor(description = "This is message", duration = 3000){
            await this.addToast("inform", description, duration);
        },
        async info(description = "This is message", duration = 3000){
            await this.addToast("inform", description, duration);
        },
        async warning(description = "This is message", duration = 3000){
            await this.addToast("warning", description, duration);
        },
        async warn(description = "This is message", duration = 3000){
            await this.addToast("warning", description, duration);
        },
        async error(description = "This is message", duration = 3000){
            await this.addToast("error", description, duration);
        }
    },
}
</script>

<style scoped>
@import url(@/css/base/toast/toast-manager.css);
</style>

