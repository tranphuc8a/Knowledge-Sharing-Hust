

<template>
    <div class="p-invited-uo-crb">
        <MDeleteButton 
            label="Xóa lời mời"
            :onclick="resolveDeleteInvite"
            :buttonStyle="buttonStyle"
            fa="user-xmark" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </div>
</template>



<script>
import { DeleteRequest, Request } from '@/js/services/request';
import MDeleteButton from './../../../../../base/buttons/MDeleteButton.vue';
import { myEnum } from '@/js/resources/enum';



export default {
    name: 'InvitedUoCrb',
    components: {
        MDeleteButton,
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
            dCourseRelationId: null
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


        async resolveDeleteInvite(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let inviteId = this.dCourseRelationId;
                await new DeleteRequest('CourseRelations/invite/' + inviteId)
                    .execute();
                this.getCourseRoleType().value = myEnum.ECourseRoleType.NotInRelation;
            } catch (e) {
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

.p-invited-uo-crb{
    width: 100%;
}

</style>

