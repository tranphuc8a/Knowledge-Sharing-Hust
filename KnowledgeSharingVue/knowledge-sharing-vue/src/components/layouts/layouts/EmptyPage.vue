<template>
    <div class="p-body">
        <RouterView />
    </div>

    <PopupManager ref="popupManager"/>
    <ToastManager ref="toastManager"/>
</template>

<script>

import PopupManager from '@/components/base/popup/MPopupManager.vue';
import ToastManager from '@/components/base/toast/MToastManager.vue';
import { language } from '@/js/resources/language';
import resolveAxiosResponse from '@/js/services/resolve-axios-response';

export default {
    name: 'KSEmptyPage',
    data(){
        return {
            inject: {
                language: this.globalData.lang,
                changeLanguage: this.changeLanguage
            },
            languageEnum: this.globalData.enum.language
        }
    },
    components: {
        PopupManager, ToastManager
    },
    mounted(){
        this.toastManager = this.$refs.toastManager;
        this.popupManager = this.$refs.popupManager;
        resolveAxiosResponse.updateManager(this.toastManager, this.popupManager);
    },
    methods: {
        /**
         * Các hàm thực hiện lấy về đối tượng được inject cho các components con
         * @param none
         * @returns đối tượng cần inject
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        getToastManager(){
            return this.$refs.toastManager;
        },
        getPopupManager(){
            return this.$refs.popupManager;
        },
        /**
         * Hàm thực hiện thay đổi ngôn ngữ cho các component nhận inject language của trang này
         * @param {*} lang - ngôn ngữ cần thay đổi (theo enum language)
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None 
        */
        changeLanguage(lang){
            try {
                if (lang === this.languageEnum.VIETNAMESE && this.getLanguage() !== language['vi']){
                    this.inject.language = language['vi'];
                    // console.log("Change language to " + 'vi');
                } else if (lang === this.languageEnum.ENGLISH){
                    this.inject.language = language['en'];
                    // console.log("Change language to " + 'en');
                }
            } catch (error){
                console.error(error);
            }
        }
    },
    provide(){
        return {
            parent: this,
            getToastManager: this.getToastManager,
            getPopupManager: this.getPopupManager,

            inject: this.inject
        }
    }
}
</script>

<style scoped>
.p-body{
    width: 100%;
    height: 100vh;
}
</style>
