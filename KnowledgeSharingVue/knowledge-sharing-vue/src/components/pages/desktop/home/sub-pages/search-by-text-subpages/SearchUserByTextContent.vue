

<template>
    <div class="p-search-user-by-text-content">
        <div class="p-subt-result-card card">
            <div class="p-subt-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-subt-result">

            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import ProfileFriendCard from '../../../profile/components/profile-friend-sp/ProfileFriendCard.vue';
import ProfileFriendCardSkeleton from '../../../profile/components/profile-friend-sp/ProfileFriendCardSkeleton.vue';
import { useRoute } from 'vue-router';


export default {
    name: 'SearchUserByTextContent',
    components: {
        NotFoundPanel,
        ProfileFriendCard,
        ProfileFriendCardSkeleton
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            listUser: [],
            searchKey: '',
        }
    },
    async created(){
        this.createdPage();
    },
    async mounted(){
    },
    methods: {
        async createdPage(){
            try {
                this.searchKey = this.route.query.seacrh;
                this.listUser = await this.$store.dispatch('searchUserByText', this.searchKey);
            } catch (e){
                console.error(e);
            }
        }
    },
    watch: {
        '$route.query.search': {
            handler(){
                this.createdPage();
            },
            deep: true,
        }
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-search-user-by-text-content{
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

</style>

