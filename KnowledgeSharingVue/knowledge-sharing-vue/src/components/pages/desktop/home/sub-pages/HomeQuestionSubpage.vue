

<template>
    <div class="p-home-question-subpage-frame">
        <PostSubpage 
            :get-post="getQuestion"
            :is-show-add-post="true"
        />
    </div>
</template>



<script>
import PostSubpage from './PostSubpage.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'HomeQuestionSubpage',
    components: {
        PostSubpage
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

