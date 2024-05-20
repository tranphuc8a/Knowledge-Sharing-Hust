

<template>
    <div class="p-profile-friend-item-card">
        <div class="p-pfc-card card">
            <div class="p-pfc__left">
                <div class="p-pfc__avatar">
                    <TooltipUserAvatar :user="dUser" :size="50" />
                </div>
                <div class="p-pfc__userinfo">
                    <div class="p-pfc__fullname">
                        <TooltipUsername :user="dUser" :style="usernameStyle" />
                    </div>
                    <div class="p-pfc__username">
                        @{{ dUser?.Username ?? "username" }}
                    </div>
                </div>

            </div>
            <div class="p-pfc__right">
                <div class="p-pfc__button">
                    <UserRelationButton />
                </div>
            </div>

        </div>
    </div>
</template>



<script>
import UserRelationButton from '../user-relation-button/UserRelationButton.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';

export default {
    name: 'ProfileFriendCard',
    components: {
        UserRelationButton,
        TooltipUserAvatar,
        TooltipUsername
    },
    props: {
        user: {
            type: Object,
            required: true,
        }
    },
    watch: {
        user: {
            handler(){
                this.refresh();
            },
            deep: true,
        }
    },
    data(){
        return {
            dUser: null,
            usernameStyle: {
                fontSize: '18px',
            }
        }
    },
    async mounted(){
        this.dUser = this.user;
    },
    methods: {
        async refresh(){
            try {
                this.dUser = this.user;
            } catch (e){
                console.error(e);
            }
        
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return {
            getUser: () => this.dUser,
        }
    }
}

</script>


<style scoped>

.p-profile-friend-item-card{
    width: 100%;
}

.p-pfc-card{
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-pfc__left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}
.p-pfc__right{
    display: flex;
}

.p-pfc__userinfo{
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-around;
    align-items: flex-start;
    gap: 4px;
}

.p-pfc__button{
    align-self: center;
    width: fit-content;
}

</style>

