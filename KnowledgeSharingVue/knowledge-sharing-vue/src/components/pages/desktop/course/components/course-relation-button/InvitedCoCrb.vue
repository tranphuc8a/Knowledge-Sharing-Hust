

<template>
    <div class="p-invited-co-crb">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Phản hồi lời mời"
                :onclick="null"
                :buttonStyle="buttonStyle"
                fa="circle-question" family="fas" :iconStyle="iconStyle"
                ref="button"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import { PostRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue';
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
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
            let res = await this.confirmInvite(true);
            return res;
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
                this.$refs.button.loading();
                this.isWorking = true;
                let inviteId = this.dCourseRelationId;
                await new PostRequest('CourseRelations/invite/confirm/' + inviteId + '/' + isAccept)
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
                this.$refs.button?.normal?.();
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

.p-invited-co-crb{
    width: 100%;
}

</style>

