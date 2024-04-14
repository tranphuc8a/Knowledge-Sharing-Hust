<template>
    <div class="p-postcard-header">
        <div class="p-header-left">
            <TooltipUserAvatar :user="lesson?.User" :size="36" />
            <div class="p-lesson-infor">
                <div class="p-lesson-owner-name">
                    <TooltipUsername :user="lesson?.User" />
                </div>
                <div class="p-lesson-detail-infor">
                    <VisualizedDatetime :datetime="lesson?.CreatedTime" />
                    <VisualizedPrivacy :privacy="lesson?.Privacy" />
                </div>
            </div>
        </div>
        <div class="p-header-right">
            <MContextMenu>
                <MActionIcon fa="ellipsis-h" />
            </MContextMenu>
        </div>
    </div>
</template>

<script>
import MActionIcon from '@/components/base/icons/MActionIcon'
import MContextMenu from '@/components/base/context-menu/MContextMenu.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import VisualizedDatetime from '@/components/base/visualized-components/VisualizedDatetime.vue';
import VisualizedPrivacy from '@/components/base/visualized-components/VisualizedPrivacy.vue';

export default {
    name: 'PostCardHeader',
    data(){
        return{
            
        }
    },
    components: {
        MActionIcon, TooltipUserAvatar, TooltipUsername, VisualizedDatetime, VisualizedPrivacy, MContextMenu
    },
    methods: {

    },
    props: {
        post: {
            type: Object,
            required: true
        }
    }
}
</script>

<style scoped>
.p-postcard-header{
    padding: 0 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-header-left{
    display: flex;
    justify-self: flex-start;
    flex-flow: row nowrap;
    align-items: center;
    gap: 8px;
}

.p-lesson-infor{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
}

.p-lesson-detail-infor{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}
</style>
