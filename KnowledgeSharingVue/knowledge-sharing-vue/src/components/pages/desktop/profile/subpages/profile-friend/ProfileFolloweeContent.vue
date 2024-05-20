

<template>
    <div class="p-profile-relation-content">
        <ProfileRelationContent 
            :user-relation-type="userRelationType"
            :get-more-user="getMoreUserCallback"
        />
    </div>
</template>



<script>
import ProfileRelationContent from './ProfileRelationContent.vue';
import { myEnum } from '@/js/resources/enum';
import { Validator } from '@/js/utils/validator';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'ProfileFolloweeContent',
    components: {
        ProfileRelationContent
    },
    props: {
    },
    data(){
        return {
            userRelationType: null,
            getMoreUserCallback: null,
            isMySelf: null,
        }
    },
    async created(){
        this.refresh();
    },
    async mounted(){
    },
    // async updated(){
    //     this.refresh();
    // },
    methods: {
        async resolveOnClickSearch(searchText){
            try {
                // change callback:
                if (Validator.isNotEmpty(searchText)){
                    // search friends:
                    // prepare Url
                    let url = "UserRelations/my/followees/search";
                    this.isMySelf = await this.getIsMySelf();
                    if (!this.isMySelf) {
                        let userId = this.getUser()?.UserId;
                        url = "UserRelations/followees/search/" + userId;
                    }

                    this.getMoreUserCallback = this.getCallbackWithUrl(url, {search: searchText});
                } else {
                    // get all friends
                    // prepare Url
                    let url = "UserRelations/my/followees/";
                    this.isMySelf = await this.getIsMySelf();
                    if (!this.isMySelf) {
                        let userId = this.getUser()?.UserId;
                        url = "UserRelations/followees/" + userId;
                    }

                    this.getMoreUserCallback = this.getCallbackWithUrl(url);
                }
            } catch (e) {
                console.error(e);
            }
        },

        async refresh(){
            try {
                if (this.registerOnClickSearch != null){
                    this.registerOnClickSearch(this.resolveOnClickSearch.bind(this));
                }
                this.userRelationType = myEnum.EUserRelationType.Friend;
                
                // prepare Url
                let url = "UserRelations/my/followees/";
                this.isMySelf = await this.getIsMySelf();
                if (!this.isMySelf) {
                    let userId = this.getUser()?.UserId;
                    url = "UserRelations/followees/" + userId;
                }

                // init callback
                this.getMoreUserCallback = this.getCallbackWithUrl(url);
            } catch (e) {
                console.error(e);
            }
        },

        getCallbackWithUrl(url, params = {}){
            return async function(limit, offset){
                let res = await new GetRequest(url).setParams({
                        ...params,
                        limit: limit,
                        offset: offset
                    }).execute();
                return res;
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
        registerOnClickSearch: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-relation-content {
    width: 100%;
}

</style>

