

<template>
    <div class="p-profile-subpage" 
        @:scroll="resolveOnScroll"
        ref="list"
        >

    </div>
</template>



<script>


export default {
    name: 'ProfileHomeFeedSubpage',
    props: {
    },
    data(){
        return {
            isOutOfPost: false,
            isWaiting: false,
        }
    },
    mounted(){

    },
    methods: {
        async resolveOnScroll(){
            try {
                if (this.isOutOfPost || this.isWaiting){
                    return;
                }
                let listComponent = this.$refs.list;

                let scrollHeight = listComponent.scrollHeight;
                let scrollTop = listComponent.scrollTop;
                let clientHeight = listComponent.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                let averagePostHeight = 500;

                if (scrollHeight - scrollPosition < averagePostHeight * 5){
                    await this.loadMorePost();
                }
            } catch (e){
                console.error(e);
            }
        },

        async loadMorePost(){
            try {
                if (̣this.isOutOfPost || this.isWaiting){
                    return;
                }
                this.isWaiting = true;
                // get more post
            } catch (e){
                console.error(e);
            } finally {
                this.isWaiting = false;
            }
        },

        async refresh(){
            try {
                
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
        getIsMySelf: {},

    }
}

</script>

<style scoped>

</style>

