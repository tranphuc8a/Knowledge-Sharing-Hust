

<template>
    <div class="p-profile-mark-lesson-content">
        <div class="p-pmlc-frame">
            <PostSubpage
                :getPost="getPostCallback"
            />
        </div>
    </div>
</template>



<script>
// import { Validator } from '@/js/utils/validator';
import PostSubpage from '@/components/pages/desktop/home/sub-pages/PostSubpage.vue';
import { GetRequest } from '@/js/services/request';

export default {
    name: 'ProfileMarkLessonContent',
    components: {
        PostSubpage,
    },
    props: {
    },
    data(){
        return {
            getPostCallback: null,
        }
    },
    async mounted(){
        this.initContent();
    },
    methods: {
        async initContent(){
            try {
                this.getPostCallback = async function(limit, offset){
                    // preapre request
                    // have: limit, offset

                    // call request
                    let res = await new GetRequest('Marks/my/lessons')
                        .setParams({ limit: limit, offset: offset})
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

.p-profile-mark-lesson-content{
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

.p-pmlc-frame{
    width: 100%;
    max-width: 750px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

