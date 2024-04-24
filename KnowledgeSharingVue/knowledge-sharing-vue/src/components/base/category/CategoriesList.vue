<template>
    <div class="category-list">
        <div v-for="(category, index) in categories" :key="index" 
            class="category" @:click="resolveClickCategory(category)">
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

            async resolveClickCategory(category){
                console.log("Clicked category: " + category);
            }
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
    background-color: var(--primary-color-100);
    padding: 4px 8px;
    border-radius: 12px;
    height: fit-content;
    font-size: 12px;
    color: var(--primary-color);
    cursor: pointer;
    max-width: 100px;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    /* text-overrlow only active with these four attributes */
}

.category:hover {
    background-color: var(--primary-color-200);
}

.category:active {
    background-color: var(--primary-color-300);
    color: var(--primary-color-600);
}
</style>
