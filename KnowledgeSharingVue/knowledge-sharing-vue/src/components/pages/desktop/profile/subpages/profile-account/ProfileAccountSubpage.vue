

<template>
    <div class="p-profile-account-subpage p-profile-main-subpage">
        <div class="p-profile-account-card card">
            <div class="p-profile-account-header">
                <div class="p-pah-heading">
                    Quản lý tài khoản
                </div>
                <div class="p-pah-search-button">
                    
                </div>
            </div>

            <div class="p-profile-account-navigation">
                <!-- Profile Account Navigation -->
                <ProfileAccountNavigation />
            </div>

            <div class="p-profile-account-content">
                <router-view />
            </div>
        </div>
    </div>
</template>



<script>
import ProfileAccountNavigation from './ProfileAccountNavigation.vue';

export default {
    name: 'ProfileFriendSubpage',
    components: {
        ProfileAccountNavigation,
    },
    props: {
    },
    data(){
        return {
        }
    },
    async mounted(){
    },
    methods: {
        
    },
    inject: {
    },
    provide(){
        return {
        }
    }
}

</script>


<style scoped>



.p-profile-account-subpage{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    padding-bottom: 32px;
}


.p-profile-account-subpage .p-profile-account-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    padding-bottom: 32px;
}

.p-profile-account-subpage .p-profile-account-card > * {
    width: 100%;
}


.p-profile-account-subpage .p-profile-account-header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-profile-account-subpage .p-pah-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

</style>

