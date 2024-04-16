

<template>
    <m-context-popup>
        <template #popupContextMask>
            <MActionIcon fa="ellipsis-h" :onclick="()=>{}"
                :iconStyle="iconStyle" :containerStyle="containerStyle" />
        </template>

        <template #popupContextContent>
            <div class="p-cmc-content">
                <template v-for="(option, index) in listOptions" :key="index">
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
export default {
    name: "CommentMenuContext",
    props: {
        onEdit: {},
        onDelete: {},
        onReply: {}
    },
    components: {
        MContextPopup
    },
    async mounted(){
        try {
            this.currentUser = await CurrentUser.getInstance();
            if (this.currentUser == null){
                this.listOptions = [this.actions.Reply];
            }
            if (this.currentUser.UserId == this.comment?.UserId && this.comment?.UserId != null){
                this.listOptions = [
                    this.actions.Reply,
                    this.actions.Edit,
                    this.actions.Delete
                ];
            }
            if (this.currentUser.UserId == this.post?.UserId && this.comment?.UserId != null){
                this.listOptions = [
                    this.actions.Reply,
                    this.actions.Delete
                ];
            }
        }
        catch (error){
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
        }
    },
    data() {
        return {
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
                Reply: 3
            },
            listOptions: [],
            options: {
                [1]: {
                    label: 'Chỉnh sửa',
                    fa: 'pencil',
                    onClick: this.resolveEditComment,
                }, 
                [2]: {
                    label: 'Xóa',
                    fa: 'trash-can',
                    onClick: this.resolveDeleteComment,
                },
                [3]: {
                    label: 'Phản hồi',
                    fa: 'share',
                    onClick: this.resolveReplyComment,
                }
            }
        }
    },
    inject: {
        inject: {},
        comment: {},
        post: {}
    }
}

</script>

<style scoped>

.p-cmc-content{
    padding: 18px 0px;
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