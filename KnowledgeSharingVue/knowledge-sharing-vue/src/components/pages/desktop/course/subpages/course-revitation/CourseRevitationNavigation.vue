

<template>
    <div class="p-course-revitation-navigation">
        <div class="p-nav-left">
            <div class="p-nav-item"
                v-for="(item, index) in mainItems"
                :key="item?.key ?? index"
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
import { myEnum } from '@/js/resources/enum';
// import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'CourseRevitationNavigation',
    components: {
    },
    props: {
    },
    data(){
        return {
            // isMySelf: false,
            listItems: [],
            mainItems: [],
            moreItems: [],
            eCourseRoleType: myEnum.ECourseRoleType,
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
                let courseId = this.getCourse()?.UserItemId;
                let postUrl = '/course/' + courseId + '/revite/'; 
                this.listItems = { 
                    [this.eCourseRoleType.Requesting]: { 
                        key: this.eCourseRoleType.Requesting, 
                        label: 'Yêu cầu tham gia', 
                        link: postUrl + 'request' 
                    },
                    [this.eCourseRoleType.Invited]: { 
                        key: this.eCourseRoleType.Invited,
                        label: 'Lời mời',
                        link: postUrl + 'invite'
                    },
                    [this.eCourseRoleType.Member]: { 
                        key: this.eCourseRoleType.Member,
                        label: 'Thành viên',
                        link: postUrl + 'member'
                    }
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                let listMainItemType = [
                    this.eCourseRoleType.Requesting, 
                    this.eCourseRoleType.Invited, 
                    this.eCourseRoleType.Member
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
        getCourse: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-revitation-navigation{
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

