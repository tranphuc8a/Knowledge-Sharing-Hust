

<template>
    <TooltipMenuContext :options="getOptions()">
        <MButton 
            label="Thêm bạn bè"
            :onclick="resolveAddFriend"
            :buttonStyle="buttonStyle"
            fa="user-plus" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </TooltipMenuContext>
</template>



<script>
import TooltipMenuContext from '@/components/base/tooltip/TooltipMenuContext.vue';
import MButton from '@/components/base/buttons/MButton';
import { Request, PostRequest } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'NotRelationButton',
    components: {
        MButton, TooltipMenuContext
    },
    props: {
    },
    data(){
        return {
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
            
            },
        }
    },
    mounted(){

    },
    methods: {

        getOptions(){
            return [
                {
                    fa: 'user-tag',
                    onclick: this.resolveFollow.bind(this),
                    label: 'Theo dõi'
                }
            ]
        },


        async resolveAddFriend(){
            try {
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/add-friend/' + this.getUser().UserId)
                    .execute();
                //  success:
                this.getUser().UserRelationType = myEnum.EUserRelationType.Requester;
                this.forceUpdateUserRelationButton();
            } catch (error) {
                Request.resolveAxiosError(error);
            }
        },

        async resolveFollow(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/follow/' + this.getUser().UserId)
                    .execute();
                //  success:
                this.getUser().UserRelationType = myEnum.EUserRelationType.Follower;
                this.forceUpdateUserRelationButton();
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.$refs['button']?.normal();
            }
        },
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getUser: {},
        forceUpdateUserRelationButton: {}
    }
}

</script>

<style scoped>

</style>

