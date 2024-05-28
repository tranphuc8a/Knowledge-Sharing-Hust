

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
    name: "CourseLessonMenuContext",
    props: {
        onEdit: {},
        onDelete: {},
    },
    components: {
        MContextPopup
    },
    async mounted(){
        try {
            this.post = this.getPost();
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
                this.getPopupManager().inform(`Bạn có chắc chắn muốn xóa bài giảng này khỏi khóa học không?`, this.submitDelete.bind(this));
            } catch (e) {
                console.error(e);
            }
        },
        async submitDelete(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let participantId = this.getCourseLesson()?.CourseLessonId;
                if (participantId == null) return;
                let url = 'CourseLessons/lesson/' + participantId;
                await new DeleteRequest(url).execute();
                
                // delete success:
                this.getToastManager().success(`Đã xóa bài giảng khỏi khóa học!`);

                // push sang location hop ly hon
                window.location.reload();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            } finally {
                this.isWorking = false;
            }
        },
        async resolveMark(){
            try {
                await new PostRequest('Knowledges/mark/' + this.getPost()?.UserItemId).execute();
                this.getPost().IsMarked = true;
                await this.updateOptions();
                this.getToastManager().success(`Lưu thành công!`);
                this.forceUpdateCourseLessonCard?.();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveUnmark(){
            try {
                await new PostRequest('Knowledges/unmark/' + this.getPost()?.UserItemId).execute();
                this.getPost().IsMarked = false;
                await this.updateOptions();
                this.getToastManager().success(`Đã bỏ lưu!`);
                this.forceUpdateCourseLessonCard?.();
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
        async resolveHistory(){
            try {
                this.getToastManager().inform("Tính năng đang được phát triển, vui lòng thử lại sau!");
            } catch (e){
                console.error(e);
            }
        },
        async resolveCopyUserItemId(){
            try {
                let itemId = this.getCourseLesson()?.CourseLessonId;
                if (itemId != null){
                    navigator.clipboard.writeText(itemId.toString());
                    this.getToastManager().success('Đã sao chép mã bài giảng trong khóa học vào clipboard');
                }
            } catch (error) {
                console.error(error);
            }
        },

        async updateOptions(){
            try {
                // role of user for a course-lesson:
                // admin, owner of course lesson, participant
                this.currentUser = await CurrentUser.getInstance();
                let anonymousActions = [
                    this.actions.Report,
                    this.actions.History,
                    this.actions.CopyUserItemId,
                ]
                if (this.currentUser == null){ 
                    // Anonymous
                    this.listOptions = anonymousActions;
                } else if (this.currentUser.Role == myEnum.EUserRole.Admin){
                    // Admin
                    this.listOptions = [
                        this.actions.Delete,
                        ...anonymousActions
                    ];
                    if (this.getPost().IsMarked){
                        this.listOptions.push(this.actions.Unmark);
                    } else {
                        this.listOptions.push(this.actions.Mark);
                    }
                } else if (this.currentUser.Role == myEnum.EUserRole.Banned){
                    // Banned
                    this.listOptions = anonymousActions;
                } else {
                    // User:
                    if (this.currentUser.UserId == this.getPost()?.UserId){
                        // Owner
                        this.listOptions = [
                            this.actions.Edit,
                            this.actions.Delete,
                            ...anonymousActions
                        ];
                        if (this.getPost().IsMarked){
                            this.listOptions.push(this.actions.Unmark);
                        } else {
                            this.listOptions.push(this.actions.Mark);
                        }
                    } else {
                        // Not Owner (Participant)
                        this.listOptions = [
                            ...anonymousActions
                        ];
                        if (this.getPost().IsMarked){
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
            post: null,
            router: useRouter(),
            currentUser: null,
            isWorking: false,
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
                Mark: 4,
                Unmark: 5,
                Report: 6,
                History: 7,
                CopyUserItemId: 12,
            },
            listOptions: [],
            options: {
                [1]: {
                    id: 1,
                    label: 'Chỉnh sửa',
                    fa: 'pencil',
                    onClick: this.resolveEdit
                }, 
                [2]: {
                    id: 2,
                    label: 'Xóa',
                    fa: 'trash-can',
                    onClick: this.resolveDelete,
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
                [7]: {
                    id: 7,
                    label: 'Lịch sử chỉnh sửa',
                    fa: 'history',
                    onClick: this.resolveHistory,
                }, 
                [12]: {
                    id: 12,
                    label: 'Sao chép id phần tử',
                    fa: 'copy',
                    onClick: this.resolveCopyUserItemId,
                },
            }
        }
    },
    inject: {
        forceUpdateCourseLessonCard: { default: () => null },
        getCourseLesson: { default: () => null },
        getPost: {},
        getCourse: { default: () => null },
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