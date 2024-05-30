

<template>
    <div class="p-sarch-by-category-subpage">
        <div class="p-sbcs-topcard">
            <SearchByCategoryTopCard />
        </div>
        <div class="p-sbcs-content">
            <router-view />
        </div>

        <div style="width: 100%; height: 2000px; background: red;"> </div>
    </div>
</template>



<script>
import SearchByCategoryTopCard from './SearchByCategoryTopCard.vue';

export default {
    name: 'SearchByCategorySubpage',
    components: {
        SearchByCategoryTopCard,
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

.p-sarch-by-category-subpage{
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

.p-sbcs-topcard{
    width: 100%;
    max-width: 100%;
    position: sticky;
    top: 0;
}

</style>

