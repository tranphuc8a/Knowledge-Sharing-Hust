

<template>
    <div class="p-course-relation-button" v-if="isShow">
        <UserOrientedCrb v-if="dUser != null" />
        <OwnerOrientedCrb v-else-if="dCourseRoleType == eCourseRoleType.Owner" />
        <CourseOrientedCrb v-else />
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
        user: {
            type: Object,
            default: null
        }
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
            this.dCourseRoleType = this.dCourse?.CourseRoleType;
            this.dUser = this.user;
            this.currentUser = await CurrentUser.getInstance();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        async forceRender(){
            try {
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
            forceUpdateCourseRelationButton: this.forceRender,
            getUserCourse: () => this.dUser,
        }
    },
    inject: {
        getCourse: {},
    }
}

</script>

<style scoped>

.p-course-relation-button{
    width: 100%;
}

</style>

<style>
.p-course-relation-button .p-popup-context-container,
.p-course-relation-button .p-tooltip-container{
    width: 100%;
}
</style>
