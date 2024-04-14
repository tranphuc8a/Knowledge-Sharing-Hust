<template>
    <FeedCardFrame>
        <div class="p-feedcard-lesson">
            <div class="p-feedcard-lesson__header">
                <PostCardHeader :post="lesson" />
            </div>
            <div class="p-feedcard-devide"></div>
            <div class="p-feedcard-buttons">
                <MEmbeddedButton 
                    fa="book-open"
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
// import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
// import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import PostCardHeader from './PostCardHeader.vue';
import FeedCardFrame from './FeedCardFrame.vue';
import MEmbeddedButton from '@/components/base/buttons/MEmbeddedButton';
export default {
    name: "lessonFeedCard",
    data() {
        return {
            label: null,
            buttonStyle: {
                color: 'var(--blue-grey-color-800)',
            },
            iconStyle: {
                color: 'var(--grey-color)',
            },
        }
    },
    mounted() {
        this.getLabel();
    },
    components: {
        FeedCardFrame, MEmbeddedButton, PostCardHeader
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
            if (this.inject?.language != null){
                this.label = this.inject?.language?.subpages?.feedpage?.lessoncard;
            }
            return this.label;
        },

        async resolveClickAddLesson(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-lesson'});
            } catch (error) {
                console.log(error);
            }
        },

        async resolveClickAddQuestion(){
            try {
                let router = this.globalData.router;
                router.push({name: 'add-question'});
            } catch (error) {
                console.log(error);
            }
        }
        
    },
    inject: {
        inject: {},
        getToastManager: {},
        getPopupManager: {}
    },
    props: {
        lesson: {}
    }
}
</script>

<style scoped>
.p-feedcard-lesson{
    padding: 16px 0px;
    box-sizing: border-box;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.p-feedcard-lesson__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.p-feedcard-lesson__reminder{
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
    padding: 4px 12px;
    box-sizing: border-box;
    cursor: pointer;
    color: var(--grey-color-600);
}
.p-feedcard-lesson__reminder:hover{
    background-color: var(--grey-color-300);
    color: var(--blue-grey-color-800);
}

.p-feedcard-devide{
    width: 100%;
    height: 1px;
    background-color: var(--grey-color-300);
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