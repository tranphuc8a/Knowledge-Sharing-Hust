

<template>
    <div class="p-course-panel-member" v-if="!isLoaded">
        <div class="p-number-member">
            <div class="skeleton"
                style="width: 100px; height: 24px;"
            ></div>
        </div>
        <div class="p-list-members">
            <div class="skeleton"
                style="width: 150px; height: 24px;"
            ></div>
        </div>
    </div>

    
    <div class="p-course-panel-member" v-if="isLoaded">
        <div class="p-number-member">
            <span>{{ numberMembers }} thành viên </span>
        </div>
        <div class="p-list-members">
            <div class="p-list-members__item" 
                v-for="(member, index) in listMembers.slice().reverse()"
                :key="member.UserId ?? index">
                <div class="p-list-members__item_container">
                    <TooltipUserAvatar :user="member" :size="userAvatarSize"/>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import ViewCourseRegister from '@/js/models/views/view-course-register';
// import CurrentUser from '@/js/models/entities/current-user';
import { GetRequest, Request } from '@/js/services/request';

export default {
    name: 'CoursePanelMember',
    components: {
        TooltipUserAvatar
    },
    props: {
    },
    data(){
        return {
            userAvatarSize: 28,
            numberMembers: 0,
            listMembers: [],
            requiredMemberNumbers: 8,
            currentUser: null,
            isLoaded: false,
        }
    },
    async created(){
        await this.getMembers();
    },
    mounted(){
        
    },
    methods: {
        async getMembers(){
            try {
                if (this.getCourse() == null) {
                    return;
                }
                this.isLoaded = true;
                let res = await new GetRequest('CourseRelations/registers/' + this.getCourse().UserItemId)
                    .setParams({
                        limit: this.requiredMemberNumbers,
                        offset: 0
                    }).execute();
                let body = await Request.tryGetBody(res);
                this.numberMembers = Common.formatNumber(Number(body?.Total ?? 0)); 
                this.listMembers = body?.Results?.filter(function(item){
                    return item != null;
                })?.map(function(item){
                    return new ViewCourseRegister().copy(item);
                }) ?? [];
            } catch (e) {
                Request.resolveAxiosError(e);
            }
        },

    },
    inject: {
        getCourse: {},
    }
}

</script>

<style scoped>

.p-course-panel-member{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    width: 100%;
}

.p-number-member{
    font-size: 14.5px;
    color: var(--grey-color-600);
    font-family: 'ks-font-semibold';
    text-align: start;
}

.p-list-members{
    display: flex;
    flex-flow: row-reverse nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 2px;
    height: 30px;
}

.p-list-members__item{
    width: 20px;
    overflow: visible;
}

.p-list-members__item_container{
    width: 34px;
    height: 34px;
    border-radius: 100%;
    background-color: white;
    position: relative;

    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

</style>

