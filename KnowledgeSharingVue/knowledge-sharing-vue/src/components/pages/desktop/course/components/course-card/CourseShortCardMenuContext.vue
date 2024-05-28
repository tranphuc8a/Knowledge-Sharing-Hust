

<template>
    <m-context-popup>
        <template #popupContextMask>
            <MActionIcon fa="ellipsis-h" :onclick="null"
                :iconStyle="iconStyle" :containerStyle="containerStyle" />
        </template>

        <template #popupContextContent>
            <div class="p-cmp-content">
                <template v-for="(option) in listOptions" :key="option?.id">
                    <div class="p-cmp-option" @:click="options[option].onClick">
                        <MIcon :style="iconStyle" :fa="options[option].fa" 
                            :family="options[option]?.family ?? 'fas'"    
                        />
                        <span> {{ options[option].label }} </span>
                    </div>
                </template>
            </div>
        </template>

    </m-context-popup>
</template>

<script>
import { myEnum } from '@/js/resources/enum';
import MContextPopup from '@/components/base/popup/MContextPopup.vue';
import CurrentUser from '@/js/models/entities/current-user';
// import { MyRandom } from '@/js/utils/myrandom';
import { useRouter } from 'vue-router';
import { DeleteRequest, PostRequest, Request } from '@/js/services/request';

export default {
    name: "CourseShortCardMenuContext",
    props: {
        onEdit: {},
        onDelete: {},
        onComment: {},
    },
    components: {
        MContextPopup
    },
    async mounted(){
        try {
            this.course = this.getCourse();
            await this.updateOptions();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async resolveEdit(){
            try {
                if (this.onEdit){
                    this.onEdit();
                }
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                let url = '/course-edit/' + courseId;
                this.router.push(url);
            }
            catch (error){
                console.error(error);
            }
        },
        async resolveUpdate(){
            try {
                if (this.onUpdate){
                    this.onUpdate();
                }
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                let url = '/course-update/' + courseId;
                this.router.push(url);
            }
            catch (error){
                console.error(error);
            }
        },
        async resolveDelete(){
            try {
                if (this.onDelete){
                    this.onDelete();
                }
                this.getPopupManager().inform(`Bạn có chắc chắn muốn xóa khóa học này không?`, this.submitDelete.bind(this));
            } catch (e) {
                console.error(e);
            }
        },
        async submitDelete(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                let url = 'Course/' + courseId;
                await new DeleteRequest(url).execute();
                
                // delete success:
                this.getToastManager().success(`Xóa khóa học thành công!`);

                // push sang location hop ly hon
                window.location.reload();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveMark(){
            try {
                await new PostRequest('Knowledges/mark/' + this.getCourse()?.UserItemId)
                    .execute();
                this.getCourse().IsMarked = true;
                await this.updateOptions();
                this.getToastManager().success(`Lưu thành công!`);
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveUnmark(){
            try {
                await new PostRequest('Knowledges/unmark/' + this.getCourse()?.UserItemId)
                    .execute();
                this.getCourse().IsMarked = false;
                await this.updateOptions();
                this.getToastManager().success(`Đã bỏ lưu!`);
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveReport(){
            try {
                this.getToastManager().inform("Tính năng đang được phát triển, vui lòng thử lại sau!");
            } catch (e){
                console.error(e);
            }
        },
        async resolveLockComment(){
            try {
                await new PostRequest('Comments/block/' + this.getCourse()?.UserItemId)
                    .execute();
                this.getCourse().IsBlockComment = true;
                await this.updateOptions();
                this.getToastManager().success(`Đã khóa bình luận!`);
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveUnlockComment(){
            try {
                await new PostRequest('Comments/unblock/' + this.getCourse()?.UserItemId)
                    .execute();
                this.getCourse().IsBlockComment = false;
                this.getToastManager().success(`Đã mở khóa bình luận!`);
                await this.updateOptions();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveCopyUserItemId(){
            try {
                let itemId = this.getCourse()?.UserItemId;
                if (itemId != null){
                    navigator.clipboard.writeText(itemId.toString());
                    this.getToastManager().success('Đã sao id khóa học chép vào clipboard');
                }
            } catch (error) {
                console.error(error);
            }
        },
        async resolveGotoCourse(){
            try {
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                this.router.push('/course/' + courseId);
            } catch (e){
                console.error(e);
            }
        },

        async updateOptions(){
            try {
                // role of user for a course:
                // admin, anonymouse, owner, user, banned ( ~ anonymous )
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){ 
                    // Anonymous
                    this.listOptions = [
                        this.actions.Report,
                        this.actions.CopyUserItemId,
                        this.actions.GotoCourse
                    ];
                } else if (this.currentUser.Role == myEnum.EUserRole.Admin){
                    // Admin
                    this.listOptions = [
                        this.actions.Delete,
                        this.actions.Report,
                        this.actions.CopyUserItemId,
                        this.actions.GotoCourse
                    ];
                    if (this.getCourse().IsMarked){
                        this.listOptions.push(this.actions.Unmark);
                    } else {
                        this.listOptions.push(this.actions.Mark);
                    }
                    if (this.getCourse()?.IsBlockComment){
                        this.listOptions.push(this.actions.UnlockComment);
                    } else {
                        this.listOptions.push(this.actions.LockComment);
                    }
                } else if (this.currentUser.Role == myEnum.EUserRole.Banned){
                    // Banned
                    this.listOptions = [
                        this.actions.Report,
                        this.actions.CopyUserItemId,
                        this.actions.GotoCourse
                    ];
                } else {
                    // User:
                    if (this.currentUser.UserId == this.getCourse()?.UserId){
                        // Owner
                        this.listOptions = [
                            this.actions.Edit,
                            this.actions.Update,
                            this.actions.Delete,
                            this.actions.Report,
                            this.actions.CopyUserItemId,
                            this.actions.GotoCourse
                        ];
                        if (this.getCourse().IsMarked){
                            this.listOptions.push(this.actions.Unmark);
                        } else {
                            this.listOptions.push(this.actions.Mark);
                        }
                        if (this.getCourse()?.IsBlockComment){
                            this.listOptions.push(this.actions.UnlockComment);
                        } else {
                            this.listOptions.push(this.actions.LockComment);
                        }
                    } else {
                        // Not Owner
                        this.listOptions = [
                            this.actions.Report,
                            this.actions.CopyUserItemId,
                            this.actions.GotoCourse
                        ];
                        if (this.getCourse().IsMarked){
                            this.listOptions.push(this.actions.Unmark);
                        } else {
                            this.listOptions.push(this.actions.Mark);
                        }
                    }
                }
            }
            catch (error){
                console.error(error);
            }
        }
    },
    data() {
        return {
            course: null,
            router: useRouter(),
            currentUser: null,
            iconStyle: {
                fontSize: '18px',
                color: 'var(--primary-color)'
            },
            containerStyle: {
                width: '36px',
                height: '36px'
            },
            actions: {
                Edit: 1,
                Delete: 2,
                Update: 3,
                Mark: 4,
                Unmark: 5,
                Report: 6,
                LockComment: 10,
                UnlockComment: 11,
                CopyUserItemId: 12,
                GotoCourse: 13,
            },
            listOptions: [],
            options: {
                [1]: {
                    id: 1,
                    label: 'Chỉnh sửa thông tin',
                    fa: 'pencil',
                    onClick: this.resolveEdit
                }, 
                [2]: {
                    id: 2,
                    label: 'Xóa',
                    fa: 'trash-can',
                    onClick: this.resolveDelete,
                },
                [3]: {
                    id: 3,
                    label: 'Cập nhật bài giảng',
                    fa: 'pencil',
                    onClick: this.resolveUpdate,
                },
                [4]: {
                    id: 4,
                    label: 'Lưu',
                    fa: 'bookmark',
                    family: 'far',
                    onClick: this.resolveMark,
                },
                [5]: {
                    id: 5,
                    label: 'Bỏ lưu',
                    fa: 'bookmark',
                    onClick: this.resolveUnmark,
                },
                [6]: {
                    id: 6,
                    label: 'Báo cáo',
                    fa: 'flag',
                    onClick: this.resolveReport,
                },
                [10]: {
                    id: 10,
                    label: 'Khóa bình luận',
                    fa: 'lock',
                    onClick: this.resolveLockComment,
                },
                [11]: {
                    id: 11,
                    label: 'Mở khóa bình luận',
                    fa: 'unlock',
                    onClick: this.resolveUnlockComment,
                },
                [12]: {
                    id: 12,
                    label: 'Sao chép id phần tử',
                    fa: 'copy',
                    onClick: this.resolveCopyUserItemId,
                },
                [13]: {
                    id: 13,
                    label: 'Đi đến khóa học',
                    fa: 'forward',
                    onClick: this.resolveGotoCourse,
                },
            }
        }
    },
    inject: {
        getLanguage: {},
        getCourse: {},
        getPopupManager: {},
        getToastManager: {},
    }
}

</script>

<style scoped>

.p-cmp-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-cmp-option{
    width: 100%;
    padding: 8px 28px 8px 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-cmp-option :first-child{
    width: 20px;
}

.p-cmp-option:hover{
    background-color: var(--primary-color-100);
}

.p-cmp-option:active{
    background-color: var(--primary-color-200);
}

</style>