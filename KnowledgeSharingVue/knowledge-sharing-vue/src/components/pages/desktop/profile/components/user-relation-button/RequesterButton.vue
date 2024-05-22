

<template>
    <MMenuContextPopup :options="getOptions()">
        <MButton 
            label="Đã gửi lời mời"
            :onclick="null"
            :buttonStyle="buttonStyle"
            fa="user-tag" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </MMenuContextPopup>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import MButton from '@/components/base/buttons/MButton';
import { DeleteRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'RequesterButton',
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
                    fa: 'user-minus',
                    onclick: this.resolveDeleteRequest.bind(this),
                    label: 'Hủy lời mời'
                }
            ]
        },

        async resolveDeleteRequest(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new DeleteRequest('UserRelations/delete-request/' + this.getUser().UserRelationId)
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

