

<template>
    <div class="p-user-oriented-crb" v-if="isLoaded">
        <UserRelationButton v-if="isMyCourse === false" />
        <InvitedUoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Invited" />
        <MemberUoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Member" />
        <RequestedUoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Requesting" />
        <div v-show="false" v-else-if="dCourseRoleType?.value == eCourseRoleType.Owner"></div>
        <GuestUoCrb v-else />
    </div>
</template>



<script>

import { GetRequest, Request } from '@/js/services/request';
import UserRelationButton from '../../../profile/components/user-relation-button/UserRelationButton.vue';
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
        UserRelationButton
    },
    props: {
        initCourseRoleType: {
            type: Number,
            default: myEnum.ECourseRoleType.NotInRelation
        }
    },
    data(){
        return {
            isLoaded: false,
            dCourse: null,
            dUser: null,
            dCourseRoleType: {
                value: null
            },
            dCourseRelationId: {
                value: null
            },
            eCourseRoleType: myEnum.ECourseRoleType,
            isMyCourse: false,
        }
    },
    async created(){
        try {
            this.dCourse = this.getCourse();
            this.dUser = this.getUserCourse();
            this.isMyCourse = await this.getIsMyCourse();
            this.dCourseRoleType.value = this.dUser?.CourseRoleType;
            this.dCourseRelationId.value = this.dUser?.CourseRelationId;

            if (this.dCourseRoleType.value == null || this.dCourseRelationId.value == null){
                await this.refresh();
            } else {
                this.isLoaded = true;
            }
        } catch (error) {
            console.error(error);
        }
    },
    mounted(){
    },
    methods: {
        async refresh(){
            try {
                this.isLoaded = false;
                // get status between course and userid
                let courseId = this.dCourse.UserItemId;
                let userId = this.dUser.UserId;

                let url = 'CourseRelations/course-user-status/' + courseId + '/' + userId;
                let res = await new GetRequest(url)
                    .setParams({
                        isFocusCourse: false
                    }).execute();
                let body = await Request.tryGetBody(res);
                this.dCourseRoleType.value = body.CourseRoleType;
                this.dCourseRelationId.value = body.CourseRelationId;
                this.isLoaded = true;
            } catch (error) {
                console.error(error);
            }
        },

        async forceRender(){
            try {
                await this.refresh();
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
            getUser: () => this.dUser,
        }
    },
    inject: {
        getCourse: {},
        getIsMyCourse: {},
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

