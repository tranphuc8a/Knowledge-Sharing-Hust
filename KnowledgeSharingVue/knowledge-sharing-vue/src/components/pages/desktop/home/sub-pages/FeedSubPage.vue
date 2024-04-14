<template>
    <div class="p-feedsubpage">
        <AddPostFeedCard />
    </div>
</template>

<script>
import AddPostFeedCard from '../components/feed-subpage/AddPostFeedCard.vue';

export default {

    components: {
        AddPostFeedCard
    },
    data(){
        return {
            title: "Desktop Home Page"
        }
    },
    methods: {

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
    flex: column nowrap;
    gap: 16px;
}

</style>