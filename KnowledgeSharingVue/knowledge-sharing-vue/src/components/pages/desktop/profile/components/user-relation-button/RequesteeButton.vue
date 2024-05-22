

<template>
    <MMenuContextPopup :options="getOptions()">
        <MButton 
            label="Phản hồi lời mời"
            :onclick="null"
            :buttonStyle="buttonStyle"
            fa="user-cog" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </MMenuContextPopup>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import MButton from '@/components/base/buttons/MButton';
import { PostRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'RequesteeButton',
    components: {
        MMenuContextPopup,
        MButton,
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
                    fa: 'user-check',
                    onclick: this.resolveAcceptRequest.bind(this),
                    label: 'Chấp nhận'
                }, {
                    fa: 'user-minus',
                    onclick: this.resolveDenyRequest.bind(this),
                    label: 'Từ chối'
                }
            ]
        },

        async resolveAcceptRequest(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/confirm-friend/' + this.getUser().UserRelationId + "/true")
                    .execute();
                //  success:
                this.getUser().UserRelationType = myEnum.EUserRelationType.Friend;
                this.forceUpdateUserRelationButton();
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.$refs['button']?.normal();
            }
        },

        async resolveDenyRequest(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/confirm-friend/' + this.getUser().UserRelationId + "/false")
                    .execute();
                //  success:
                this.getUser().UserRelationType = myEnum.EUserRelationType.NotInRelation;
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

