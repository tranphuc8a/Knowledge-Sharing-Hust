

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
import CurrentUser from '@/js/models/entities/current-user';
import HomeNavigationItem from '../components/home-navigation/HomeNavigationItem.vue';
import HomeNavigationUser from '../components/home-navigation/HomeNavigationUser.vue';

export default {
    name: 'HomeNavigationSubpage',
    data() {
        return {
            items: [],
            currentUser: null,
        }
    },
    methods: {
        goBack() {
            this.$router.go(-1)
        },
        
        async initItems(){
            let currentUser = await CurrentUser.getInstance();
            let username = currentUser?.Username;
            let postfix = '/profile/' + String(username);
            let fa = ['home', 'book-reader', 'layer-group', 'book-open', 'comments', 'user-friends', 'bookmark'];
            let label = ['Trang chủ', 'Đang học', 'Khóa học của tôi', 'Bài giảng của tôi', 'Bài thảo luận của tôi', 'Bạn bè', 'Đã lưu'];
            let destination = ['/', postfix + '/learn', postfix + '/course', 
                                postfix + '/lesson', postfix + '/question', 
                                postfix + '/friend', postfix + '/save'];
            this.items = [];
            for (let i = 0; i < fa.length; i++) {
                this.items.push({
                    fa: fa[i],
                    label: label[i],
                    destination: destination[i]
                });
            }
        }
    },
    mounted() {
        this.initItems();
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