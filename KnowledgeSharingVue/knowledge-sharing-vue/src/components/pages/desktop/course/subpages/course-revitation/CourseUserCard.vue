

<template>
    <div class="p-course-user-item-card">
        <div class="p-cui-card card">
            <div class="p-cui__left">
                <div class="p-cui__avatar">
                    <TooltipUserAvatar :user="dUser" :size="50" />
                </div>
                <div class="p-cui__userinfo">
                    <div class="p-cui__fullname">
                        <TooltipUsername :user="dUser" :style="usernameStyle" />
                    </div>
                    <div class="p-cui__username">
                        @{{ dUser?.Username ?? "username" }}
                    </div>
                </div>

            </div>
            <div class="p-cui__right">
                <div class="p-cui__button">
                    <CourseRelationButton 
                        :user="dUser"
                    />
                </div>
            </div>

        </div>
    </div>
</template>



<script>
import CourseRelationButton from '../../components/course-relation-button/CourseRelationButton.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';

export default {
    name: 'CourseUserCard',
    components: {
        CourseRelationButton,
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

.p-course-user-item-card{
    width: 100%;
}

.p-cui-card{
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-cui__left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}
.p-cui__right{
    display: flex;
}

.p-cui__userinfo{
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-around;
    align-items: flex-start;
    gap: 4px;
}

.p-cui__button{
    align-self: center;
    width: fit-content;
}

</style>

