

<template>
    <div class="p-profile-friend-navigation">
        <div class="p-pfnav-left">
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

        <div class="p-pfnav-right">
            <!-- More profile context button -->
            <!-- <ProfilePanelMoreContextButton :items="moreItems"/> -->
        </div>
    </div>
</template>



<script>
import { myEnum } from '@/js/resources/enum';
// import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ProfileFriendNavigation',
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
            eUserRelationType: myEnum.EUserRelationType,
        }
    },
    async created(){
        this.refresh();
        this.initItems();
    },
    async mounted(){
    },
    methods: {
        async initItems(){
            try {
                let username = this.getUser()?.Username;
                let postUrl = '/profile/' + username + '/friend/'; 
                this.listItems = { 
                    [this.eUserRelationType.Friend]: { 
                        key: this.eUserRelationType.Friend, 
                        label: 'Bạn bè', 
                        link: postUrl + 'friend' 
                    },
                    [this.eUserRelationType.Follower]: { 
                        key: this.eUserRelationType.Follower,
                        label: 'Đang theo dõi',
                        link: postUrl + 'follower'
                    },
                    [this.eUserRelationType.Followee]: {
                        key: this.eUserRelationType.Followee,
                        label: 'Người theo dõi',
                        link: postUrl + 'followee'
                    },
                    [this.eUserRelationType.Requestee]: {
                        key: this.eUserRelationType.Requestee,
                        label: 'Lời mời kết bạn',
                        link: postUrl + 'requestee'
                    },
                    [this.eUserRelationType.Requester]: {
                        key: this.eUserRelationType.Requester,
                        label: 'Yêu cầu của bạn',
                        link: postUrl + 'requester'
                    },
                    [this.eUserRelationType.Blockee]: {
                        key: this.eUserRelationType.Blocker,
                        label: 'Đã chặn',
                        link: postUrl + 'blocker'
                    },
                    [this.eUserRelationType.Blocker]: {
                        key: this.eUserRelationType.Blockee,
                        label: 'Bị chặn',
                        link: postUrl + 'blockee'
                    }
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                this.isMySelf = await this.getIsMySelf();
                let listMainItemType = [];
                if (this.isMySelf) {
                    listMainItemType = [
                        this.eUserRelationType.Friend,
                        this.eUserRelationType.Follower,
                        this.eUserRelationType.Followee,
                        this.eUserRelationType.Requestee,
                        this.eUserRelationType.Requester,
                        this.eUserRelationType.Blocker,
                        this.eUserRelationType.Blockee,
                    ];
                } else {
                    listMainItemType = [
                        this.eUserRelationType.Friend,
                        this.eUserRelationType.Follower,
                        this.eUserRelationType.Followee
                    ];
                }
                for (let type of listMainItemType){
                    this.mainItems.push(this.listItems[type]);
                }
            } catch (e){
                console.error(e);
            }
        }
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

.p-profile-friend-navigation{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-pfnav-left{
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

