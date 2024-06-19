

<template>
    <div class="p-search-post-by-text-content">
        <PostSubpage
            :getPost="getPostCallback"
            :isViewComment="false"
        />
    </div>
</template>



<script>
import { Validator } from '@/js/utils/validator';
import PostSubpage from '../PostSubpage.vue';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'SearchLessonByTextContent',
    components: {
        PostSubpage,
    },
    props: {
    },
    data(){
        return {
            getPostCallback: null,
            searchKey: '',
        }
    },
    async mounted(){
        this.resolveNewSearchKey();
    },
    watch: {
        '$route.query.search': {
            handler: function () {
                this.resolveNewSearchKey();
            },
            immediate: true
        }
    },
    methods: {
        async resolveNewSearchKey(){
            try {
                this.searchKey = this.$route.query.search;
                this.getPostCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset, searchKey
                    if (Validator.isEmpty(this.searchKey)) return {
                        Body: [],
                    }

                    // call request
                    let res = await new GetRequest('Lessons/search')
                        .setParams({search: this.searchKey, limit: limit, offset: offset})
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
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

.p-search-post-by-text-content{
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

