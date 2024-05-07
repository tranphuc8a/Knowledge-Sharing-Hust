

<template>
    <MMenuContextPopup :options="getOptions()">
        <MCancelButton 
            label="Đang theo dõi"
            :onclick="()=>{}"
            :buttonStyle="buttonStyle"
            fa="user-tag" family="fas" :iconStyle="iconStyle"
            ref="button"
        />
    </MMenuContextPopup>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import MCancelButton from '@/components/base/buttons/MCancelButton';
import { PostRequest, Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';


export default {
    name: 'FollowerButton',
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
                    fa: 'user-plus',
                    onclick: this.resolveAddFriend.bind(this),
                    label: 'Thêm bạn bè'
                }, {
                    fa: 'user-minus',
                    onclick: this.resolveUnFollow.bind(this),
                    label: 'Bỏ theo dõi'
                }
            ]
        },

        async resolveAddFriend(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/add-friend/' + this.getUser().UserId)
                    .execute();
                //  success:
                this.getUser().UserRelationType = myEnum.EUserRelationType.Requester;
                this.forceUpdateUserRelationButton();
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.$refs['button']?.normal();
            }
        },

        async resolveUnFollow(){
            try {
                this.$refs['button']?.loading();
                if (this.getUser() == null) return;
                await new PostRequest('UserRelations/unfollow/' + this.getUser().UserId)
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

