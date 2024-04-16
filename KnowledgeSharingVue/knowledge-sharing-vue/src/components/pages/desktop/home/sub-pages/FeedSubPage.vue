<template>
    <div class="p-feedsubpage">
        <AddPostFeedCard />
        <component v-for="item in listPosts" 
            :key="item?.UserItemId"
            :is="getComponent(item)"
            v-bind="{post: item}">
            Hello
        </component>
    </div>
</template>

<script>
import AddPostFeedCard from '../components/feed-subpage/postcard/AddPostFeedCard.vue';
import LessonFeedCard from '../components/feed-subpage/postcard/LessonFeedCard.vue';
import QuestionFeedCard from '../components/feed-subpage/postcard/QuestionFeedCard.vue';
import { myEnum } from '@/js/resources/enum';

export default {

    components: {
        AddPostFeedCard, LessonFeedCard, QuestionFeedCard
    },
    data(){
        return {
            title: "Desktop Home Page",
            listPosts: [ null, null ]
        }
    },
    methods: {
        getComponent(item){
            if (item?.PostType ==  myEnum.EPostType.Question){
                return QuestionFeedCard;
            }
            return LessonFeedCard;
        }
    },
    inject: {
        inject: {},
        getToastManager: {},
        getPopupManager: {}
    }
}

</script>


<style scoped>

.p-feedsubpage{
    display: flex;
    flex-flow: column nowrap;
    gap: 16px;
}

</style>