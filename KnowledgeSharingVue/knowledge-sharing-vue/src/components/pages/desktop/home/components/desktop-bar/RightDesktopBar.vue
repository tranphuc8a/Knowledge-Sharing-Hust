
<template>
    <div class="d-rightbar">
        <div class="d-rightbar__message" :title="getLabel()?.message">
            <MActionIcon fa="envelope" :onclick="resolveClickMessageIcon" 
                :iconStyle="iconStyle" :containerStyle="containerStyle"
            />
        </div>
        <div class="d-rightbar__notification" :title="getLabel()?.notification">
            <MActionIcon fa="bell" :onclick="resolveClickNotificationIcon" 
                :iconStyle="iconStyle" :containerStyle="containerStyle"/>
        </div>
        <div class="d-rightbar__avatar">
            <AvatarMenuContext />
        </div>
    </div>
</template>

<script>
import MActionIcon from '@/components/base/icons/MActionIcon'
import AvatarMenuContext from './AvatarMenuContext.vue';
import { useRouter } from 'vue-router';
export default {
    name: "RightDesktopBar",
    data() {
        return {
            iconStyle: {
                fontSize: '24px'
            },
            containerStyle: {
                width: '40px',
                height: '40px'
            },
            textfield: null,
            router: useRouter()
        }
    },
    mounted() {
        this.getLabel();
        this.textfield = this.$refs.textfield;
    },
    components: {
        AvatarMenuContext, MActionIcon
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.pages?.homepage?.homeDbarResource;
            }
            return this.label;
        },

        /**
         * Hàm thực hiện click icon message
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         */
        async resolveClickMessageIcon(){
            try {
                // navigate to message page
                // this.router.push({name: 'message'});
            } catch (error) {
                console.error(error);
            }
        },

        /**
         * Hàm thực hiện click icon notification
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         */
        async resolveClickNotificationIcon(){
            try {
                // navigate to message page
                // this.router.push({name: 'message'});
            } catch (error) {
                console.error(error);
            }
        }
        
    },
    props: {

    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
@import url(@/css/pages/desktop/home/components/desktop-bar.css);
</style>