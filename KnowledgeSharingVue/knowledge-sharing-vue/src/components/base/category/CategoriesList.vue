<template>
    <div class="category-list">
        <CategoryItem v-for="(category, index) in categories" :key="index" 
            :category="category" :is-editing="false"
            
        />
    </div>
</template>

<script>
import CategoryItem from './CategoryItem.vue';
export default {
    name: 'CategoriesList',
    components: {
        CategoryItem,
    },
    props: {
        categories: {
            type: Array,
            required: true,
        },
    },
    methods: {
        
    },
};
</script>

<style scoped>
.category-list {
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
    width: fit-content;
    height: fit-content;
}

</style>
