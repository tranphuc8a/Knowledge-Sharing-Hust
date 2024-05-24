

<template>
    <div class="p-home-lesson-subpage">
        <PostSubpage 
            :get-post="getLesson"
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
    name: 'HomeLessonSubpage',
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
        async getLesson(limit, offset){
            let currentUser = await CurrentUser.getInstance();
            let url = "Lessons";
            if (currentUser == null){
                url = "Lessons/anonymous";
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

.p-home-lesson-subpage-frame{
    width: 100%;
    padding-bottom: 32px;
}

</style>

