

<template>
    <m-context-popup>
        <template #popupContextMask>
            <MActionIcon fa="ellipsis-h" :onclick="()=>{}"
                :iconStyle="iconStyle" :containerStyle="containerStyle" />
        </template>

        <template #popupContextContent>
            <div class="p-cmc-content">
                <template v-for="(option) in listOptions" :key="option?.id">
                    <div class="p-cmc-option" @:click="options[option].onClick">
                        <MIcon :style="iconStyle" :fa="options[option].fa"> </MIcon>
                        <span> {{ options[option].label }} </span>
                    </div>
                </template>
            </div>
        </template>

    </m-context-popup>
</template>

<script>
import MContextPopup from '@/components/base/popup/MContextPopup.vue';
import CurrentUser from '@/js/models/entities/current-user';
// import { MyRandom } from '@/js/utils/myrandom';
export default {
    name: "CommentMenuContext",
    props: {
        onEdit: {},
        onDelete: {},
        onReply: {},
        onToggleInformation: {}
    },
    components: {
        MContextPopup
    },
    async mounted(){
        try {
            this.comment = this.getComment();
            await this.updateOptions();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async resolveEditComment(){
            if (this.onEdit){
                this.onEdit();
            }
        },
        async resolveDeleteComment(){
            if (this.onDelete){
                this.onDelete();
            }
        },
        async resolveReplyComment(){
            if (this.onReply){
                this.onReply();
            }
        },
        async resolveToggleInformation(){
            if (this.onToggleInformation){
                await this.onToggleInformation();
                await this.updateOptions();
            }
        },

        async updateOptions(){
            try {
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.listOptions = [this.actions.Reply];
                }
                else if (this.currentUser.UserId == this.getComment()?.UserId && this.getComment()?.UserId != null){
                    this.listOptions = [
                        this.actions.Reply,
                        this.actions.Edit,
                        this.actions.Delete
                    ];
                }
                else if (this.currentUser.UserId == this.getPost()?.UserId && this.getComment()?.UserId != null){
                    this.listOptions = [
                        this.actions.Reply,
                        this.actions.Delete
                    ];
                }
                if (this.getComment()?.isHideCommentInformation == true){
                    this.listOptions.push(this.actions.ShowInformation);
                } else {
                    this.listOptions.push(this.actions.HideInformation);
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
            isHideCommentInformation: null,
            comment: null,
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
                Reply: 3,
                HideInformation: 4,
                ShowInformation: 5
            },
            listOptions: [],
            options: {
                [1]: {
                    id: 1,
                    label: 'Chỉnh sửa',
                    fa: 'pencil',
                    onClick: this.resolveEditComment
                }, 
                [2]: {
                    id: 2,
                    label: 'Xóa',
                    fa: 'trash-can',
                    onClick: this.resolveDeleteComment,
                },
                [3]: {
                    id: 3,
                    label: 'Phản hồi',
                    fa: 'share',
                    onClick: this.resolveReplyComment,
                },
                [4]: {
                    id: 4,
                    label: 'Ẩn thông tin',
                    fa: 'eye-slash',
                    onClick: this.resolveToggleInformation,
                },
                [5]: {
                    id: 5,
                    label: 'Hiển thị thông tin',
                    fa: 'eye',
                    onClick: this.resolveToggleInformation,
                }
            }
        }
    },
    inject: {
        getLanguage: {},
        getComment: {},
        getPost: {}
    }
}

</script>

<style scoped>

.p-cmc-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-cmc-option{
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

.p-cmc-option :first-child{
    width: 20px;
}

.p-cmc-option:hover{
    background-color: var(--primary-color-100);
}

.p-cmc-option:active{
    background-color: var(--primary-color-200);
}

</style>