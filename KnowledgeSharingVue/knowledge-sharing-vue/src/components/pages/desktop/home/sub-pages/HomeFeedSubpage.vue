

<template>
    <div class="p-home-feed-subpage">
        <PostSubpage 
                :get-post="getPost"
            >
                <template #addpost>
                    <AddPostFeedCard />
                </template>
        </PostSubpage>
    </div>
</template>



<script>
import PostSubpage from './PostSubpage.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest } from '@/js/services/request';
import AddPostFeedCard from '../components/feed-subpage/postcard/AddPostFeedCard.vue';

export default {
    name: 'HomeFeedSubpage',
    components: {
        PostSubpage,
        AddPostFeedCard
    },
    props: {
    },
    data(){
        return {
        }
    },
    methods: {
        async getPost(limit, offset){
            let currentUser = await CurrentUser.getInstance();
            let url = "Posts";
            if (currentUser == null){
                url = "Posts/anonymous";
            }
            let res = new GetRequest(url)
                .setParams({
                    limit: limit,
                    offset: offset
                })
                .execute();
            return res;
        }
    },
}

</script>

<style scoped>

.p-home-feed-subpage{
    width: 100%;
    padding-bottom: 32px;
}


</style>

