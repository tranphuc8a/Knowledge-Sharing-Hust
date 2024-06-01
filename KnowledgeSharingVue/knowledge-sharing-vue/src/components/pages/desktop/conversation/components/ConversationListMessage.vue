

<template>
    <div class="p-conversation-list-message">
        <div class="p-plm-scrollable"
            @:scroll="resolveOnScroll"
            ref="scroll-container"
        >
            <div class="p-plm-empty" v-show="isOutOfMessage && !(listMessages.length > 0)">
                <NotFoundPanel
                    text="Không có tin nhắn"
                />
            </div>

            <div class="p-plm-list-message">
                <ConversationMessage
                    v-for="message in listMessages.slice().reverse()"
                    :key="message.MessageId"
                    :viewMessage="message"
                />
            </div>

            <ConversationMessageSkeleton v-if="!isOutOfMessage" />
        </div>
    </div>
</template>



<script>
import ConversationMessage from './ConversationMessage.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import Debounce from '@/js/utils/debounce';
import ConversationMessageSkeleton from './ConversationMessageSkeleton.vue';
import { GetRequest, Request } from '@/js/services/request';
import ResponseMessageModel from '@/js/models/api-response-models/response-message-model';


export default {
    name: 'ConversationListMessage',
    components: {
        ConversationMessage,
        NotFoundPanel,
        ConversationMessageSkeleton,
    },
    props: {
    },
    data(){
        return {
            listMessages: [],
            throttleScroll: Debounce.throttle(this.resolveOnScroll.bind(this), 1000),
            isOutOfMessage: false,
            isWorking: false,
            conversation: null,
        }
    },
    async mounted(){
        this.reloadLisMessage();
    },
    methods: {
        async loadMoreMessages(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let conversationId = this.getConversation()?.ConversationId;
                if (conversationId == null) return;

                // prepare:
                let limit = 10;
                let offset = this.listMessages.length;
                let url = 'Conversations/messages/' + conversationId;
                let res = await new GetRequest(url)
                    .setParams({ limit, offset })
                    .execute();
                let body = Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // get Data:
                let tempListMessage = body.map(function(item){
                    return new ResponseMessageModel().copy(item);
                }).sort(function(a, b){
                    let acreatedTime = new Date(a.CreatedTime).getTime();
                    let bcreatedTime = new Date(b.CreatedTime).getTime();
                    return - acreatedTime + bcreatedTime;
                });
                if (tempListMessage.length < limit){
                    this.isOutOfMessage = true;
                }
                if (tempListMessage.length > 0){
                    this.listMessages = this.listMessages.concat(tempListMessage);
                }

            } catch (error) {
                console.error(error);
            } finally {
                this.isWorking = false;
            }
        },


        async appendMessage(message){
            try {
                let newMessage = new ResponseMessageModel().copy(message);
                this.listMessages.unshift(newMessage);
                this.scrollTopBottom();
            } catch (error) {
                console.error(error);
            }
        },


        async resolveOnScroll(){
            try {
                if (this.isWorking || this.isOutOfMessage) return;
                let scrollContainer = this.$refs['scroll-container'];
                if (scrollContainer == null) return;
                let scrollTop = scrollContainer.scrollTop;
                let scrollHeight = scrollContainer.scrollHeight;
                let clientHeight = scrollContainer.clientHeight;
                let leftHeight = scrollHeight - (scrollTop + clientHeight);

                let averageHeight = 150;
                let leftNumberElements = 5;
                if (leftHeight < averageHeight * leftNumberElements){
                    this.loadMoreMessages();
                }
            } catch (error) {
                console.error(error);
            }
        },


        async reloadLisMessage(){
            try {
                this.isWorking = false;
                this.isOutOfMessage = false;
                this.listMessages = [];
                this.conversation = await this.getConversation();
                if (this.conversation == null){
                    this.isOutOfMessage = true;
                    return;
                }
                this.loadMoreMessages();
                let that = this;
                this.$nextTick(() => {
                    that.scrollTopBottom();
                });
            } catch (error) {
                console.error(error);
            }
        },

        async scrollTopBottom(){
            try {
                let that = this;
                this.$nextTick(() => {
                    let scrollContainer = that.$refs['scroll-container'];
                    if (scrollContainer == null) return;
                    scrollContainer.scrollTop = scrollContainer.scrollHeight;
                });
            } catch (error) {
                console.error(error);
            }
        }
    },
    inject: {
        getConversation: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-conversation-list-message {
    display: flex;
    flex-direction: column;
    height: 100%;
    width: 100%;
    background-color: #f5f5f5;
    overflow: hidden;
}

.p-plm-scrollable {
    display: flex;
    flex-direction: column-reverse;
    height: 100%;
    max-height: 100%;
    width: 100%;
    overflow-y: scroll;
    padding: 8px;
}

.p-plm-scrollable > :first-child {
    margin-top: auto !important;
}

.p-plm-empty {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 8px;
    height: 100%;
    width: 100%;
}

.p-plm-list-message {
    display: flex;
    flex-direction: column;
    gap: 8px;
    width: 100%;
}
</style>

