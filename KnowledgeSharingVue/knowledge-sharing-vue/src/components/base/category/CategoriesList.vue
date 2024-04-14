<template>
    <div class="category-list">
        <div v-for="(category, index) in categories" :key="index" class="category" @mouseover="highlightCategory(index)" @click="handleCategoryClick(category)">
            {{ category }}
        </div>
    </div>
</template>

<script>
    export default {
        name: 'CategoriesList',
        props: {
            categories: {
                type: Array,
                required: true,
            },
        },
        methods: {
            highlightCategory(index) {
                // Add your highlight logic here
                console.log("Hovered over category:", this.categories[index]);
            },
            handleCategoryClick(category) {
                // Add your click event handling logic here
                console.log("Clicked on category:", category);
            },
        },
    };
</script>

<style scoped>
    .category-list {
        display: flex;
        flex-wrap: wrap;
    }

    .category {
        background-color: #f0f0f0;
        padding: 5px 10px;
        margin: 5px;
        border-radius: 5px;
        cursor: pointer;
    }

    .category:hover {
        background-color: #e0e0e0;
    }
</style>
