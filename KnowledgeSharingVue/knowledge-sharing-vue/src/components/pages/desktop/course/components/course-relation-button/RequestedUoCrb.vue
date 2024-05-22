

<template>
    <div class="p-requested-uo-crb">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Phản hồi yêu cầu"
                :onclick="null"
                :buttonStyle="buttonStyle"
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
    name: 'RequestedUoCrb',
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
                    fa: 'circle-check',
                    onclick: this.resolveAcceptRequest.bind(this),
                    label: 'Chấp nhận yêu cầu'
                },
                {
                    fa: 'circle-xmark',
                    onclick: this.resolveDenyRequest.bind(this),
                    label: 'Từ chối'
                }
            ]
        },

        async resolveAcceptRequest(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                this.$refs.button.loading();
                let requestId = this.dCourseRelationId;
                await new PostRequest('CourseRelations/request/confirm/' + requestId + '/true')
                    .execute();
                this.getCourseRoleType().value = myEnum.ECourseRoleType.Member;
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
                this.$refs.button?.normal?.();
            }
        },


        async resolveDenyRequest(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                this.$refs.button.loading();
                let requestId = this.dCourseRelationId;
                await new PostRequest('CourseRelations/request/confirm/' + requestId + '/false')
                    .execute();
                this.getCourseRoleType().value = myEnum.ECourseRoleType.NotInRelation;
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
                this.$refs.button?.normal?.();
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

.p-requested-uo-crb{
    width: 100%;
}

</style>

