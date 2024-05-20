

<template>
    <div class="p-profile-friend-subpage p-profile-main-subpage">
        <div class="p-profile-friend-card card">
            <div class="p-profile-friend-header">
                <div class="p-pfh-heading">
                    Bạn bè
                </div>
                <div class="p-pfh-search-button">
                    <!-- Textfield Button -->
                    <MTextfieldButton 
                        placeholder="Tìm kiếm bạn bè"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveOnClickSearch" 
                        :validator="null"
                        state="normal"
                        ref="textfield"
                    />
                </div>
            </div>

            <div class="p-profile-friend-navigation">
                <!-- Profile Friend Navigation -->
                <ProfileFriendNavigation />
            </div>

            <div class="p-profile-friend-content">
                <router-view />
            </div>
        </div>
    </div>
</template>



<script>
// import { Validator } from '@/js/utils/validator';
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue';
import ProfileFriendNavigation from '../../components/profile-friend-sp/ProfileFriendNavigation.vue';


export default {
    name: 'ProfileFriendSubpage',
    components: {
        MTextfieldButton,
        ProfileFriendNavigation
    },
    props: {
    },
    data(){
        return {
            onClickSearch: null,
        }
    },
    async mounted(){
    },
    methods: {
        registerOnClickSearch(handler){
            if (handler){
                this.onClickSearch = handler;
            }
        },

        async resolveOnClickSearch(){
            try {
                // get search text:
                let searchText = await this.$refs.textfield.getValue();
                // if (Validator.isEmpty(searchText)){
                //     return;
                // }

                // call handler
                if (this.onClickSearch != null){
                    await this.onClickSearch(searchText);
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return {
            registerOnClickSearch: this.registerOnClickSearch,
        }
    }
}

</script>


<style scoped>

@import url(@/css/pages/desktop/profile/friend-subpage/profile-relation-subpage.css);

</style>

