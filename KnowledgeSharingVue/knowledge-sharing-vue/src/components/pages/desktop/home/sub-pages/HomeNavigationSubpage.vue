

<template>
    <div class="d-subpage">
        <HomeNavigationUser />
        <div class="p-devide"></div>
        <div v-for="item in items" :key="item.label">
            <HomeNavigationItem 
                :fa="item.fa" 
                :label="item.label" 
                :destination="item.destination"
                :family="'fas'" />
        </div>
    </div>

</template>



<script>

import HomeNavigationItem from '../components/home-navigation/HomeNavigationItem.vue';
import HomeNavigationUser from '../components/home-navigation/HomeNavigationUser.vue';

export default {
    name: 'HomeNavigationSubpage',
    data() {
        return {
            items: [
                {
                    fa: 'home',
                    label: 'Trang chủ',
                    destination: '/'
                },
                {
                    fa: 'book-reader',
                    label: 'Đang học',
                    destination: '/learning'
                },
                {
                    fa: 'layer-group',
                    label: 'Khóa học của tôi',
                    destination: '/my-courses'
                },
                {
                    fa: 'book-open',
                    label: 'Bài giảng của tôi',
                    destination: '/my-lessons'
                },
                {
                    fa: 'comments',
                    label: 'Bài thảo luận của tôi',
                    destination: '/my-questions'
                },
                {
                    fa: 'user-friends',
                    label: 'Bạn bè',
                    destination: '/friends'
                },
                {
                    fa: 'bookmark',
                    label: 'Đã lưu',
                    destination: '/bookmarks'
                }
            ]
        }
    },
    methods: {
        goBack() {
            this.$router.go(-1)
        }
    },
    mounted() {
    },
    components: {
        HomeNavigationItem, HomeNavigationUser
    },
    props: {
    }
}

</script>


<style scoped>

.d-subpage{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 8px;
    width: 100%;
}

.d-subpage > div{
    width: 100%;
}

</style>