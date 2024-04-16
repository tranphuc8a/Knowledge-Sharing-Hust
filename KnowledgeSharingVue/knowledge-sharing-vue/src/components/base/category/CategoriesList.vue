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
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
    width: fit-content;
    height: fit-content;
}

.category {
    background-color: var(--grey-color-200);
    padding: 4px 8px;
    border-radius: 12px;
    height: fit-content;
    font-size: 12px;
    color: var(--grey-color-700);
    cursor: pointer;
    max-width: 100px;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    /* text-overrlow only active with these four attributes */
}

.category:hover {
    background-color: var(--grey-color-300)
}

.category:active {
    background-color: var(--grey-color-400)
}
</style>
