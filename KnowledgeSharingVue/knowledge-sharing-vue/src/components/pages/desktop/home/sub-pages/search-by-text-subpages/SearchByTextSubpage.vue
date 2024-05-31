

<template>
    <div class="p-sarch-by-text-subpage">
        <div class="p-sbts-topcard">
            <SearchByTextTopCard />
        </div>
        
        <div class="p-sbts-content">
            <router-view />
        </div>

    </div>
</template>



<script>
import SearchByTextTopCard from './SearchByTextTopCard.vue';

export default {
    name: 'SearchByTextSubpage',
    components: {
        SearchByTextTopCard,
    },
    props: {
    },
    data(){
        return {
        }
    },
    methods: {
    },
}

</script>

<style scoped>

.p-sarch-by-text-subpage{
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

.p-sbts-topcard{
    width: 100%;
    max-width: 100%;
    position: sticky;
    z-index: 1;
    top: 0;
}

.p-sbts-content{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

