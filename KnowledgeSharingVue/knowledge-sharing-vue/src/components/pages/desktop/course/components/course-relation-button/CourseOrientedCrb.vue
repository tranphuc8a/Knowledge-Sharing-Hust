

<template>
    <div class="p-course-oriented-crb" v-if="isShow">
        <InvitedCoCrb v-if="dCourseRoleType?.value == eCourseRoleType.Invited" />
        <MemberCoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Member" />
        <RequestedCoCrb v-else-if="dCourseRoleType?.value == eCourseRoleType.Requesting" />
        <GuestCoCrb v-else />
    </div>
</template>



<script>

// import { GetRequest, Request } from '@/js/services/request';
import InvitedCoCrb from './InvitedCoCrb.vue';
import MemberCoCrb from './MemberCoCrb.vue';
import RequestedCoCrb from './RequestedCoCrb.vue';
import GuestCoCrb from './GuestCoCrb.vue';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'CourseOrientedCrb',
    components: {
        InvitedCoCrb,
        MemberCoCrb,
        RequestedCoCrb,
        GuestCoCrb,
    },
    props: {
    },
    data(){
        return {
            isShow: true,
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
            this.dCourseRoleType.value = this.dCourse.CourseRoleType;
            this.dCourseRelationId.value = this.dCourse.CourseRelationId;
            // this.refresh();
        } catch (error) {
            console.error(error);
        }
    },
    mounted(){
    },
    methods: {
        // async refresh(){
        //     try {
        //         // let courseId = this.getCourse?.()?.UserItemId;
        //         // console.log(".")
        //         // get status between course and userid
        //         // let url = 'CourseRelations/course-status/' + courseId;
        //         // let res = await new GetRequest(url)
        //         //     .setParams({
        //         //         isFocusCourse: true
        //         //     }).execute();
        //         // let body = await Request.tryGetBody(res);
        //         // this.dCourseRoleType.value = body.CourseRoleType;
        //         // this.dCourseRelationId.value = body.CourseRelationId;
        //     } catch (error) {
        //         console.error(error);
        //     }
        // },

        async forceRender(){
            try {
                await this.refresh();
                this.isShow = false;
                let that = this;
                this.$nextTick(() => {
                    that.isShow = true;
                });
            } catch (e){
                console.error(e);
            }
        }
    },
    provide(){
        return {
            forceUpdateCourseOrientedCrb: this.forceRender,
            getCourseRelationId: () => this.dCourseRelationId,
            getCourseRoleType: () => this.dCourseRoleType,
        }
    },
    inject: {
        getCourse: {},
        getToastManager: {},
        getPopupManager: {},
    }
}

</script>

<style scoped>

.p-course-oriented-crb{
    width: 100%;
}

</style>

