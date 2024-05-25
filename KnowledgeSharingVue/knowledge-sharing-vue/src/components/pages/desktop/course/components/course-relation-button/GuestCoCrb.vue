

<template>
    <div class="p-guest-co-crb">
        <div class="p-guest-co-crb__notfree" v-show="!isCourseFree">
            <TooltipMenuContext :options="getOptions()">
                <MButton 
                    label="Thanh toán ngay"
                    :onclick="resolveClickPayment"
                    :buttonStyle="buttonStyle"
                    fa="credit-card" family="fas" :iconStyle="iconStyle"
                    ref="button-payment"
                />
            </TooltipMenuContext>
        </div>
        <div class="p-guest-co-crb__free" v-show="isCourseFree">
            <MButton 
                label="Đăng ký"
                :onclick="resolveClickRegister"
                :buttonStyle="buttonStyle"
                fa="address-card" family="fas" :iconStyle="iconStyle"
                ref="button-register"
            />
        </div>
    </div>
</template>



<script>
import { PostRequest, Request } from '@/js/services/request';
import MButton from './../../../../../base/buttons/MButton.vue'
import CourseRelation from '@/js/models/entities/course-relation';
import CourseRegister from '@/js/models/entities/course-register';
import TooltipMenuContext from '@/components/base/tooltip/TooltipMenuContext.vue';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'GuestCoCrb',
    components: {
        MButton, TooltipMenuContext
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
            isCourseFree: false,
            router: useRouter(),
        }
    },
    async created(){
        this.refresh();
    },
    methods: {
        async refresh(){
            try {
                this.dCourse = this.getCourse();
                this.dCourseRelationId = this.getCourseRelationId().value;
                this.isWorking = false;
                this.isCourseFree = !(Number(this.dCourse.Fee) > 0);
            } catch (e){
                console.error(e);
            }
        },

        getOptions(){
            return [
                {
                    fa: 'hand',
                    onclick: this.resolveClickRequest.bind(this),
                    label: 'Yêu cầu tham gia'
                }
            ]
        },

        async resolveClickPayment(){
            try {
                let courseId = this.dCourse.UserItemId;
                if (courseId == null) return;
                this.router.push('/pay-course/' + courseId);
            } catch (e) {
                console.error(e);
            }
        },


        async resolveClickRegister(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let courseId = this.dCourse.UserItemId;
                if (courseId == null) {
                    return;
                }
                let res = await new PostRequest('CourseRelations/register/' + courseId)
                    .execute();
                let body = await Request.tryGetBody(res);
                let courseRegister = new CourseRegister().copy(body);
                this.getCourseRelationId().value = courseRegister.CourseRegisterId;
                this.getCourseRoleType().value = myEnum.ECourseRoleType.Member;
            } catch (e) {
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveClickRequest(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                this.$refs['button-payment'].loading();
                let courseId = this.dCourse.UserItemId;
                if (courseId == null) {
                    return;
                }
                let res = await new PostRequest('CourseRelations/request/' + courseId)
                    .execute();
                let body = await Request.tryGetBody(res);
                let courseRelation = new CourseRelation().copy(body);
                this.getCourseRelationId().value = courseRelation.CourseRelationId;
                this.getCourseRoleType().value = myEnum.ECourseRoleType.Requesting;
            } catch (e) {   
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
                this.$refs['button-payment']?.normal?.();
            }
        }
    },
    inject: {
        getCourse: {},
        getCourseRelationId: {},
        getCourseRoleType: {},
        forceUpdateCourseOrientedCrb: {},
        getToastManager: {},
        getPopupManager: {},
    }
}

</script>

<style scoped>

.p-guest-co-crb{
    width: 100%;
}

</style>

