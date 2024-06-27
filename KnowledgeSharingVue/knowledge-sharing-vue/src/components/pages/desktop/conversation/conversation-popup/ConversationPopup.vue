

<template>
    <!-- Fixed and decorbackground -->
    <div class="p-conversation-popup-background" v-show="!dIsShow" v-if="isLive">
        <!-- Position and size of popup -->
        <div class="p-conversation-popup-frame p-conversation-popup-hide-frame" @:click="resolveExpandConversation">
            <!-- Content popup -->
            <div class="p-conversation-popup">
                <!-- popup-header -->
                <div class="p-cp-header">
                    <!-- conversation name -->
                    <div class="p-cp-name card-subheading">
                        <EllipsisText :text="conversation?.ConversationName ?? 'Nhắn tin'" :max-line="1" />
                    </div>

                    <!-- action buttons -->
                    <div class="p-cp-buttons">
                        <!-- Close button -->
                        <span>
                            <MIcon title="Đóng cuộc trò chuyện"
                                fa="xmark"
                                @:click="resolveCloseConversationPopup"
                                :style="iconStyle"
                            />
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Fixed and decorbackground -->
    <div class="p-conversation-popup-background" v-show="dIsShow" v-if="isLive">
        <!-- Position and size of popup -->
        <div class="p-conversation-popup-frame">
            <!-- Content popup -->
            <div class="p-conversation-popup">
                <!-- popup-header -->
                <div class="p-cp-header">
                    <!-- conversation name -->
                    <div class="p-cp-name card-subheading">
                        <EllipsisText :text="conversation?.ConversationName ?? 'Nhắn tin'" :max-line="1" />
                    </div>

                    <!-- action buttons -->
                    <div class="p-cp-buttons">
                        <!-- Hide button -->
                        <span>
                            <MIcon title="Ẩn cuộc trò chuyện"
                                fa="minus"
                                @:click="resolveHideConversationPopup"
                                :style="iconStyle"
                            />
                        </span>

                        <!-- Close button -->
                        <span>
                            <MIcon title="Đóng cuộc trò chuyện"
                                fa="xmark"
                                @:click="resolveCloseConversationPopup"
                                :style="iconStyle"
                            />
                        </span>
                    </div>
                </div>

                <!-- popup-body -->
                <div class="p-cp-body" v-if="isShowListMessage">
                    <!-- List messages -->
                    <ConversationListMessage  ref="list-message" />
                </div>

                <!-- popup-footer -->
                <div class="p-cp-footer">
                    <!-- Enter message element -->
                    <ConversationEnterMessage :on-send-message="resolveOnSendMessage" />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { Validator } from '@/js/utils/validator';
import ConversationEnterMessage from '../components/ConversationEnterMessage.vue';
import WebSocketProxy from '@/js/services/web-socket';
import ResponseConversationModel from '@/js/models/api-response-models/response-conversation-model';
import ViewMessage from '@/js/models/views/view-message';
import EllipsisText from '@/components/base/text/EllipsisText.vue';
import ConversationListMessage from '../components/ConversationListMessage.vue';
// import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ConversationPopup',
    components: {
        ConversationEnterMessage,
        EllipsisText,
        ConversationListMessage,
    },
    props: {
        isShow: {
            default: false,
        }
    },
    data(){
        return {
            conversation: null,
            dIsShow: this.isShow,
            isLive: false,
            isShowListMessage: false,
            soundResource: require('@/assets/sound/noti-sound.mp3'),
            notificationSound: [],
            // userConversation: null,
            circleIndex: 0,
            iconStyle: { fontSize: '20px' },

            socketUrl: 'Sockets/live-chat-socket',
            websocket: null,
            listMessages: [],
        }
    },
    async created(){
    },
    async mounted(){
        try {
            this.notificationSound = [1, 2, 3, 4, 5].map(() => new Audio(this.soundResource));
        } catch (error) {
            console.error(error);
        }
    },
    methods: {
        async connectSocket(){
            try {
                this.websocket = new WebSocketProxy(this.socketUrl);
                this.websocket.registerOnMessage(this.resolveOnReceiveMessage.bind(this));
            } catch (error) {
                console.error(error);
            }
        },

        async resolveOnSendMessage(text){
            try {
                // let message = new ViewMessage();
                // let currentUser = await CurrentUser.getInstance();
                // message.copy(currentUser);
                // message.Content = text;
                // message.MessageId = new Date().getTime();
                // this.$refs['list-message'].appendMessage(message);
                // this.playNotificationSound();

                if (Validator.isEmptyOrSpace(text)) return;
                if (Validator.isEmptyOrSpace(this.conversation?.ConversationId)) return;
                let payload = {
                    ConversationId: this.conversation.ConversationId,
                    Content: text,
                    ReplyId: null,
                };
                this.websocket.send(payload);
            } catch (error) {
                console.error(error);
            }
        },

        async resolveOnReceiveMessage(message){
            try {
                if (Validator.isEmptyOrSpace(message?.ConversationId)) return;
                if (Validator.isEmptyOrSpace(this.conversation?.ConversationId)) return;
                let viewMessage = await new ViewMessage().copy(message);
                // append message to list messages
                if (this.$refs['list-message'] != null)
                    this.$refs['list-message'].appendMessage(viewMessage);
                this.playNotificationSound();
            } catch (error) {
                console.error(error);
            }
        },

        async forceUpdateListMessages(){
            try {
                let that = this;
                this.isShowListMessage = false;
                this.$nextTick(() => {
                    that.isShowListMessage = true;
                });
            } catch (error) {
                console.error(error);
            }
        },

        async refreshNewConversation(conversation){
            try {
                if (Validator.isEmptyOrSpace(conversation)) return;
                this.conversation = new ResponseConversationModel().copy(conversation);
                // let currentUser = await CurrentUser.getInstance();
                // this.userConversation = conversation.Participants.find(function (participant) {
                //     return participant.UserId == currentUser.UserId;
                // });
                this.forceUpdateListMessages();                
            } catch (error) {
                console.error(error);
            }
        },

        async playNotificationSound(){
            try {
                this.notificationSound[this.circleIndex].play();
                this.circleIndex = (this.circleIndex + 1) % this.notificationSound.length;
            } catch (error) {
                console.error(error);
            }
        },

        async resolveHideConversationPopup(){
            try {
                this.hide();
            } catch (error) {
                console.error(error);
            }
        },

        async resolveCloseConversationPopup(){
            try {
                this.hide();
                this.isLive = false;
                this.websocket.close();
            } catch (error) {
                console.error(error);
            }
        },

        async getCurrentConversation(){
            try {
                return this.conversation;
            } catch (error) {
                console.error(error);
            }
        },

        async resolveExpandConversation(){
            try {
                this.show();
            } catch (error) {
                console.error(error);
            }
        },

        async show(conversation){
            try {
                this.playNotificationSound();
                if (conversation != null){
                    this.refreshNewConversation(conversation);
                }
                this.dIsShow = true;
                if (!this.isLive){
                    this.isLive = true;
                    this.connectSocket();
                }
            } catch (error) {
                console.error(error);
            }
        },
        async hide(){
            try {
                this.dIsShow = false;
            } catch (error) {
                console.error(error);
            }
        }
    },
    inject: {
    },
    provide(){
        return {
            getConversation: () => this.conversation,
        }
    }
}

</script>


<style scoped>

.p-conversation-popup-background {
    width: 0;
    height: 0;
}

.p-conversation-popup-frame {
    position: fixed;
    bottom: 0;
    right: 96px;
    width: 400px;
    height: 550px;
    background-color: var(--primary-color-100);
    border-radius: 8px;
    box-shadow: 0 0 8px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column;
    overflow: hidden;
}

@media screen and (max-width: 600px) {
    .p-conversation-popup-frame {
        left: 16px;
        right: 16px;
        width: calc(100vw - 32px);
        height: calc(100vh - 32px);
    }
}

.p-conversation-popup-hide-frame {
    height: fit-content;
    background-color: var(--primary-color-200);
    cursor: pointer;
}

.p-conversation-popup {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
}

.p-cp-header {
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    padding: 16px 16px;
}

.p-cp-name {
    width: 100%;
    height: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-cp-buttons {
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
}

.p-cp-buttons > * {
    cursor: pointer;
    font-size: 20px;
}

.p-cp-body {
    width: 100%;
    height: 0;
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    overflow: hidden;
}

.p-cp-footer {
    width: 100%;
    height: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    padding: 16px;
}

</style>

