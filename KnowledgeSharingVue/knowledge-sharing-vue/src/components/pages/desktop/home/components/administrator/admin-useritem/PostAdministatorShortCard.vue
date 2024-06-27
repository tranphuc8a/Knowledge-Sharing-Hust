

<template>

    <PostShortCard
        :post="dPost"
    >
        <template #postMenuContext>
            <PostAdministatorMenuContext />
        </template>
    </PostShortCard>
    
</template>



<script>

import PostShortCard from '@/components/base/cards/PostShortCard.vue';
import PostAdministatorMenuContext from './PostAdministatorMenuContext.vue';

export default {
    name: 'PostAdministatorShortCard',
    components: {
        PostShortCard,
        PostAdministatorMenuContext,
    },
    props: {
        post: {
            type: Object,
            required: true,
        }
    },
    watch: {
        post: {
            handler(){
                this.refresh();
            },
            deep: true,
        }
    },
    data(){
        return {
            dPost: this.post
        }
    },
    async mounted(){
        this.dPost = this.post;
    },
    methods: {
        async refresh(){
            try {
                this.dPost = this.post;
            } catch (e){
                console.error(e);
            }
        
        }
    },
    inject: {
    },
    provide(){
        return {
            getPost: () => this.dPost,
        }
    }
}

</script>

