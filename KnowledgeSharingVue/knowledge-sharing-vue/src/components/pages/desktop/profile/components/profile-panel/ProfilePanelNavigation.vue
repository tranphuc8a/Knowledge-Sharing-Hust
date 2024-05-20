

<template>
    <div class="p-profile-navigation">
        <div class="p-pnav-left">
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

        <div class="p-pnav-right">
            <!-- More profile context button -->
            <ProfilePanelMoreContextButton :items="moreItems"/>
        </div>

    </div>
</template>



<script>
import ProfilePanelMoreContextButton from './ProfilePanelMoreContextButton.vue';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ProfilePanelNavigation',
    components: {
        ProfilePanelMoreContextButton
    },
    props: {
    },
    data(){
        return {
            itemEnum: {},
            items: { 
                Feed: 0,
                Learn: 1,
                Lesson: 2,
                Question: 3,
                Course: 4,
                Save: 5,
                Star: 6,
                Friend: 7,
                Comment: 8,
                Payment: 9,
                Image: 10,
                Message: 11,
                Notification: 12,
                Block: 13,
                Invitation: 14,
                Account: 15
            },
            mainItems: [],
            moreItems: [],
            currentUser: null,
            isMySelf: false,
        }
    },
    async created(){
        await this.createItems();
    },
    mounted(){
        try {
            this.refreshListItem();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async createItems(){
            let username = this.getUser()?.Username;
            try {
                // let index = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
                let name = ["Feed", "Learn", "Lesson", "Question", "Course", "Save", 
                            "Star", "Friend", "Comment", "Payment", "Image", "Message", 
                            "Notification", "Block", "Invitation", "Account"];
                
                let key = ['feed', 'learn', 'lesson', 'question', 'course', 'save', 
                            'star', 'friend', 'comment', 'payment', 'image', 'message', 
                            'notification', 'block', 'invitation', 'account'];
                
                let label = ['Bài viết', 'Học tập', 'Bài học', 'Thảo luận', 'Khóa học', 'Lưu', 
                            'Đánh giá', 'Bạn bè', 'Bình luận', 'Thanh toán', 'Hình ảnh', 'Tin nhắn', 
                            'Thông báo', 'Chặn', 'Lời mời', 'Tài khoản'];
                
                let link = ['feed', 'learn', 'lesson', 'question', 'course', 'save', 
                            'star', 'friend', 'comment', 'payment', 'image', 'message', 
                            'notification', 'block', 'invitation', 'account'];
                
                let totalItems = 16;
                for (let i = 0; i < totalItems; i++){
                    this.itemEnum[name[i]] = name[i];
                    this.items[name[i]] = {
                        key: key[i],
                        label: label[i],
                        link: `/profile/${username}/${link[i]}`
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },

        async refreshListItem(){
            try {
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser != null){
                    this.isMySelf = this.currentUser.UserId == this.getUser()?.UserId;
                }
                if (this.isMySelf){
                    this.mainItems = [
                        this.items.Feed,
                        this.items.Learn,
                        this.items.Lesson,
                        this.items.Question,
                        this.items.Course,
                        this.items.Save,
                        this.items.Friend,
                    ];
                    this.moreItems = [
                        this.items.Star,
                        this.items.Friend,
                        this.items.Comment,
                        this.items.Payment,
                        this.items.Image,
                        this.items.Message,
                        this.items.Notification,
                        this.items.Block,
                        this.items.Account
                    ];
                } else {
                    this.mainItems = [
                        this.items.Feed,
                        this.items.Lesson,
                        this.items.Question,
                        this.items.Course,
                        this.items.Friend
                    ],
                    this.moreItems = [
                        this.items.Feed,
                        this.items.Lesson,
                        this.items.Question,
                        this.items.Course,
                        this.items.Friend
                    ];
                }
            } catch (e) {
                console.error(e);
            }
        },
    },
    inject: {
        getUser: {}
    },
    // watch: {
    //     async '$route.params.username'(){
    //         await this.createItems();
    //         await this.refreshListItem();
    //     }
    // }
}

</script>

<style scoped>

.p-profile-navigation{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-pnav-left{
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

