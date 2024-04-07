<template>
    <div class="p-sidebar" :state="state">             
        <div class="p-sidebar-menu">
            <router-link to="/" class="p-sidebar-item">
                <div class="p-sidebar-item-icon">
                    <i class="pi-icon pi-home-white"></i>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().homepage }}
                </div>
            </router-link>
            <router-link to="/customer" class="p-sidebar-item">
                <div class="p-sidebar-item-icon">
                    <i class="pi-icon pi-vcard-white"></i>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().customer }}
                </div>
            </router-link>
            <router-link to="/employee" class="p-sidebar-item">
                <div class="p-sidebar-item-icon">
                    <i class="pi-icon pi-user-white"></i>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().employee }}
                </div>
            </router-link>
            <router-link to="/import" class="p-sidebar-item">
                <div class="p-sidebar-item-icon">
                    <Icon faClassname="pi-icon pi-excel-white"/>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().import }}
                </div>
            </router-link>
            <router-link to="/setting" class="p-sidebar-item">
                <div class="p-sidebar-item-icon">
                    <i class="pi-icon pi-gear-white"></i>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().setting }}
                </div>
            </router-link>
        </div>
        <div class="p-sidebar-action">
            <div class="p-sidebar-item" @:click="changeExpand">
                <div class="p-sidebar-item-icon p-collapse">
                    <i class="pi-icon pi-collapse-white"></i>
                </div>
                <div class="p-sidebar-item-title">
                    {{ getLabel().collapse }}
                </div>
                <div class="p-sidebar-item-icon p-expand">
                    <i class="pi-icon pi-expand-white"></i>
                </div>
            </div>
        </div>
    </div>
</template>


<script>

import Icon from '@/components/base/icons/MIcon.vue';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'SideBar',
    data(){
        return {
            state: 'normal',
            label: null
        }
    },
    components: { Icon },
    mounted(){
        this.getLabel();
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
                this.label = this.lang.layout.sidebar;
            }
            return this.label;
        },
        /* 
        * Thay đổi trạng thái expand của sidebar
        * @param none
        * @Author TVPhuc (19/12/23)
        * @Edit None
        **/
        async changeExpand(){
            try {
                if (this.state === myEnum.sidebarState.COLLAPSE){
                    this.switchState(myEnum.sidebarState.NORMAL);
                } else {
                    this.switchState(myEnum.sidebarState.COLLAPSE);
                }
            } catch (error){
                console.error(error);
            }
        },
        /*
        * Thay đổi trạng thái sidebar
        * @param none
        * @Author TVPhuc (19/12/23)
        * @Edit None
        **/
        switchState(state){
            this.state = state;
        }
    }
};
</script>

<style>
@import url(@/css/layouts/sidebar.css);
</style>