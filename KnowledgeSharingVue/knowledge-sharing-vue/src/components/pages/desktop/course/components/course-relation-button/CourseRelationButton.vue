

<template>
    <div class="p-course-relation-button" v-if="isShow">
        <OwnerOrientedCrb v-if="dCourseRoleType == eCourseRoleType.Owner" />
        <CourseOrientedCrb v-else-if="dUser == null" />
        <UserOrientedCrb v-else-if="dUser != null" />
    </div>
</template>



<script>
import OwnerOrientedCrb from './OwnerOrientedCrb.vue';
import CourseOrientedCrb from './CourseOrientedCrb.vue';
import UserOrientedCrb from './UserOrientedCrb.vue';
import CurrentUser from '@/js/models/entities/current-user';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'CourseRelationButton',
    components: {
        OwnerOrientedCrb,
        CourseOrientedCrb,
        UserOrientedCrb
    },
    props: {
    },
    data(){
        return {
            isShow: true,
            dCourse: null,
            dCourseRoleType: null,
            dUser: null,
            currentUser: null,
            eCourseRoleType: myEnum.ECourseRoleType,
        }
    },
    async mounted(){
        try {
            this.dCourse = this.getCourse();
            this.dCourseRoleType = this.dCourse.CourseRoleType;
            this.dUser = this.getUserCourse?.();
            this.currentUser = await CurrentUser.getInstance();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async forceRender(){
            try {
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
            forceUpdateCourseRelationButton: this.forceRender
        }
    },
    inject: {
        getCourse: {},
        getUserCourse: {},
    }
}

</script>

<style scoped>

.p-course-relation-button{
    width: 100%;
}

</style>

