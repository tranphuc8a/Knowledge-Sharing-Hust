

<template>
    <div>
        <MSecondaryButton 
            label="Nhắn tin"
            :onclick="resolveClickButton"
            :buttonStyle="buttonStyle"
            fa="comment" family="fas" :iconStyle="iconStyle"
        />
    </div>
</template>



<script>
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';
import ResponseConversationModel from '@/js/models/api-response-models/response-conversation-model';
import { PostRequest, Request } from '@/js/services/request';

export default {
    name: 'MessageButton',
    components: {
        MSecondaryButton
    },
    props: {
    },
    data(){
        return {
            iconStyle: {
                fontSize: '20px'
            },
            buttonStyle: {
            
            },
        }
    },
    mounted(){

    },
    methods: {
        async getConversation(){
            try {
                let userId = this.getUser()?.UserId;
                if (userId == null) return null;
                let res = await new PostRequest('Conversations/with/' + userId).execute();
                let body = await Request.tryGetBody(res);
                let conversation = new ResponseConversationModel().copy(body);
                return conversation;
            } catch (e) {
                Request.resolveAxiosError(e);
                return null;
            }
        },

        async resolveClickButton(){
            try {
                let currentConversation = await this.getConversationPopup().getCurrentConversation();
                let userId = this.getUser()?.UserId;
                if (currentConversation != null){
                    let participants = currentConversation.Participants;
                    if (participants != null && participants.some(function(item){ return item.UserId == userId })){
                        if (this.getConversationPopup) {
                            this.getConversationPopup().show();
                        }
                        return;
                    }
                }
                let conversation = await this.getConversation();
                if (conversation == null) return;
                if (this.getConversationPopup) {
                    this.getConversationPopup().show(conversation);
                }
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getConversationPopup: {
            default: null
        },
        getUser: {},
        getCurrentUser: {},
    },
}

</script>

<style scoped>

</style>

