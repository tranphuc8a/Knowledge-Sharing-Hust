<template>
    <div class="p-header">
        <div class="p-header-left">
            <div class="p-header-home">
                <div class="p-header-platform-icon">

                    <Icon faClassname="pi-sprite-nine-dots-white" />
                </div>
                <div class="p-header-home-logo">
                    <img src="../../../assets/img/Logo_Module_TiengViet_White.66947422.svg" alt="Logo">
                </div>
                <div class="p-header-home-name">
                    <!-- Tiền lương -->
                </div>
            </div>
            <div class="p-header-navbar">
                <div class="p-button p-link-button" state="normal">
                    <div onclick="" class="p-button-content">
                        <a href="./html"> {{ getLabel()?.summary }} </a>
                    </div>
                    <div class="p-loading-container">
                        <Spinner />
                    </div>
                </div>
                <div class="p-button p-link-button" state="normal">
                    <div onclick="" class="p-button-content">
                        <a href=""> {{ getLabel()?.salaryComponent }} </a>
                    </div>
                    <div class="p-loading-container">
                        <Spinner />
                    </div>
                </div>
                <div class="p-button p-link-button" state="normal">
                    <div onclick="" class="p-button-content">
                        <a href=""> {{ getLabel()?.salaryTablePattern }} </a>
                    </div>
                    <div class="p-loading-container">
                        <Spinner />
                    </div>
                </div>
                <div class="p-button p-link-button" state="normal">
                    <div onclick="" class="p-button-content">
                        <a href=""> {{ getLabel()?.salaryData }} </a>
                    </div>
                    <div class="p-loading-container">
                        <Spinner />
                    </div>
                </div>
            </div>
        </div>
        <div class="p-header-right">
            <div class="p-header-icons">
                <div class="p-header-search p-icon-container">
                    <i class="pi-sprite-search-dark p-icon"></i>
                </div>
                <div class="p-header-setting p-icon-container">
                    <i class="pi-sprite-setting-dark p-icon"></i>
                </div>
                <div class="p-header-inform p-icon-container">
                    <i class="pi-sprite-bell-dark p-icon"></i>
                </div>
                <div class="p-header-help p-icon-container">
                    <i class="pi-sprite-question-dark p-icon"></i>
                </div>
                <div class="p-header-more p-icon-container">
                    <i class="pi-sprite-three-dots-vertical-dark p-icon"></i>
                </div>
                <div class="p-header-news p-icon-container">
                    <i class="pi-sprite-letter-dark p-icon"></i>
                </div>
            </div>
            <ContextMenu 
                    :position="'bottom right'"
                    :items="getMenuContextItems()"
            >
                <div class="p-header-avatar">
                    <img src="../../../assets/img/none-avatar.jpeg" alt="Avatar">
                </div>
            </ContextMenu>
        </div>

    </div>
</template>

<script>

import Icon from '@/components/base/icons/MIcon.vue';
import Spinner from '@/components/base/icons/MSpinner.vue';
import ContextMenu from '@/components/base/context-menu/MContextMenu.vue';
import { Request } from '@/js/services/request';

export default {
    name: 'HeaderBar',
    components: { Icon, Spinner, ContextMenu } ,
    data(){
        return {
            label: null
        };
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
                this.label = this.lang.layout.header;
            }
            return this.label;
        },

        /**
         * Lấy về menu context items
         * @param none
         * @return menuContextItems
         * @Created PhucTV (30/1/24)
         * @Modified None
        */
        getMenuContextItems(){
            let that = this;
            return {
                top: [{
                    faClassname: 'pi-icon pi-collapse-gray',
                    label: that.getLabel()?.signOut,
                    onclick: that.resolveSignOut
                }],
                bottom: []
            }
        },

        /**
         * Xử lý đăng xuất
         * @param none
         * @return none
         * @Created PhucTV (30/1/24)
         * @Modified None
        */
        async resolveSignOut(){
            try {
                Request.deleteLocalStorage();
                this.$router.push('/login');
            } catch (error) {
                console.error(error);
            }
        }
    }
};

</script>

<style>
@import url(@/css/layouts/header-bar.css);
</style>
