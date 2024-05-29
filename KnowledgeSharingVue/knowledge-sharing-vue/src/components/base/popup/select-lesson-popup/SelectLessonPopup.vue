

<template>
    <MPopup  ref="popup"
            :isShowDescription="true"
            :isShowPreviousButton="false"
            :isShowCancelButton="true"
            :isShowOkayButton="true"
            :is-show-previous-button="true"
            :isAutoHide="false"
            v-show="dIsShow"
        
            :on-okay="callbacks.onOkay"
            :on-close="callbacks.onClose"
            :on-cancel="callbacks.onCancel"
            :on-previous="callbacks.onPrevious"

            header="Chọn bài giảng thêm vào khóa học"
            description="Chọn bài giảng mà bạn muốn thêm vào khóa học. Bạn cũng có thể tạo bài giảng mới nếu cần."
            cancelButtonLabel="Thôi không chọn nữa"
            okayButtonLabel="Chốt lấy cái này"
            previousButtonLabel="Tạo bài giảng mới"
    >
        <div class="p-lesson-popup-content">
            <div class="p-lpc-search">
                <span>
                    <MTextfieldButton 
                        title="Nhập từ khóa tìm kiếm" placeholder="Tìm kiếm"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickSearch" 
                        :validator="null"
                        :oninput="resolveInputSearch"
                        state="normal"
                    />
                </span>
                <span>
                    <MActionIcon
                        fa="rotate-left"
                        :onclick="resolveClickReload"
                    />
                </span>
            </div>
            <div class="p-devide"></div>
            <div class="p-lpc-content" v-if="isCreated" ref="scroll-container"
                @:scroll="throttleScroll"
            >
                <div class="p-lps-notfound" v-show="isOutOfLesson && !(listLesson.length > 0)">
                    <NotFoundPanel text="Không tìm thấy bài giảng nào" />
                </div>

                <div class="p-lps-lesson-list">
                    <div class="p-lps-lesson-item p-radio-button"
                        v-for="lesson in listLesson"
                        :key="lesson?.UserItemId"
                    >
                        <label>
                            <div class="p-lps-lesson-item-mask">
                                <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                                <div class="p-lps-lesson-card">
                                    <LessonShortCard :lesson="lesson" />
                                </div>
                            </div>
                            <input name="select-lesson-to-course" :value="lesson" 
                                v-model="value" type="radio" class="p-radio-button-org" />
                        </label>
                    </div>

                    <!-- Skeleton -->
                    <div class="p-lps-lesson-item p-radio-button" v-show="!isOutOfLesson">
                        <label>
                            <div class="p-lps-lesson-item-mask">
                                <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                                <div class="p-lps-lesson-card">
                                    <LessonShortCardSkeleton />
                                </div>
                            </div>
                        </label>
                    </div>
                    <div class="p-lps-lesson-item p-radio-button" v-show="!isOutOfLesson">
                        <label>
                            <div class="p-lps-lesson-item-mask">
                                <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                                <div class="p-lps-lesson-card">
                                    <LessonShortCardSkeleton />
                                </div>
                            </div>
                        </label>
                    </div>
                    <div class="p-lps-lesson-item p-radio-button" v-show="!isOutOfLesson">
                        <label>
                            <div class="p-lps-lesson-item-mask">
                                <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                                <div class="p-lps-lesson-card">
                                    <LessonShortCardSkeleton />
                                </div>
                            </div>
                        </label>
                    </div>

                </div>

                
            </div>
        </div>
    </MPopup>
</template>



<script>
import MPopup from './../MPopup.vue';
import { useRouter } from 'vue-router'; 
import MTextfieldButton from './../../inputs/MTextfieldButton.vue';
import Debounce from '@/js/utils/debounce';
import { Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';
import LessonShortCard from '../../cards/LessonShortCard.vue';
import NotFoundPanel from '../NotFoundPanel.vue';
import LessonShortCardSkeleton from '../../cards/LessonShortCardSkeleton.vue';

export default {
    name: 'SelectLessonPopup',
    components: {
        MPopup,
        MTextfieldButton,
        LessonShortCard,
        NotFoundPanel,
        LessonShortCardSkeleton
    },
    props: {
        onOkay: {
            type: Function,
            default: () => {}
        },
        isShow: {
            type: Boolean,
            default: false
        },
    },
    data(){
        return {
            callbacks: {
                onOkay: this.resolveClickOkay.bind(this),
                onClose: async () => { await this.hide() },
                onCancel: async () => { await this.hide() },
                onPrevious: this.resolveClickPrevious.bind(this),
            },
            dIsShow: this.isShow,
            isCreated: false,
            router: useRouter(),
            throttleScroll: Debounce.throttle(this.resolveOnScroll.bind(this), 1000),
            listLesson: [],
            searchText: "",
            value: null,
            isOutOfLesson: false,
            isLoadingMore: false
        }
    },
    async mounted(){
    },
    methods: {
        async show(){
            this.dIsShow = true;
            if (!this.isCreated) {
                this.isCreated = true;
            }
        },
        async hide(){
            this.dIsShow = false;
        },

        async resolveOnScroll(){
            try {
                let scrollContainer = this.$refs['scroll-container'];
                if (scrollContainer == null) return;
                if (this.isOutOfLesson || this.isLoadingMore || !(this.isShow === true)){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageLessonHeight = 150;
                let leftLessonNumber = 6;

                if (scrollHeight - scrollPosition < averageLessonHeight * leftLessonNumber){
                    // console.log("Load more Lesson");
                    this.loadMoreLesson();
                }
            } catch (e){
                console.error(e);
            }
        },

        async resolveInputSearch(value){
            this.searchText = value;
        },

        async resolveClickSearch(){
            try {
                this.listLesson = [];
                this.isOutOfLesson = false;
                this.loadMoreLesson();
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreLesson(){
            if (this.isLoadingMore) return;
            try {
                this.isLoadingMore = true;

                // prepare request
                let url = '';
                if (Validator.isEmpty(this.searchText)){
                    url = 'Lessons/my';
                } else {
                    url = 'Lessons/search/my/';
                }
                let offset = this.listLesson.length;
                let limit = 10;

                // send request
                let res = await new GetRequest(url).setParams({ offset, limit, search: this.searchText }).execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // read response
                if (body.length < limit){
                    this.isOutOfLesson = true;
                }
                let tempListLesson = body.map(function(item){
                    return new ResponseLessonModel().copy(item);
                });
                if (tempListLesson.length > 0){
                    this.listLesson = this.listLesson.concat(tempListLesson);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isLoadingMore = false;
            }
        },

        async resolveClickReload(){
            try {
                this.listLesson = [];
                this.isOutOfLesson = false;
                this.isCreated = true;
                this.loadMoreLesson();
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickOkay(){
            try {
                if (this.value == null) {
                    this.getToastManager().error("Bạn chưa chọn bài giảng nào cả!");
                    return;
                }
                await this.onOkay(this.value);
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickPrevious(){
            this.router.push('/lesson-create');
        }
    },
    inject: {
        getToastManager: {}
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-lesson-popup-content {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    min-width: 800px;
    width: 100%;
    height: 600px;
    gap: 16px;
}

.p-lpc-search {
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}


.p-lpc-content {
    width: 100%;
    height: 100%;
    padding-top: 32px;
    padding-bottom: 32px;
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-lps-notfound{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 200px;
}

.p-lps-lesson-list {
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-lps-lesson-item {
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-lps-lesson-item label {
    max-width: 100%;
    max-height: 100%;
}

.p-lps-lesson-item-mask{
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-lps-lesson-card {
    width: fit-content;
    height: fit-content;
    max-width: 100%;
    max-height: 100%;
}


</style>

