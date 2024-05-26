

<template>
    <div class="p-course-edit-subpage p-course-subpage">
        <div class="p-course-edit-subpage__left">
            <!-- Navigation -->
            <CourseCreditNavigationCard
                :on-commit="resolveOnEditCourse"
                :on-change-tab="resolveOnChangeTab"
                :is-edit="true"
                :tab-index="tabIndex"
            />
        </div>

        <div class="p-course-edit-subpage__right">
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
import CourseCreditNavigationCard from '../course-create/CourseCreditNavigationCard.vue';
import CourseCreditGeneralContent from '../course-create/CourseCreditGeneralContent.vue';
import CourseCreditFeeContent from '../course-create/CourseCreditFeeContent.vue';
import CourseCreditIntroductionContent from '../course-create/CourseCreditIntroductionContent.vue';
import CourseCreditOtherContent from '../course-create/CourseCreditOtherContent.vue';

import { PatchRequest, Request } from '@/js/services/request';

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
            isWorking: false,
            dCourse: {},
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
            await this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async refresh(){
            try {
                this.tabIndex = this.tabEnum.General;
                this.dCourse = this.getCourse();
                if (this.dCourse != null){
                    await this.setInputValue(this.dCourse);
                }
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

        async resolveOnEditCourse(){
            if (this.isWorking) return;
            try {
                this.isWorking = true;
                if (! await this.validateData()){
                    return;
                }
                let courseId = this.dCourse.UserItemId;
                if (courseId == null) return;

                // Get data from input
                let inputValue = await this.getInputValue();

                // set post request
                let patchRequest = new PatchRequest('Courses/' + courseId)
                    .prepareFormData();
                for (let key in inputValue){
                    if (key == "Categories"){
                        let listCate = inputValue[key];
                        for (let cate of listCate){
                            patchRequest.addFormData(key, cate);
                        }
                        continue;
                    }
                    patchRequest.addFormData(key, inputValue[key]);
                }

                // send request
                await patchRequest.execute();

                // toast Success:
                this.getToastManager().success('Cập nhật khóa học thành công');
                await this.refreshCoursePage?.();
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
                await this.components.General.setValue(this.dCourse);
                await this.components.Fee.setValue(this.dCourse);
                await this.components.Introduction.setValue(this.dCourse);
                await this.components.Other.setValue(this.dCourse);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        refreshCoursePage: {},
        getCourse: {},
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

.p-course-edit-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.p-course-edit-subpage__left{
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

.p-course-edit-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 3;
}

</style>

