

<template>
    <div class="p-crb-owner" v-if="isShow">
        <MMenuContextPopup :options="getOptions()">
            <MSecondaryButton 
                label="Tùy chỉnh khóa học"
                :onclick="()=>{}"
                :buttonStyle="buttonStyle"
                fa="pencil" family="fas" :iconStyle="iconStyle"
                ref="button"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import CurrentUser from '@/js/models/entities/current-user';
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'
import { useRouter } from 'vue-router';
import { DeleteRequest, Request } from '@/js/services/request';

export default {
    name: 'OwnerOrientedCrb',
    components: {
        MMenuContextPopup,
        MSecondaryButton,
    },
    props: {
    },
    data(){
        return {
            isShow: true,
            dCourse: null,
            currentUser: null,
            buttonStyle: {

            },
            iconStyle: {
                fontSize: '18px'
            },
            router: useRouter(),
            isWorking: false,
        }
    },
    async mounted(){
        try {
            this.dCourse = this.getCourse();
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
        },

        getOptions(){
            return [
                {
                    family: 'fas',
                    fa: 'plus',
                    onclick: this.resolveOnCreate.bind(this),
                    label: 'Thêm khóa học mới',
                },
                {
                    family: 'fas',
                    fa: 'edit',
                    onclick: this.resolveOnEdit.bind(this),
                    label: 'Chỉnh sửa thông tin khóa học',
                },
                {
                    family: 'fas',
                    fa: 'pen-to-square',
                    onclick: this.resolveOnUpdate.bind(this),
                    label: 'Cập nhật nội dung khóa học',
                },
                {
                    family: 'fas',
                    fa: 'dollar-sign',
                    onclick: this.resolveOnEditCost.bind(this),
                    label: 'Cập nhật giá',
                },
                {
                    family: 'fas',
                    fa: 'lock',
                    onclick: this.resolveOnEditPrivacy.bind(this),
                    label: 'Thay đổi quyền riêng tư',
                },
                {
                    family: 'fas',
                    fa: 'trash-can',
                    onclick: this.resolveOnDelete.bind(this),
                    label: 'Xóa khóa học',
                },

            ]
        },

        async resolveOnCreate(){
            try {
                this.router.push('/course-create');
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnEdit(){
            try {
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;
                this.router.push(`/course-edit/${courseId}`); 
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnUpdate(){
            try {
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;
                this.router.push(`/course-update/${courseId}`);
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnEditCost(){
            try {
                console.log("Click Edit Course Cost");
                // Create update cost popup
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnEditPrivacy(){
            try {
                console.log("Click Edit Course Privacy");
                // Create update privacy popup
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnDelete(){
            try {
                let confirmMessage = "Bạn có chắc chắn muốn xóa khóa học này không?";
                this.getPopupManager().inform(confirmMessage, this.submitDeleteCourse.bind(this));
            } catch (e){
                console.error(e);
            }
        },

        async submitDeleteCourse(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;
                await new DeleteRequest("Courses/" + courseId)
                    .execute();
                window.location.reload();
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        }

    },
    provide(){
        return {
            forceUpdateOwnerOrientedCrb: this.forceRender
        }
    },
    inject: {
        getCourse: {},
        getToastManager: {},
        getPopupManager: {}
    }
}

</script>

<style scoped>

.p-crb-owner{
    width: 100%;
}

</style>

