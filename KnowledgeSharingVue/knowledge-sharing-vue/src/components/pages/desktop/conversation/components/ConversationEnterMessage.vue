

<template>
    <div class="p-enter-message"
        @keydown.shift.enter.prevent.stop="resolvePressShiftEnter" v-show="currentUser != null"
    >
        <div class="p-enter-message-avatar">
            <UserAvatar :user="currentUser" :size="36" />
        </div>
        <div class="p-enter-message-textarea">
            <MTextArea
                ref="textarea" placeholder="Nhập tin nhắn"
                :is-show-title="false" :is-show-error="false" 
                max-height="75px" rows="1"
                />
        </div>
        <div class="p-enter-message-submit" ref="submit">
            <MActionIcon fa="paper-plane" title="Gửi tin nhắn"
                :containerStyle="{width: '36px', height: '36px'}"
                :onclick="resolveSubmitMessage" 
                ref="actionicon"/>
        </div>
    </div>

</template>



<script>
import MTextArea from '@/components/base/inputs/MTextArea.vue';
import UserAvatar from '@/components/base/avatar/UserAvatar.vue';
import { Validator } from '@/js/utils/validator';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ConversationEnterMessage',
    components: {
        MTextArea,
        UserAvatar,
    },
    props: {
        onSendMessage: {
            required: true,
        },
    },
    data(){
        return {
            currentUser: null,
            isWorking: false,
        }
    },
    async created(){
        this.currentUser = await CurrentUser.getInstance();
    },
    async mounted(){
    },
    methods: {
        async resolvePressShiftEnter(){
            try {
                if (this.$refs.actionicon != null){
                    await this.$refs.actionicon.resolveOnclick();
                }
            } catch (error) {
                console.error(error);
            }
        },
        async resolveSubmitMessage(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                let text = await this.$refs.textarea.getValue();
                if (Validator.isEmptyOrSpace(text)) return;

                if (this.onSendMessage != null){
                    await this.onSendMessage(text);
                }
                this.$refs.textarea.setValue('');
            } catch (error) {
                console.error(error);
            } finally {
                this.isWorking = false;
            }
        },
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

.p-enter-message{
    width: 100%;
    height: auto;
    display: flex;
    flex-flow: row nowrap;
    justify-self: space-between;
    align-items: flex-start;
    gap: 8px;
}

.p-enter-message > :first-child,
.p-enter-message > :last-child{
    align-self: stretch;
}

.p-enter-message > :last-child{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: flex-end;
}

.p-enter-message-textarea{
    width: 0;
    flex: 1;
}


</style>

