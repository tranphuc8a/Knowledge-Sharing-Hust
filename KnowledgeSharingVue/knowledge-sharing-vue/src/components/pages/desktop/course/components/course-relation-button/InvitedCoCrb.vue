

<template>
    <div class="p-invited-co-crb">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Phản hổi lời mời"
                :onclick="()=>{}"
                :buttonStyle="buttonStyle"
                fa="circle-question" family="fas" :iconStyle="iconStyle"
                ref="button"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import { PostRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'
import CourseRelation from '@/js/models/entities/course-relation';
import CourseRegister from '@/js/models/entities/course-register';
import MMenuContextPopup from '@/components/base/tooltip/MMenuContextPopup.vue';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'InvitedCoCrb',
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
                    fa: 'circle-check',
                    onclick: this.resolveAcceptInvite.bind(this),
                    label: 'Đồng ý tham gia'
                },
                {
                    fa: 'circle-xmark',
                    onclick: this.resolveDenyInvite.bind(this),
                    label: 'Từ chối'
                }
            ]
        },

        async resolveAcceptInvite(){
            return await this.confirmInvite(true);
        },


        async resolveDenyInvite(){
            try {
                let alertMessage = 'Bạn có chắc chắn muốn từ chối lời mời này?';
                let that = this;
                let callback = async function(){
                    return await that.confirmInvite(false);
                }
                this.getPopupManager().inform(alertMessage, callback);
            } catch (e) {
                console.error(e);
            }
        },

        async confirmInvite(isAccept){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let inviteId = this.dCourseRelationId;
                let res = await new PostRequest('CourseRelations/invite/confirm/' + inviteId + '/' + isAccept)
                    .execute();
                // success:
                if (isAccept){
                    this.getCourseRoleType().value = myEnum.ECourseRoleType.Member;
                } else {
                    this.getCourseRoleType().value = myEnum.ECourseRoleType.NotInRelation;
                }
            } catch (e) {
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
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

.p-invited-co-crb{
    width: 100%;
}

</style>

