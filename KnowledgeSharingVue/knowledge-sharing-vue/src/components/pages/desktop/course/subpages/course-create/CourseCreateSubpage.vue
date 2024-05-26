

<template>
    <div class="p-course-create-subpage p-course-subpage">
        <div class="p-course-create-subpage__left">
            <!-- Navigation -->
            <CourseCreditNavigationCard
                :on-commit="resolveOnCreateCourse"
                :on-change-tab="resolveOnChangeTab"
                :is-edit="false"
                :tab-index="tabIndex"
            />
        </div>

        <div class="p-course-create-subpage__right">
            <!-- 4 content -->
            
            <CourseCreditGeneralContent
                ref="general" v-show="tabIndex == tabEnum.General" />

            <CourseCreditFeeContent
                ref="fee" v-show="tabIndex == tabEnum.Fee" />

            <CourseCreditIntroductionContent
                ref="introduction" v-show="tabIndex == tabEnum.Introduction" />

            <CourseCreditOtherContent
                ref="other" v-show="tabIndex == tabEnum.Other" />   

        </div>
    </div>
</template>



<script>
import CourseCreditNavigationCard from './CourseCreditNavigationCard.vue';
import CourseCreditGeneralContent from './CourseCreditGeneralContent.vue';
import CourseCreditFeeContent from './CourseCreditFeeContent.vue';
import CourseCreditIntroductionContent from './CourseCreditIntroductionContent.vue';
import CourseCreditOtherContent from './CourseCreditOtherContent.vue';

import CurrentUser from '@/js/models/entities/current-user';
import { PostRequest, Request } from '@/js/services/request';
import { useRouter } from 'vue-router';

export default {
    name: 'ProfileEditSubpage',
    components: {
        CourseCreditNavigationCard,
        CourseCreditGeneralContent,
        CourseCreditFeeContent,
        CourseCreditIntroductionContent,
        CourseCreditOtherContent,
    },
    props: {
    },
    data(){
        return {
            tabEnum: {
                General: "General",
                Fee: "Fee",
                Introduction: "Introduction",
                Other: "Other",
            },
            tabIndex: null,
            components: {
            },
            router: useRouter(),
            isWorking: false,
        }
    },
    async mounted(){
        try {
            this.components = {
                General: this.$refs.general,
                Fee: this.$refs.fee,
                Introduction: this.$refs.introduction,
                Other: this.$refs.other,
            }
            this.user = await CurrentUser.getInstance();
            await this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async refresh(){
            try {
                this.tabIndex = this.tabEnum.General;
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnChangeTab(tabIndex){
            try {
                this.tabIndex = tabIndex;
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnCreateCourse(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                if (! await this.validateData()){
                    return;
                }

                // Get data from input
                let inputValue = await this.getInputValue();

                // set post request
                let postRequest = new PostRequest('Courses')
                    .prepareFormData();
                for (let key in inputValue){
                    if (key == "Categories"){
                        let listCate = inputValue[key];
                        for (let cate of listCate){
                            postRequest.addFormData(key, cate);
                        }
                        continue;
                    }
                    postRequest.addFormData(key, inputValue[key]);
                }

                // send request
                let response = await postRequest.execute();
                let body = await Request.tryGetBody(response);
                let courseId = body.UserItemId;

                // toast Success:
                this.getToastManager().success('Tạo khóa học thành công');
                this.router.push(`/course/${courseId}`);
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },

        async validateData(){
            try {
                let errorTabIndex = null;
                if (! (await this.components.General.validate())) {
                    errorTabIndex = this.tabEnum.General;
                } else if (! (await this.components.Fee.validate())) {
                    errorTabIndex = this.tabEnum.Fee;
                } else if (! (await this.components.Introduction.validate())) {
                    errorTabIndex = this.tabEnum.Introduction;
                } else if (! (await this.components.Other.validate())) {
                    errorTabIndex = this.tabEnum.Other;
                }
                if (errorTabIndex != null){
                    await this.startDynamicValidate();
                    await this.focusError(errorTabIndex);
                    return false;
                }

                return true;                
            } catch (error){
                console.error(error);
                return false;
            }
        },

        async startDynamicValidate(){
            try {
                this.components.General.startDynamicValidate();
                this.components.Introduction.startDynamicValidate();
                this.components.Fee.startDynamicValidate();
                this.components.Other.startDynamicValidate();
            } catch (e){
                console.error(e);
            }
        },

        async stopDynamicValidate(){
            try {
                this.components.General.stopDynamicValidate();
                this.components.Introduction.stopDynamicValidate();
                this.components.Fee.stopDynamicValidate();
                this.components.Other.stopDynamicValidate();
            } catch (e){
                console.error(e);
            }
        },

        async focusError(tabIndex){
            try {
                this.tabIndex = tabIndex;
                let component = this.components[tabIndex];
                await component.focusError();
            } catch (e){
                console.error(e);
            }
        },

        async getInputValue(){
            try {
                let generalData = await this.components.General.getValue();
                let feeData = await this.components.Fee.getValue();
                let introductionData = await this.components.Introduction.getValue();
                let otherData = await this.components.Other.getValue();
                let combinedData = { ...generalData, ...feeData, ...introductionData, ...otherData };
                return combinedData;
            } catch (e){
                console.error(e);
                return null;
            }
        },

        async setInputValue(){
            try {
                await this.components.General.setValue(null);
                await this.components.Fee.setValue(null);
                await this.components.Introduction.setValue(null);
                await this.components.Other.setValue(null);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
    }
}

</script>

<style>
.p-course-subpage{
    max-width: var(--course-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.p-course-create-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.p-course-create-subpage__left{
    width: 0;
    flex-shrink: 1;
    flex-grow: 1;

    position: sticky;
    top: 0;

    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-course-create-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 3;
}

</style>

