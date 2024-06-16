<template>
    <div class="p-body">
        <RouterView />
        <!-- <router-view v-slot="{ Component }">
            <keep-alive>
                <component :is="Component" />
            </keep-alive>
        </router-view> -->
    </div>

    <PopupManager ref="popupManager"/>
    <ToastManager ref="toastManager"/>
    <LoadingPanel ref="loadingPanel"/>
    <ConversationPopup ref="conversationPopup"/>
    <SelectImagePopup ref="selectImagePopup"/>
    
</template>

<script>
import LoadingPanel from './components/base/popup/LoadingPanel.vue';
import PopupManager from '@/components/base/popup/MPopupManager.vue';
import ToastManager from '@/components/base/toast/MToastManager.vue';
import { language } from '@/js/resources/language';
import resolveAxiosResponse from '@/js/services/resolve-axios-response';
import CurrentUser from './js/models/entities/current-user';
import ConversationPopup from './components/pages/desktop/conversation/conversation-popup/ConversationPopup.vue';
import SelectImagePopup from './components/base/popup/select-image-popup/SelectImagePopup.vue';
// import Common from './js/utils/common';

export default {
    name: 'KSEmptyPage',
    data(){
        return {
            languageEnum: this.globalData.enum.language
        }
    },
    components: {
        PopupManager, ToastManager, LoadingPanel,
        ConversationPopup, SelectImagePopup
    },
    mounted(){
        this.toastManager = this.$refs.toastManager;
        this.popupManager = this.$refs.popupManager;
        this.loadingPanel = this.$refs.loadingPanel;
        resolveAxiosResponse.updateManager(this.toastManager, this.popupManager);

        this.registerLoadingPanel();
        // Common.loadRecaptchaScript();
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
        getLoadingPanel(){
            return this.$refs.loadingPanel;
        },
        getConversationPopup(){
            return this.$refs.conversationPopup;
        },
        getSelectImagePopup(){
            return this.$refs.selectImagePopup;
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
                if (lang === this.languageEnum.VIETNAMESE){
                    this.globalData.lang = language['vi'];
                    console.log("Change language to " + 'vi');
                } else if (lang === this.languageEnum.ENGLISH){
                    this.globalData.lang = language['en'];
                    console.log("Change language to " + 'en');
                }
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Hàm thực hiện dang ky ham hien thi loading panel cho globalMethod
         * @param none
         * @returns none
         * @Created PhucTV (25/4/24)
         * @Modified None
         */ 
        registerLoadingPanel(){
            try {
                let that = this;
                this.globalMethods.getLoadingPanel = function(){
                    return that.loadingPanel;
                }
            } catch (e) {
                console.error(e);
            }
        },


        async getCurrentUser(){
            try {
                return await CurrentUser.getInstance();
            } catch (e){
                return null;
            }
        }
    },
    provide(){
        return {
            parent: this,
            getToastManager: this.getToastManager,
            getPopupManager: this.getPopupManager,
            getLoadingPanel: this.getLoadingPanel,
            getConversationPopup: this.getConversationPopup,
            getSelectImagePopup: this.getSelectImagePopup,
            getCurrentUser: this.getCurrentUser,
            getLanguage: () => this.globalData.lang,
            changeLanguage: this.changeLanguage
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


<style>

@import url(@/css/main.css);
</style>
