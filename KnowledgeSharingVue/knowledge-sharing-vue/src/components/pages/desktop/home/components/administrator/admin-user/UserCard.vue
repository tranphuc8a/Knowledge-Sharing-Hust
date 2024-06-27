

<template>
    <div class="p-user-card">
        <div class="p-uc-card card">
            <div class="p-uc__left">
                <div class="p-uc__avatar">
                    <TooltipUserAvatar :user="dUser" :size="50" />
                </div>
                <div class="p-uc__userinfo">
                    <div class="p-uc__fullname">
                        <TooltipUsername :user="dUser" :style="usernameStyle" />
                    </div>
                    <div class="p-uc__username">
                        @{{ dUser?.Username ?? "username" }}
                    </div>
                </div>

            </div>
            <div class="p-uc__right">
                <div class="p-uc__button">
                    <slot />
                </div>
            </div>

        </div>
    </div>
</template>



<script>
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';

export default {
    name: 'UserCard',
    components: {
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
            dUser: this.user,
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
    },
    provide(){
        return {
            getUser: () => this.dUser,
        }
    }
}

</script>


<style scoped>

.p-user-card{
    width: 100%;
    max-width: 100%;
}

.p-uc-card{
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-uc__left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}
.p-uc__right{
    display: flex;
}

.p-uc__userinfo{
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-around;
    align-items: flex-start;
    gap: 4px;
}

.p-uc__button{
    align-self: center;
    width: fit-content;
}

</style>

