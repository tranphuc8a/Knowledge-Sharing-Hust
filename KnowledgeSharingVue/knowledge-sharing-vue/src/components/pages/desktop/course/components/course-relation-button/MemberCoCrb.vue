

<template>
    <div class="p-member-co-crb">
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
import { PostRequest, Request } from '@/js/services/request';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue';
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import { useRouter } from 'vue-router';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'MemberCoCrb',
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
                    fa: 'right-from-bracket',
                    onclick: this.resolveLeaveCourse,
                    label: 'Rời khóa học'
                }
            ]
        },

        async resolveLeaveCourse(){
            try {
                let alertMsg = 'Bạn có chắc chắn muốn rời khóa học này?';
                let that = this;
                let callback = async function(){
                    await that.submitLeaveCourse();
                }
                this.getPopupManager().inform(alertMsg, callback);
            } catch (e){
                console.error(e);
            }
        },

        async submitLeaveCourse(){
            if(this.isWorking) return;
            try {
                this.isWorking = true;
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;
                await new PostRequest('CourseRelations/unregister/' + courseId)
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

.p-member-co-crb{
    width: 100%;
}

</style>

