

<template>
    <div class="p-administrator-one-user-content">
        <div class="p-aouc-result-card card">
            <div class="p-aouc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-aouc-result">
                <div class="p-aouc-result-list" v-if="!isLoaded">
                    <UserCardSkeleton />
                </div>
                <div class="p-aouc-result-list" v-if="isLoaded && isUserExisted">
                    <UserAdministratorCard :user="user" />
                </div>
                <div class="p-aouc-result-notfound" v-if="isLoaded && !isUserExisted" >
                    <NotFoundPanel 
                        :text="errorText" 
                    />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import UserAdministratorCard from '../../components/administrator/admin-user/UserAdministratorCard.vue';
import UserCardSkeleton from '../../components/administrator/admin-user/UserCardSkeleton.vue';
import { useRoute } from 'vue-router';
import { GetRequest, Request } from '@/js/services/request';
import ViewUserProfile from '@/js/models/views/view-user-profile';


export default {
    name: 'AdministratorOneUserContent',
    components: {
        NotFoundPanel,
        UserAdministratorCard,
        UserCardSkeleton,
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            user: null,
            filter: '',
            isLoaded: false,
            isUserExisted: false,
            errorText: 'Không tìm thấy người dùng',
        }
    },
    async created(){
    },
    async mounted(){
        this.createdPage();
    },
    methods: {

        async createdPage(){
            try {
                this.isLoaded = false;
                this.filter = this.route.query['filter'];
                
                let url = 'Users/admin/user-profile/' + this.filter;
                let res = await new GetRequest(url).execute();
                let body = await Request.tryGetBody(res);
                this.user = new ViewUserProfile().copy(body);
                this.isUserExisted = true;

            } catch (e){
                let userMsg = await Request.tryGetUserMessage(e);
                if (userMsg != null){
                    this.errorText = userMsg;
                } else {
                    console.error(e);
                }
                this.isUserExisted = false;
            } finally {
                this.isLoaded = true;
            }
        },

        async resolveOnDeletedUser(){
            this.createdPage();
        }
    },
    watch: {
        '$route.query.filter': {
            handler(){
                this.createdPage();
            },
            deep: true,
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getCurrentUser: {},
    },
    provide(){
        return {
            onUserDeleted: this.resolveOnDeletedUser,
        }
    }
}

</script>


<style scoped>

.p-administrator-one-user-content{
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

.p-aouc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aouc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-aouc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aouc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-aouc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

