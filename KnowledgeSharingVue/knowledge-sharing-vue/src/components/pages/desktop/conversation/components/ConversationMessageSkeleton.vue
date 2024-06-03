

<template>
    <div class="p-conversation-message-frame">

        <div class="p-conversation-message-other">
            <div class="p-cmo-left">
                <div style="width: 30px; height: 30px; border-radius: 100%;" class="skeleton">
                </div>
            </div>
            <div class="p-cmo-right">
                <div class="p-cmo-content-frame skeleton"
                    style="height: 30px; width: 170px; padding: 0px"
                ></div>
            </div>
        </div>
    
        <div class="p-conversation-message-other">
            <div class="p-cmo-left">
                <div style="width: 30px; height: 30px; border-radius: 100%;" class="skeleton">
                </div>
            </div>
            <div class="p-cmo-right">
                <div class="p-cmo-content-frame skeleton"
                    style="height: 30px; width: 75px; padding: 0px"
                ></div>
            </div>
        </div>
    
        <div class="p-conversation-message-me">
            <div class="p-cmm-left">
                <div class="p-cmm-content-frame skeleton"
                    style="height: 30px; width: 160px; padding: 0px"
                ></div>
            </div>
            <div class="p-cmm-right">
                <div style="width: 30px; height: 30px; border-radius: 100%; " class="skeleton">
                </div>
            </div>
        </div>
    
        <div class="p-conversation-message-other">
            <div class="p-cmo-left">
                <div style="width: 30px; height: 30px; border-radius: 100%;" class="skeleton">
                </div>
            </div>
            <div class="p-cmo-right">
                <div class="p-cmo-content-frame skeleton"
                    style="height: 30px; width: 150px; padding: 0px"
                ></div>
            </div>
        </div>
        
    </div>
</template>



<script>
export default {
    name: 'ConversationMessageSkeleton',
}

</script>


<style scoped>

.p-conversation-message-frame {
    width: 100%;
    height: fit-content;
    display: flex;
    flex-direction: column;
    gap: 8px;
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
}

.p-cmm-left {
    width: 0;
    flex-grow: 1;
    max-width: 70%;
    display: flex;
    flex-flow: row-reverse nowrap;
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
    background-color: var(--primary-color-200);
    padding: 8px;
    border-radius: 8px;
    width: 100%;
    max-width: 100%;
    
}

.p-cmo-content-frame {
    background-color: var(--grey-color-200);
    padding: 8px;
    border-radius: 8px;
    width: 100%;
    max-width: 100%;
}

</style>

