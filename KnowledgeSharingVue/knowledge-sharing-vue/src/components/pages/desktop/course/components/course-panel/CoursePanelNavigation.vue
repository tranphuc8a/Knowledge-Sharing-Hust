

<template>
    <div class="p-course-panel-navigation">
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
            <!-- More course context button -->
            <CoursePanelMoreContextButton :items="moreItems"/>
        </div>

    </div>
</template>



<script>
import CoursePanelMoreContextButton from './CoursePanelMoreContextButton.vue';
// import CurrentUser from '@/js/models/entities/current-user';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'CoursePanelNavigation',
    components: {
        CoursePanelMoreContextButton
    },
    props: {
    },
    data(){
        return {
            itemEnum: {},
            items: { 
                Introduction: 0,
                Lesson: 1,
                Question: 2,
                Register: 3,
                Revite: 4, // Request and Invite
                Payment: 5,
                Edit: 6, // Edit Course Information
                Update: 7, // Update Course Lessons
            },
            mainItems: [],
            moreItems: [],
            currentUser: null,
            isMyCourse: false,
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
            let courseId = this.getCourse()?.UserItemId;
            try {
                // let index = [0, 1, 2, 3, 4, 5, 6, 7 ];
                let name = ["Introduction", "Lesson", "Question", 
                            "Register", "Revite", "Payment", 
                            "Edit", "Update"];
                
                let key = ['introduction', 'lesson', 'question', 
                            'register', 'revite', 'payment',
                            'edit', 'update'];
                
                let label = ['Giới thiệu', 'Bài học', 'Thảo luận', 
                            'Thành viên', 'Yêu cầu - Lời mời', 'Thống kê',
                            'Chỉnh sửa', 'Cập nhật bài học'];
                
                let link = ['introduction', 'lesson', 'question', 
                            'register', 'revite', 'payment',
                            'edit', 'lesson-edit'];
                
                let totalItems = name.length;
                for (let i = 0; i < totalItems; i++){
                    this.itemEnum[name[i]] = name[i];
                    this.items[name[i]] = {
                        key: key[i],
                        label: label[i],
                        link: `/course/${courseId}/${link[i]}`
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },

        async refreshListItem(){
            try {
                this.isMyCourse = await this.getIsMyCourse();
                if (this.isMyCourse){
                    this.mainItems = [
                        this.items.Introduction,
                        this.items.Lesson,
                        this.items.Question,
                        this.items.Register,
                        this.items.Revite,
                        this.items.Payment
                    ];
                    this.moreItems = [
                        this.items.Introduction,
                        this.items.Lesson,
                        this.items.Question,
                        this.items.Register,
                        this.items.Revite,
                        this.items.Payment,
                        this.items.Edit,
                        this.items.Update,
                    ];
                } else {
                    let courseRoleType = this.getCourse()?.CourseRoleType;
                    if (courseRoleType == myEnum.ECourseRoleType.Member){
                        this.mainItems = [
                            this.items.Introduction,
                            this.items.Lesson,
                            this.items.Question,
                            this.items.Register,
                        ];
                        this.moreItems = [
                            this.items.Introduction,
                            this.items.Lesson,
                            this.items.Question,
                            this.items.Register,
                        ];
                    } else {
                        this.mainItems = [
                            this.items.Introduction
                        ];
                        this.moreItems = [
                            this.items.Introduction
                        ];
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },
    },
    inject: {
        getCourse: {},
        getIsMyCourse: {},
    },
    // watch: {
    //     async '$route.params.courseId'(){
    //         await this.createItems();
    //         await this.refreshListItem();
    //     }
    // }
}

</script>

<style scoped>

.p-course-panel-navigation{
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

