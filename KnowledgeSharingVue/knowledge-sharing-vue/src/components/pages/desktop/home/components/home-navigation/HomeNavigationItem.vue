

<template>
    <div class="d-navigation-item" @:click="resolveClickItem">
        <div class="d-item-icon">
            <MIcon :fa="fa" :family="family" :style="iconStyle"/>
        </div>
        <div class="d-item-label">
            {{ label }}
        </div>
    </div>
</template>



<script>

import { useRouter } from 'vue-router';

export default {
    name: 'HomeNavigationItem',
    props: {
        fa: {
            default: 'home'
        },
        family: {
            default: 'fas'
        },
        label: {
            default: 'Home'
        },
        destination: {
            default: '/'
        }
    },
    data(){
        return {
            title: '',
            isLessonExisted: false,
            lesson: null,
            router: useRouter(),
            iconStyle: {
                width: '24px',
                height: '24px'
            }
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickItem(){
            try {
                this.router.push(this.destination);
            } catch (e) {
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>

.d-navigation-item{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 12px;
    cursor: pointer;
    padding: 8px;
    border-radius: 4px;
}

.d-navigation-item:hover{
    background-color: var(--primary-color-100);
}

.d-navigation-item:active{
    background-color: var(--primary-color-200);
}

.d-item-label{
    font-family: 'ks-font-semibold';
    font-size: 15px;
    color: var(--grey-color-600);

    width: 100%;
    align-self: stretch;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
}

</style>

