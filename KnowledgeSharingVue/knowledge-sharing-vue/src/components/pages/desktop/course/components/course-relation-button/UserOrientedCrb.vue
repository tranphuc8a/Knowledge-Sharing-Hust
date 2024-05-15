

<template>
    <div class="p-user-oriented-crb" v-if="isShow">
        <InvitedUoCrb v-if="dCourseRoleType?.value == eCourseRoleType.Invited" />
        <MemberUoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Member" />
        <RequestedUoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Requesting" />
        <GuestUoCrb v-else />
    </div>
</template>



<script>

import { GetRequest } from '@/js/services/request';
import InvitedUoCrb from './InvitedUoCrb.vue';
import MemberUoCrb from './MemberUoCrb.vue';
import RequestedUoCrb from './RequestedUoCrb.vue';
import GuestUoCrb from './GuestUoCrb.vue';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'UserOrientedCrb',
    components: {
        InvitedUoCrb,
        MemberUoCrb,
        RequestedUoCrb,
        GuestUoCrb,
    },
    props: {
        initCourseRoleType: {
            type: Number,
            default: myEnum.ECourseRoleType.NotInRelation
        }
    },
    data(){
        return {
            isShow: false,
            dCourse: null,
            dCourseRoleType: {
                value: null
            },
            dCourseRelationId: {
                value: null
            },
            eCourseRoleType: myEnum.ECourseRoleType,
        }
    },
    async created(){
        try {
            this.dCourse = this.getCourse();
            this.dCourseRoleType.value = this.initCourseRoleType;
            this.refresh();
        } catch (error) {
            console.error(error);
        }
    },
    mounted(){
    },
    methods: {
        async refresh(){
            try {
                let courseId = this.getCourse?.()?.UserItemId;
                let userId = this.getUserCourse?.()?.UserId;
                // get status between course and userid
                let url = 'CourseRelations/course-user-status/' + courseId + '/' + userId;
                let res = await new GetRequest(url)
                    .setParams({
                        isFocusCourse: false
                    }).execute();
                let body = await Request.tryGetBody(res);
                this.dCourseRoleType.value = body.CourseRoleType;
                this.dCourseRelationId.value = body.CourseRelationId;
            } catch (error) {
                console.error(error);
            }
        },

        async forceRender(){
            try {
                await this.refresh();
                this.isShow = false;
                this.$nextTick(() => {
                    this.isShow = true;
                });
            } catch (e){
                console.error(e);
            }
        }
    },
    provide(){
        return {
            forceUpdateUserOrientedCrb: this.forceRender,
            getCourseRelationId: () => this.dCourseRelationId,
            getCourseRoleType: () => this.dCourseRoleType,
        }
    },
    inject: {
        getCourse: {},
        getUserCourse: {},
        getToastManager: {},
        getPopupManager: {},
    }
}

</script>

<style scoped>

.p-user-oriented-crb{
    width: 100%;
}

</style>

