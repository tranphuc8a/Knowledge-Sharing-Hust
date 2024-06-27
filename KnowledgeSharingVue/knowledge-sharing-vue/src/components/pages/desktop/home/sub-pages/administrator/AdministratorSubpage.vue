

<template>
    <div class="p-administrator-subpage" v-if="isAdmin">
        <div class="p-as-topcard">
            <AdministratorTopCard />
        </div>
        <div class="p-as-content">
            <router-view />
        </div>
    </div>
    <div class="p-administrator-subpage" v-if="!isAdmin">
        <div class="p-as-not-right">
            <NotFoundPanel :text="isNotAdminText" />
        </div>
    </div>
</template>



<script>
import { myEnum } from '@/js/resources/enum';
import AdministratorTopCard from './AdministratorTopCard.vue';
import CurrentUser from '@/js/models/entities/current-user';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';

export default {
    name: 'AdministratorSubpage',
    components: {
        AdministratorTopCard,
        NotFoundPanel,
    },
    created(){
        this.updateIsAdmin();
    },
    props: {
    },
    data(){
        return {
            isAdmin: true,
            isNotAdminText: 'Bạn không có quyền truy cập trang này.',
        }
    },
    methods: {
        async updateIsAdmin(){
            try {
                let isAdmin = false;
                let currentUser = await CurrentUser.getInstance();
                if (currentUser){
                    isAdmin = (currentUser.Role == myEnum.EUserRole.Admin);
                }
                this.isAdmin = isAdmin;
            } catch (error) {
                console.error(error);
            }
        }
    },
}

</script>

<style scoped>

.p-administrator-subpage{
    max-width: 100%;
    width: 100%;
    padding-bottom: 32px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    position: relative;
    gap: 16px;
}

.p-as-not-right{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    gap: 16px;
    position: relative;
    height: 300px;
}

.p-as-topcard{
    width: 100%;
    max-width: 100%;
    position: sticky;
    z-index: 1;
    top: 0;
}

.p-as-content{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    position: relative;
}

</style>

