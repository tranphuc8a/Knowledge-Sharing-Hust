<template>
    <FeedCardFrame>
        <div class="p-feedcard-addcourse">
            <div class="p-feedcard-addcourse__header">
                <TooltipUserAvatar :user="currentUser" :size="36" />
                <div class="p-feedcard-addcourse__reminder">
                    Chào {{ (currentUser?.FullName ?? "bạn") }}, hãy tạo một khóa học cho bản thân mình nào
                </div>
            </div>
            <div class="p-feedcard-devide"></div>
            <div class="p-feedcard-buttons">
                <MEmbeddedButton 
                    fa="plus"
                    icon-family="fas"
                    label="Tạo mới khóa học"
                    :onclick="resolveClickAddCourse"
                    :buttonStyle="buttonStyle"
                    :iconStyle="iconStyle"
                />
            </div>
        </div>
    </FeedCardFrame>
</template>

<script>
import FeedCardFrame from '../feed-subpage/postcard/FeedCardFrame.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton';
import CurrentUser from '@/js/models/entities/current-user';
import { useRouter } from 'vue-router';

export default {
    name: "AddCourseFeedCard",
    data() {
        return {
            label: null,
            buttonStyle: {
                color: 'var(--grey-color-600)',
            },
            iconStyle: {
                color: 'var(--grey-color)',
            },
            currentUser: null,
            router: useRouter(),
        }
    },
    async mounted() {
        this.currentUser = await CurrentUser.getInstance();
    },
    components: {
        FeedCardFrame, TooltipUserAvatar, MEmbeddedButton
    },
    methods: {
        

        async resolveClickAddCourse(){
            try {
                this.router.push('/course-create/');
            } catch (error) {
                console.error(error);
            }
        },
        
    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
.p-feedcard-addcourse{
    padding: 16px 16px;
    box-sizing: border-box;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.p-feedcard-addcourse__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.p-feedcard-addcourse__reminder{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;

    font-weight: 500;
    flex: 1;
    text-align: justify;
    font-size: 16px;
    min-height: 42px;
    border-radius: 32px;
    background-color: var(--grey-color-200);
    padding: 8px 16px;
    box-sizing: border-box;
    cursor: pointer;
    color: var(--grey-color-600);
}
.p-feedcard-addcourse__reminder:hover{
    background-color: var(--grey-color-300);
    color: var(--blue-grey-color-800);
}

.p-feedcard-devide{
    width: 100%;
    height: 1px;
    background-color: var(--primary-color-200);
}

.p-feedcard-buttons{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-feedcard-buttons .p-button{
    border-radius: 8px;
}
</style>