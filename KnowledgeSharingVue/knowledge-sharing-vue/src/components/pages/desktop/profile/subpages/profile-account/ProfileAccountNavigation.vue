

<template>
    <div class="p-profile-account-navigation">
        <div class="p-panav-left">
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

        <div class="p-panav-right">
            <!-- More profile context button -->
            <!-- <ProfilePanelMoreContextButton :items="moreItems"/> -->
        </div>
    </div>
</template>



<script>

export default {
    name: 'ProfileAccountNavigation',
    components: {
    },
    props: {
    },
    data(){
        return {
            listItems: [],
            mainItems: [],
            moreItems: [],
            keys: {
                ChangePassword: 0,
                Logout: 1,
            }
        }
    },
    async created(){
        await this.initItems();
        await this.refresh();
    },
    async mounted(){
    },
    methods: {
        async initItems(){
            try {
                let username = this.getUser()?.Username;
                let postUrl = '/profile/' + username + '/account/'; 
                this.listItems = { 
                    [this.keys.ChangePassword]: { 
                        key: this.keys.ChangePassword, 
                        label: 'Đổi mật khẩu', 
                        link: postUrl + 'change-password' 
                    },
                    [this.keys.Logout]: { 
                        key: this.keys.Logout,
                        label: 'Đăng xuất',
                        link: postUrl + 'logout'
                    },
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                let listMainItemType = [
                    this.keys.ChangePassword,
                    this.keys.Logout,
                ];
                for (let type of listMainItemType){
                    this.mainItems.push(this.listItems[type]);
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-profile-account-navigation{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-panav-left{
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

