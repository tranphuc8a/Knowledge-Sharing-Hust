

<template>
    <div class="p-profile-mark-navigation">
        <div class="p-nav-left">
            <div class="p-nav-item"
                v-for="(item, index) in mainItems"
                :key="item.key ?? index"
            >
                <router-link :to="item.link" class="p-nav-item-button">
                    <div class="p-nav-item-text">
                        {{ item.label }} 
                    </div>
                </router-link>
            </div>
        </div>

        <div class="p-nav-right">
            <!-- More profile context button -->
            <!-- <ProfilePanelMoreContextButton :items="moreItems"/> -->
        </div>
    </div>
</template>



<script>
// import { myEnum } from '@/js/resources/enum';
// import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ProfileMarkNavigation',
    components: {
    },
    props: {
    },
    data(){
        return {
            isMySelf: false,
            listItems: [],
            mainItems: [],
            moreItems: [],
        }
    },
    async created(){
        await this.initItems();
    },
    async mounted(){
    },
    methods: {
        async initItems(){
            try {
                let username = this.getUser()?.Username ?? this.getUser()?.UserId;
                let postUrl = '/profile/' + username + '/save/'; 
                this.mainItems = [
                    {
                        key: 'post',
                        label: 'Bài viết đã lưu',
                        link: postUrl + 'post',
                    },
                    {
                        key: 'lesson',
                        label: 'Bài giảng đã lưu',
                        link: postUrl + 'lesson',
                    },
                    {
                        key: 'course',
                        label: 'Khóa học đã lưu',
                        link: postUrl + 'course',
                    },
                    {
                        key: 'question',
                        label: 'Bài thảo luận đã lưu',
                        link: postUrl + 'question',
                    },
                ]
            } catch (e){
                console.error(e);
            }
        },
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-mark-navigation{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-nav-left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 2px;
}

.p-nav-item{
    border-bottom: solid transparent 3px;
    padding-bottom: 1px;
}

.p-nav-item:has(.router-link-active){
    border-bottom: solid var(--primary-color-500) 3px;
}

.p-nav-item:has(.router-link-active) .p-nav-item-button{
    color: var(--primary-color);
}

.p-nav-item-button{
    text-decoration: none;
    font-family: 'ks-font-semibold';
    color: var(--grey-color-600);
    cursor: pointer;
}

.p-nav-item-text{
    padding: 16px;
    border-radius: 4px;
    height: 52px;
    display: flex;
    flex-flow: row nowrap;
    justify-self: center;
    align-items: center;
}

.p-nav-item-text:hover{
    background-color: var(--grey-color-200);
}

</style>

