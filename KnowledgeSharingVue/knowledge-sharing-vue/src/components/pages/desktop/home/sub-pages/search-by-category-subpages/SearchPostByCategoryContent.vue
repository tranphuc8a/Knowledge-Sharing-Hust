

<template>
    <div class="p-search-post-by-category-content">
        <PostSubpage
            :getPost="getPostCallback"
        />
    </div>
</template>



<script>
import { Validator } from '@/js/utils/validator';
import PostSubpage from '../PostSubpage.vue';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'SearchPostByCategoryContent',
    components: {
        PostSubpage,
    },
    props: {
    },
    data(){
        return {
            getPostCallback: null,
            category: '',
        }
    },
    async mounted(){
        this.resolveNewCategory();
    },
    watch: {
        '$route.query.category': {
            handler: function () {
                this.resolveNewCategory();
            },
            immediate: true
        }
    },
    methods: {
        async resolveNewCategory(){
            try {
                this.category = this.$route.query.category;
                this.getPostCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset, searchKey
                    if (Validator.isEmpty(this.category)) return {
                        Body: [],
                    }
                    let url = 'Categories/posts/' + this.category;
                    let currentUser = await this.getCurrentUser();
                    if (currentUser == null){
                        url = 'Categories/anonymous/posts/' + this.category;
                    }

                    // call request
                    let res = await new GetRequest(url)
                        .setParams({ limit: limit, offset: offset })
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getCurrentUser: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-search-post-by-category-content{
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

