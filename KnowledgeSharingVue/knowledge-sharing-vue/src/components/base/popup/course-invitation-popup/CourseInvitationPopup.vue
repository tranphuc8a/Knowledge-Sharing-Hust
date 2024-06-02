

<template>
    <MPopup  ref="popup"
            :isShowDescription="true"
            :isShowPreviousButton="false"
            :isShowCancelButton="false"
            :isShowOkayButton="true"
            :isAutoHide="false"
            v-show="dIsShow"
        
            :on-okay="callbacks.onOkay"
            :on-close="callbacks.onClose"
            :on-cancel="callbacks.onCancel"
            :on-previous="callbacks.onPrevious"

            header="Chọn người dùng mời vào khóa học"
            description="Chọn người dùng mà bạn muốn mời vào khóa học của mình. Bạn có thể tìm kiếm người dùng bằng cách nhập tên, username hoặc email của họ vào ô tìm kiếm."
            okayButtonLabel="Đóng"
    >
        <div class="p-course-invitation-popup-content">
            <div class="p-cipc-search">
                <span>
                    <MTextfieldButton 
                        placeholder="Tìm kiếm"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickSearch" 
                        :validator="null"
                        :oninput="resolveInputSearch"
                        state="normal" ref="textfield"
                    />
                </span>
                <span>
                    <MActionIcon
                        fa="rotate-left"
                        :onclick="resolveClickReload"
                        :containerStyle="{
                            width: '36px',
                            height: '36px'
                        }"
                    />
                </span>
            </div>
            <div class="p-devide"></div>
            <div class="p-cipc-content" v-if="isCreated" ref="scroll-container"
                @:scroll="throttleScroll"
            >
                <div class="p-cipc-notfound" v-show="isOutOfUser && !(listUser.length > 0)">
                    <NotFoundPanel text="Không tìm người dùng nào nào" />
                </div>

                <div class="p-cipc-user-list">
                    <div class="p-cipc-user-item"
                        v-for="user in listUser"
                        :key="user?.UserId"
                    >
                        <CourseUserCard :user="user" />
                    </div>

                    <!-- Skeleton -->
                    <div class="p-cipc-user-item" v-show="!isOutOfUser">
                        <CourseUserCardSkeleton />
                    </div>
                    <div class="p-cipc-user-item" v-show="!isOutOfUser">
                        <CourseUserCardSkeleton />
                    </div>
                    <div class="p-cipc-user-item" v-show="!isOutOfUser">
                        <CourseUserCardSkeleton />
                    </div>
                </div>
                
            </div>
        </div>
    </MPopup>
</template>



<script>
import MPopup from './../MPopup.vue';
import { useRouter } from 'vue-router'; 
import MTextfieldButton from '@/components/base/inputs/MTextfieldButton.vue';
import Debounce from '@/js/utils/debounce';
import { GetRequest, Request } from '@/js/services/request';
import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';
import NotFoundPanel from '../NotFoundPanel.vue';
import CourseUserCardSkeleton from '@/components/pages/desktop/course/subpages/course-revitation/CourseUserCardSkeleton.vue';
import CourseUserCard from '@/components/pages/desktop/course/subpages/course-revitation/CourseUserCard.vue';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'SelectuserPopup',
    components: {
        MPopup,
        MTextfieldButton,
        CourseUserCard,
        NotFoundPanel,
        CourseUserCardSkeleton
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
                onPrevious: async () => { await this.hide() },
            },
            dIsShow: this.isShow,
            isCreated: false,
            router: useRouter(),
            throttleScroll: Debounce.throttle(this.resolveOnScroll.bind(this), 1000),
            listUser: [],
            searchText: "",
            value: null,
            isOutOfUser: false,
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
                this.loadMoreUser();
            }
        },
        async hide(){
            this.dIsShow = false;
        },

        async resolveOnScroll(){
            try {
                let scrollContainer = this.$refs['scroll-container'];
                if (scrollContainer == null) return;
                if (this.isOutOfUser || this.isLoadingMore || !(this.dIsShow === true)){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageuserHeight = 150;
                let leftuserNumber = 6;

                if (scrollHeight - scrollPosition < averageuserHeight * leftuserNumber){
                    // console.log("Load more user");
                    this.loadMoreUser();
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
                this.searchText = await this.$refs.textfield.getValue();
                this.listUser = [];
                this.isOutOfUser = false;
                this.loadMoreUser();
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreUser(){
            if (this.isLoadingMore) return;
            try {
                this.isLoadingMore = true;

                // prepare request
                let searchText = this.searchText;
                if (Validator.isEmpty(searchText) || Validator.isEmpty(searchText?.trim?.())){
                    searchText = 'a';
                }
                let offset = this.listUser.length;
                let limit = 10;

                // send request
                let res = await new GetRequest('Users/search').setParams({ offset, limit, searchKey: searchText }).execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // read response
                if (body.length < limit){
                    this.isOutOfUser = true;
                }
                let tempListUser = body.map(function(item){
                    return new ResponseUserCardModel().copy(item);
                });
                if (tempListUser.length > 0){
                    this.listUser = this.listUser.concat(tempListUser);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isLoadingMore = false;
            }
        },

        async resolveClickReload(){
            try {
                this.listUser = [];
                this.isOutOfUser = false;
                this.isCreated = true;
                await this.loadMoreUser();
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickOkay(){
            try {
                await this.onOkay?.();
            } catch (e){
                console.error(e);
            }
        },
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-course-invitation-popup-content {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    min-width: 50vw;
    min-width: 400px;
    max-width: 100%;
    width: 100%;
    height: 600px;
    gap: 16px;
}

.p-cipc-search {
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}


.p-cipc-content {
    width: 100%;
    height: 100%;
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-cipc-notfound{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 200px;
}

.p-cipc-user-list {
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
}

.p-cipc-user-item {
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 4px;
    cursor: initial;
}



.p-cipc-user-item {
    width: 100%;
    height: 100%;
    max-width: 100%;
    max-height: 100%;
    padding: 16px 12px;
    padding-right: 16px;
    border-radius: 8px;
}

.p-cipc-user-item:hover{
    background-color: var(--primary-color-50);
}

.p-cipc-user-item-mask{
    width: 100%;
    display: flex;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-cipc-user-card {
    width: 0;
    flex-grow: 1;
    height: fit-content;
    max-width: 100%;
    max-height: 100%;
}


</style>

