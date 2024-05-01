<template>
    <div class="category-frame">
        <div class="category" @:click="resolveClickCategory">
            {{ category }}
        </div>
        <MIcon fa="times" class="category-icon" v-show="isEditing"
            :style="iconStyle" @:click.prevent="resolveClickClose"/>
    </div>
</template>

<script>

import { useRouter } from 'vue-router';

export default {
    name: 'CategoryItem',
    props: {
        category: {
            type: String,
            required: true,
        },
        isEditing: {
            type: Boolean,
            default: false
        },
        onDeleteCategory: {}
    },
    data(){
        return {
            iconStyle: {
                fontSize: '16px',
                color: 'var(--primary-color)',
                cursor: 'pointer'
            },
            router: useRouter()
        }
    },
    methods: {

        async resolveClickCategory(){
            if (this.isEditing) return;
            console.log("Clicked category: " + this.category);
        },

        async resolveClickClose(){
            try {
                if (this.onDeleteCategory) {
                    this.onDeleteCategory(this.category);
                }
            } catch (e) {
                console.error(e);
            }
        }
    },
}

</script>

<style scoped>

.category-frame{
    display: flex;
    width: fit-content;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 4px;

    background-color: var(--primary-color-100);
    padding: 4px 8px;
    border-radius: 12px;
    height: fit-content;
}

.category-frame:hover {
    background-color: var(--primary-color-200);
}

.category-frame:active {
    background-color: var(--primary-color-300);
    color: var(--primary-color-600);
}

.category {
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



.category-icon{
    flex-shrink: 0;
    flex-grow: 0;
}

</style>