

<template>
    <div class="p-home-question-subpage-frame">
        <PostSubpage 
            :get-post="getQuestion"
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
    name: 'HomeQuestionSubpage',
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
        async getQuestion(limit, offset){
            let currentUser = await CurrentUser.getInstance();
            let url = "Questions";
            if (currentUser == null){
                url = "Questions/anonymous";
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

.p-home-question-subpage-frame{
    width: 100%;
    padding-bottom: 32px;
}

</style>

