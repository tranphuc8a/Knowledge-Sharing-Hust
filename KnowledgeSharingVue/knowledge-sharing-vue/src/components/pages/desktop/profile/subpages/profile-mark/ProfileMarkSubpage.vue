

<template>
    <div class="p-profile-mark-subpage p-profile-main-subpage">
        <div class="p-profile-mark-card card">
            <div class="p-profile-mark-header">
                <div class="p-pmh-heading card-heading">
                    Đã lưu
                </div>
                <div class="p-pmh-search-button">
                    <!-- Textfield Button -->
                    <!-- <MTextfieldButton 
                        placeholder="Tìm kiếm bạn bè"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveOnClickSearch" 
                        :validator="null"
                        state="normal"
                        ref="textfield"
                    /> -->
                </div>
            </div>

            <div class="p-profile-mark-navigation">
                <!-- Profile Friend Navigation -->
                <ProfileMarkNavigation />
            </div>
        </div>

        <div class="p-profile-mark-content">
            <router-view />
        </div>
    </div>
</template>



<script>
// import { Validator } from '@/js/utils/validator';
// import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue';
import ProfileMarkNavigation from './ProfileMarkNavigation.vue';


export default {
    name: 'ProfileFriendSubpage',
    components: {
        // MTextfieldButton,
        ProfileMarkNavigation
    },
    props: {
    },
    data(){
        return {
            // onClickSearch: null,
        }
    },
    async mounted(){
    },
    methods: {
        // registerOnClickSearch(handler){
        //     if (handler){
        //         this.onClickSearch = handler;
        //     }
        // },

        // async resolveOnClickSearch(){
        //     try {
        //         // get search text:
        //         let searchText = await this.$refs.textfield.getValue();
        //         // if (Validator.isEmpty(searchText)){
        //         //     return;
        //         // }

        //         // call handler
        //         if (this.onClickSearch != null){
        //             await this.onClickSearch(searchText);
        //         }
        //     } catch (e){
        //         console.error(e);
        //     }
        // }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return {
            // registerOnClickSearch: this.registerOnClickSearch,
        }
    }
}

</script>


<style scoped>

.p-profile-mark-subpage {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    gap: 16px;
}

.p-profile-mark-card {
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    padding: 16px 16px 0 16px;
    gap: 16px;
    z-index: 1;
    position: sticky;
    top: 16px;
}

.p-profile-mark-header {
    width: 100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
}

.p-profile-mark-navigation {
    width: 100%;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
}

.p-profile-mark-content {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    gap: 16px;
}

</style>

