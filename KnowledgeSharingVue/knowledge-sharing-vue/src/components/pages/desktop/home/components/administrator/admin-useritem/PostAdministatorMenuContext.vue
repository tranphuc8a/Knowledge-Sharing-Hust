

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
    name: "PostAdministatorMenuContext",
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
            this.post = this.getPost();
            this.isLesson = this.getPost()?.PostType == myEnum.EPostType.Lesson;
            this.postResource = this.isLesson ? "Bài giảng" : "Bài thảo luận";
            await this.updateOptions();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        
        async resolveDelete(){
            try {
                if (this.onDelete){
                    this.onDelete();
                }
                this.getPopupManager().inform(`Bạn có chắc chắn muốn xóa ${this.postResource} này không?`, this.submitDelete.bind(this));
            } catch (e) {
                console.error(e);
            }
        },
        async submitDelete(){
            try {
                let url = 'Lessons/admin';
                if (this.getPost()?.PostType == myEnum.EPostType.Question){
                    url = 'Questions/admin';
                }
                await new DeleteRequest(url + this.getPost()?.UserItemId)
                    .execute();
                // delete success:
                this.getToastManager().success(`Xóa ${this.postResource} thành công!`);

                if (this.onPostDeleted){
                    this.onPostDeleted(this.getPost()?.UserItemId);
                }
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        
        async resolveMark(){
            try {
                await new PostRequest('Knowledges/mark/' + this.getPost()?.UserItemId)
                    .execute();
                this.getPost().IsMarked = true;
                await this.updateOptions();
                this.getToastManager().success(`Lưu thành công!`);
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveUnmark(){
            try {
                await new PostRequest('Knowledges/unmark/' + this.getPost()?.UserItemId)
                    .execute();
                this.getPost().IsMarked = false;
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
        async resolveHistory(){
            try {
                this.getToastManager().inform("Tính năng đang được phát triển, vui lòng thử lại sau!");
            } catch (e){
                console.error(e);
            }
        },
        async resolveLockComment(){
            try {
                await new PostRequest('Comments/admin/block/' + this.getPost()?.UserItemId)
                    .execute();
                this.getPost().IsBlockComment = true;
                await this.updateOptions();
                this.forceUpdateCommentList?.();
                this.getToastManager().success(`Đã khóa bình luận!`);
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveUnlockComment(){
            try {
                await new PostRequest('Comments/admin/unblock/' + this.getPost()?.UserItemId)
                    .execute();
                this.getPost().IsBlockComment = false;
                this.getToastManager().success(`Đã mở khóa bình luận!`);
                this.forceUpdateCommentList?.();
                await this.updateOptions();
            } catch (error) {
                console.error(error);
                Request.resolveAxiosError(error);
            }
        },
        async resolveCopyUserItemId(){
            try {
                let itemId = this.getPost()?.UserItemId;
                if (itemId != null){
                    navigator.clipboard.writeText(itemId.toString());
                    this.getToastManager().success('Đã sao chép vào clipboard');
                }
            } catch (error) {
                console.error(error);
            }
        },
        async resolveGotoCourse(){
            try {
                let courseId = this.getPost()?.CourseId;
                if (courseId == null) return;
                this.router.push('/course/' + courseId);
            } catch (e){
                console.error(e);
            }
        },

        async updateOptions(){
            try {
                // role of user for a post: admin
                this.currentUser = await CurrentUser.getInstance();
                
                this.listOptions = [
                    this.actions.Report,
                    this.actions.History,
                    this.actions.CopyUserItemId,
                    this.actions.Delete,
                ]

                if (this.getPost().IsMarked){
                    this.listOptions.push(this.actions.Unmark);
                } else {
                    this.listOptions.push(this.actions.Mark);
                }
                if (this.getPost()?.IsBlockComment){
                    this.listOptions.push(this.actions.UnlockComment);
                } else {
                    this.listOptions.push(this.actions.LockComment);
                }

                if (this.getPost()?.PostType === myEnum.EPostType.Question){
                    let question = this.getPost();
                    if (question?.Privacy == myEnum.EPrivacy.Private
                        && question?.CourseId != null){
                        this.listOptions.push(this.actions.GotoCourse);
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
            // for listen the change of inject:
            dCommentProvider: this.commentProvider,
            post: null,
            router: useRouter(),
            isLesson: null,
            currentUser: null,
            postResource: 'Bài giảng',
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
                Comment: 3,
                Mark: 4,
                Unmark: 5,
                Report: 6,
                History: 7,
                Complete: 8,
                Uncomplete: 9,
                LockComment: 10,
                UnlockComment: 11,
                CopyUserItemId: 12,
                GotoCourse: 13,
            },
            listOptions: [],
            options: {
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
        getPost: {},
        getCourse: { default: () => null },
        getPopupManager: {},
        getToastManager: {},
        onPostDeleted: { default: null }
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