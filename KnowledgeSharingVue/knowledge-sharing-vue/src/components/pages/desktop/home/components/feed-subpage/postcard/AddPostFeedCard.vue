<template>
    <FeedCardFrame v-show="currentUser != null">
        <div class="p-feedcard-addpost">
            <div class="p-feedcard-addpost__header">
                <TooltipUserAvatar :user="currentUser" :size="36" />
                <div class="p-feedcard-addpost__reminder">
                    {{ getLabel()?.remind(currentUser?.FullName ?? "bạn") }}
                </div>
            </div>
            <div class="p-feedcard-devide"></div>
            <div class="p-feedcard-buttons">
                <MEmbeddedButton 
                    fa="book-open"
                    icon-family="fas"
                    :label="getLabel()?.addLesson"
                    :onclick="resolveClickAddLesson"
                    :buttonStyle="buttonStyle"
                    :iconStyle="iconStyle"
                />
                <MEmbeddedButton 
                    fa="comments"
                    :label="getLabel()?.addQuestion"
                    :onclick="resolveClickAddQuestion"
                    :buttonStyle="buttonStyle"
                    :iconStyle="iconStyle"
                />
            </div>
        </div>
    </FeedCardFrame>
</template>

<script>
import FeedCardFrame from './FeedCardFrame.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton';
import CurrentUser from '@/js/models/entities/current-user';
import { useRouter } from 'vue-router';

export default {
    name: "AddPostFeedCard",
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
        this.getLabel();
        this.currentUser = await CurrentUser.getInstance();
    },
    components: {
        FeedCardFrame, TooltipUserAvatar, MEmbeddedButton
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.getLanguage != null){
                this.label = this.getLanguage()?.subpages?.feedpage?.addpostcard;
            }
            return this.label;
        },

        async resolveClickAddLesson(){
            try {
                this.router.push('/lesson-create/');
            } catch (error) {
                console.error(error);
            }
        },

        async resolveClickAddQuestion(){
            try {
                this.router.push('/question-create/');
            } catch (error) {
                console.error(error);
            }
        }
        
    },
    inject: {
        getLanguage: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
.p-feedcard-addpost{
    padding: 16px 16px;
    box-sizing: border-box;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.p-feedcard-addpost__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.p-feedcard-addpost__reminder{
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
.p-feedcard-addpost__reminder:hover{
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