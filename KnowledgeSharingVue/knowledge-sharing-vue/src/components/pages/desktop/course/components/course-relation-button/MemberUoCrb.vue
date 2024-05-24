

<template>
    <div class="p-member-uo-crb">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Đã tham gia"
                :onclick="null"
                :buttonStyle="buttonStyle"
                ref="button"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import { DeleteRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue';
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'MemberUoCrb',
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
            dUser: null,
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
                this.dUser = this.getUserCourse?.();
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
                    onclick: this.resolveDeleteMember.bind(this),
                    label: 'Xóa thành viên'
                }
            ]
        },

        async resolveDeleteMember(){
            try {
                let alertMsg = 'Bạn có chắc chắn muốn xóa thành viên này?';
                let that = this;
                let callback = async function(){
                    await that.submitDeleteMember();
                }
                this.getPopupManager().inform(alertMsg, callback);
            } catch (e){
                console.error(e);
            }
        },

        async submitDeleteMember(){
            if(this.isWorking) return;
            try {
                this.isWorking = true;
                let registerId = this.dCourseRelationId;
                if (registerId == null) return;
                await new DeleteRequest('CourseRelations/register/' + registerId)
                    .execute();
                this.getCourseRoleType().value = myEnum.ECourseRoleType.NotInRelation;
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
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

.p-member-uo-crb{
    width: 100%;
}

</style>

