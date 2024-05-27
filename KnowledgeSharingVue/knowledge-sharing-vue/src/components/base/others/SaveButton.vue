

<template>
    <div class="p-save-button" @:click="toggleSave">
        <MIcon fa="bookmark" :family="isSave ? 'fas' : 'far'" :style="iconStyle" />
    </div>
</template>



<script>
import { PostRequest } from '@/js/services/request';



export default {
    name: 'SaveButton',
    components: {
    },
    props: {
        knowledgeId: {
            required: true,
        },
        initValue: {
            type: Boolean,
            default: false,
        },
        resolveOnChange: {
            default: null
        },
        style: {
            type: Object,
            default: () => ({})
        }
    },
    data(){
        return {
            dKnowledgeId: this.knowledgeId,
            isSave: this.initValue,
            isWorking: false,
            dKnowledge: null,
            iconStyle: {
                color: 'var(--primary-color)',
                ...this.style,
            }
        }
    },
    async mounted(){
        try {
            this.dKnowledge = this.getKnowledge?.();
        } catch (e){
            console.error(e)
        }
    },
    methods: {
        async resolveOnCommitChange(isSave){
            if (this.isWorking) return;
            let oldValue = this.isSave;
            
            try {
                this.isWorking = true;
                this.isSave = isSave;
                let url = 'Knowledges/mark/' + this.dKnowledgeId;
                if (isSave === false){
                    url = 'Knowledges/unmark/' + this.dKnowledgeId;
                }
                await new PostRequest(url).execute();
                
                // success:
                let knowledge = this.getKnowledge?.();
                if (knowledge){
                    knowledge.IsMarked = isSave;
                }
                let successMessage = isSave ? 'Đã lưu thành công' : 'Đã bỏ lưu';
                this.getToastManager().success(successMessage);
            } catch (e){
                Request.resolveAxiosError(e);
                this.isSave = oldValue;
            } finally {
                this.isWorking = false;
            }
        },

        async toggleSave(){
            await this.resolveOnCommitChange(!this.isSave);
        }
    },
    inject: {
        getKnowledge: {
            default: () => null
        },
        getPopupManager: {},
        getToastManager: {},
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-save-button {
    cursor: pointer;
    width: fit-content;
}

</style>

