

<template>
    <div class="p-conversation-message-frame">
        <div class="p-conversation-message-me" v-if="isMyMessage">
            <div class="p-cmm-left">
                <div class="p-cmm-content-frame">
                    <LatexMarkdownRender
                        :markdown-content="viewMessage?.Content ?? ''"
                    />
                </div>
                
            </div>
            <!-- <div class="p-cmm-right">
                <span>
                    <TooltipUserAvatar
                        :user="user"
                        :size="30"
                    />
                </span>
            </div> -->
        </div>

        <div class="p-conversation-message-other" v-if="!isMyMessage">
            <div class="p-cmo-left">
                <span>
                    <TooltipUserAvatar
                        :user="user"
                        :size="30"
                    />
                </span>
            </div>
            <div class="p-cmo-right">
                <div class="p-cmo-content-frame">
                    <LatexMarkdownRender
                        :markdown-content="viewMessage?.Content ?? ''"
                    />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import CurrentUser from '@/js/models/entities/current-user';
import LatexMarkdownRender from '@/components/base/markdown/LatexMarkdownRender.vue';

export default {
    name: 'ConversationMessage',
    components: {
        TooltipUserAvatar,
        LatexMarkdownRender,
    },
    props: {
        viewMessage: {
            required: true,
        }
    },
    data(){
        return {
            user: null,
            isMyMessage: false,
            currentUser: null,
        }
    },
    async created(){
        this.currentUser = await CurrentUser.getInstance();
        this.user = this.viewMessage?.getUser?.();
        this.isMyMessage = this.currentUser?.UserId == this.user?.UserId;
    },
    async mounted(){
    },
    methods: {

    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-conversation-message-frame {
    width: 100%;
    height: fit-content;
    display: flex;
    flex-direction: column;
}

.p-conversation-message-me {
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: stretch;
    gap: 8px;
}

.p-conversation-message-other {
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 8px;
    text-align: left;
}

.p-cmm-left {
    width: 0;
    flex-grow: 1;
    max-width: 70%;

    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: stretch;
}

.p-cmm-right {
    width: fit-content;
}

.p-cmo-left {
    width: fit-content;
}

.p-cmo-right {
    width: 0;
    flex-grow: 1;
    max-width: 70%;
}

.p-cmm-content-frame {
    background-color: var(--primary-color-100);
    padding: 8px;
    border-radius: 8px;
    width: fit-content;
    max-width: 100%;
    
}

.p-cmo-content-frame {
    background-color: var(--grey-color-400);
    padding: 8px;
    border-radius: 8px;
    width: fit-content;
    max-width: 100%;
}

</style>

