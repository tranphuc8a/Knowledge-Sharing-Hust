

<template>
    <div class="p-guest-ou-crb">
        <MButton 
            label="Mời tham gia khóa học"
            :onclick="resolveClickInvite"
            :buttonStyle="buttonStyle"
            fa="user-plus" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </div>
</template>



<script>
import { PostRequest, Request } from '@/js/services/request';
import MButton from './../../../../../base/buttons/MButton.vue'
import CourseRelation from '@/js/models/entities/course-relation';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'GuestUoCrb',
    components: {
        MButton,
    },
    props: {
    },
    data(){
        return {
            buttonStyle: {

            }, iconStyle: {
                fontSize: '18px'
            },
            isWorking: false,
            dUser: null,
            dCourse: null,
            dCourseRelationId: null,
        }
    },
    async created(){
        this.refresh();
    },
    methods: {
        async resolveClickInvite(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let userId = this.dUser.UserId;
                let courseId = this.dCourse.UserItemId;
                if (userId == null || courseId == null) {
                    return;
                }
                let res = await new PostRequest('CourseRelations/invite/' + courseId + '/' + userId)
                    .execute();
                let body = await Request.tryGetBody(res);
                let relation = new CourseRelation().copy(body);
                this.dCourseRelationId = relation.CourseRelationId;
                this.getCourseRelationId().value = relation.CourseRelationId;
                this.getCourseRoleType().value = myEnum.ECourseRoleType.Invited;
            } catch (e) {
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },

        async refresh(){
            try {
                this.dUser = this.getUserCourse();
                this.dCourse = this.getCourse();
                this.dCourseRelationId = this.getCourseRelationId().value;
                this.isWorking = false;
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getUserCourse: {},
        getCourse: {},
        getCourseRelationId: {},
        getCourseRoleType: {},
        forceUpdateUserOrientedCrb: {},
        getToastManager: {},
        getPopupManager: {},
    }
}

</script>

<style scoped>

.p-guest-ou-crb{
    width: 100%;
}

</style>

