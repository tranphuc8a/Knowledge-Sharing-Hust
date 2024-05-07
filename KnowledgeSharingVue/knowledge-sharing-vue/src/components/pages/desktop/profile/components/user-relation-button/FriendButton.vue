

<template>
    <MMenuContextPopup :options="getOptions()">
        <MCancelButton 
            label="Bạn bè"
            :onclick="()=>{}"
            :buttonStyle="buttonStyle"
            fa="user-friends" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </MMenuContextPopup>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import MCancelButton from '@/components/base/buttons/MCancelButton';
import { DeleteRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'FriendButton',
    components: {
        MMenuContextPopup,
        MCancelButton,
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
                    fa: 'user-times',
                    onclick: this.resolveUnfriend.bind(this),
                    label: 'Hủy kết bạn'
                }
            ]
        },

        async resolveUnfriend(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new DeleteRequest('UserRelations/delete-friend/' + this.getUser().UserId)
                    .execute();
                this.getUser().UserRelationType = myEnum.EUserRelationType.NotInRelation;
                this.forceUpdateUserRelationButton?.();
            } catch (e) {
                Request.resolveAxiosError(e);
            } finally {
                this.$refs['button']?.normal();
            }
        }
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

