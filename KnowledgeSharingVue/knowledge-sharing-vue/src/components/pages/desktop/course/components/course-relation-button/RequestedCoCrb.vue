

<template>
    <div class="p-requested-co-crb">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Đã gửi yêu cầu"
                :onclick="()=>{}"
                :buttonStyle="buttonStyle"
                ref="button"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import { DeleteRequest, PostRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'
import CourseRelation from '@/js/models/entities/course-relation';
import CourseRegister from '@/js/models/entities/course-register';
import MMenuContextPopup from '@/components/base/tooltip/MMenuContextPopup.vue';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'RequestedCoCrb',
    components: {
        MSecondaryButton, MMenuContextPopup
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
            dCourse: null,
            dCourseRelationId: null,
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
            } catch (e){
                console.error(e);
            }
        },

        getOptions(){
            return [
                {
                    fa: 'user-minus',
                    onclick: this.resolveDeleteRequest.bind(this),
                    label: 'Xóa yêu cầu'
                },
                {
                    fa: 'credit-card',
                    onclick: this.resolvePayCourse.bind(this),
                    label: 'Thanh toán ngay'
                }
            ]
        },

        async resolveDeleteRequest(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                this.$refs.button.loading();
                let requestId = this.dCourseRelationId;
                if (requestId == null) return;
                await new DeleteRequest('CourseRelations/request/' + requestId)
                    .execute();
                this.getCourseRoleType().value = myEnum.ECourseRoleType.NotInRelation;
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
                this.$refs.button?.normal?.();
            }
        },


        async resolvePayCourse(){
            if (this.isWorking) return;
            try {
                let courseId = this.dCourse.UserItemId;
                if (courseId == null) return;
                this.router.push('/pay-course/' + courseId);
            } catch (e){
                Request.resolveAxiosError(e);
            }
        }
    },
    inject: {
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

.p-requested-co-crb{
    width: 100%;
}

</style>

