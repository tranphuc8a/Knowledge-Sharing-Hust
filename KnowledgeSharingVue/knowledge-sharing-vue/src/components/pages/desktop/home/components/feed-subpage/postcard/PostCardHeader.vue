<template>
    <div class="p-postcard-header">
        <div class="p-header-left">
            <TooltipUserAvatar :user="getPost()?.User" :size="36" />
            <div class="p-post-infor">
                <div class="p-post-owner-name">
                    <TooltipUsername :user="getPost()?.User" />
                </div>
                <div class="p-post-detail-infor">
                    <VisualizedDatetime :datetime="getPost()?.CreatedTime" />
                    <VisualizedPrivacy :privacy="getPost()?.Privacy" />
                </div>
            </div>
        </div>
        <div class="p-header-right">
            <PostCardMenuContext />
        </div>
    </div>
</template>

<script>
import PostCardMenuContext from './PostCardMenuContext.vue';
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
        PostCardMenuContext,
        TooltipUserAvatar, TooltipUsername, VisualizedDatetime, VisualizedPrivacy
    },
    methods: {

    },
    inject: {
        getPost: {}
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

.p-post-infor{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
}

.p-post-detail-infor{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}
</style>
