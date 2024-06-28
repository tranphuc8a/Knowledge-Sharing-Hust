<template>
    <DesktopHomeFrame>
        <div class="d-content p-credit-lestion">
            <div class="d-empty-panel" v-show="isLessonExisted === false">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="p-form" v-show="isLessonExisted === true">
                <div class="p-row p-title">
                    Chỉnh sửa bài giảng
                </div>
                <div class="p-row">
                    <MTextfield 
                        label="Tiêu đề" :title="null" placeholder="Tiêu đề của bài giảng"
                        :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="validators.title"
                        state="normal" ref="title"/>
                    <div class="p-input-image">
                        <MImageInput label="Ảnh bìa" :title="null"
                            :is-show-icon="false" :is-show-title="true" 
                            :is-show-error="false" :is-obligate="false"
                            :validator="null" ref="thumbnail"/>
                    </div>
                </div> 

                <div class="p-row">
                    <MTextArea 
                        label="Mô tả" :title="null" placeholder="Mô tả của bài giảng"
                        :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="null"
                        :rows="1" max-height="150px"
                        state="normal" ref="abstract"/>
                    <div class="p-input-privacy">
                            <MRadio 
                                label="Quyền riêng tư" title="Chọn một giới tính phù hợp"
                                :is-show-title="true" :is-show-error="false" :is-obligate="false"
                                :items="getPrivacyItems()" direction="row" group="gender-2"
                                :value="privacyEnum.Private" 
                                ref="privacy"
                            />
                        </div>
                </div>

                <div class="p-row">
                    <div class="p-input-category">
                        <CategoryInput :label="'Chọn danh mục'" 
                            ref="category" :validator="validators.category"
                            :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        />
                    </div>

                    <div class="p-input-estimate">
                        <MTextfield 
                            label="Ước lượng thời gian đọc" placeholder="Đơn vị: phút" type="number"
                            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.estimateTime"
                            state="normal" ref="estimate"/>
                    </div>
                </div>
                
                <div class="p-row p-editor">
                    <MarkdownEditor ref="content"/>
                </div>

                <div class="p-row p-submit-button">
                    <MButton label="Cập nhật bài giảng" :onclick="resolveSubmitLesson" />
                </div>
            </div>

        </div>
    </DesktopHomeFrame>

</template>

<!-- title, abstract, thumbnail, categories, Privacy, estimateTimeInMinutes, content -->

<script> 
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import MButton from '@/components/base/buttons/MButton';
import MRadio from '@/components/base/inputs/MRadio.vue';
import MImageInput from '@/components/base/inputs/MImageInput.vue';
import CategoryInput from '@/components/base/category/CategoryInput.vue';
import MTextArea from '@/components/base/inputs/MTextArea'
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import MarkdownEditor from './components/MarkdownEditor.vue';
import MTextfield from '@/components/base/inputs/MTextfield'
import { NotEmptyValidator, PositiveNumberValidator, LimitItemNumberValidator } from '@/js/utils/validator';
import { myEnum } from '@/js/resources/enum';
import { GetRequest, PatchRequest, Request } from '@/js/services/request';
import { useRouter, useRoute } from 'vue-router';
import CurrentUser from '@/js/models/entities/current-user';
import ResponseLessonModel from '@/js/models/api-response-models/response-lesson-model';

export default {
    name: 'EditLessonPage',
    components: {
        MTextfield, MTextArea, MImageInput,
        MRadio, MButton, NotFoundPanel,
        DesktopHomeFrame, MarkdownEditor, CategoryInput
    },
    data(){
        return {
            errorMessage: "Bài giảng hiện không tồn tại hoặc đã bị xóa",
            defaultErrorMessage: "Bài giảng hiện không tồn tại hoặc đã bị xóa",
            validators: {
                title: new NotEmptyValidator("Tiêu đề bài giảng không được trống"),
                estimateTime: new PositiveNumberValidator("Giá trị không hợp lệ")
                    .setIsAcceptEmpty(false, "Giá trị không được trống"),
                category: new LimitItemNumberValidator("Số lượng category phải từ 2-5 loại khác nhau")
                    .setBoundary(2, 5),
            },
            privacyEnum: myEnum.EPrivacy,
            inputs: [],
            keys: [],
            router: useRouter(),
            route: useRoute(),
            lesson: null,
            lessonId: null,
            isLessonExisted: null,
            currentUser: null
        }
    },
    async created(){
        try {
            this.getLoadingPanel().show();
            this.lessonId = this.route.params.lessonId;
            this.currentUser = await CurrentUser.getInstance();
            if (this.currentUser == null) {
                this.getPopupManager().requiredLogin();
                return;
            }
            let response = await new GetRequest('Lessons/my/' + this.lessonId)
                .execute();
            let body = await Request.tryGetBody(response);
            this.lesson = new ResponseLessonModel();
            this.lesson.copy(body);
            this.isLessonExisted = true;
            this.update();
        }
        catch (error){
            try {
                Request.resolveAxiosError(error);
                let userMsg = await Request.tryGetUserMessage(error);
                this.errorMessage = userMsg ?? this.defaultErrorMessage;
                this.isLessonExisted = false;
            } catch (e){
                console.error(e);
            }
        } finally {
            this.getLoadingPanel().hide();
        }
    },
    methods: {
        getPrivacyItems(){
            return [{
                value: this.privacyEnum.Public,
                label: 'Công khai'
            }, {
                value: this.privacyEnum.Private,
                label: 'Riêng tư'
            }]
        },

        async validate(){
            try {
                let isValid = true;
                // console.log(this.inputs);
                for (let key of this.keys) {
                    let input = this.inputs[key];
                    // console.log(input);
                    if(! await input?.validate?.()){
                        isValid = false;
                        break;
                    }
                }
                return isValid;
            }
            catch (e){
                console.error(e);
                return false;
            }
        },

        async startDynamicValidate(){
            try {
                for (let key of this.keys) {
                    let input = this.inputs[key];
                    await input?.startDynamicValidate?.();
                }
            } catch (error){
                console.error(error);
            }
        },

        async stopDynamicValidate(){
            try {
                for (let key of this.keys) {
                    let input = this.inputs[key];
                    await input?.stopDynamicValidate?.();
                }
            } catch (error){
                console.error(error);
            }
        },

        async focusError(){
            try {
                for (let key of this.keys) {
                    let input = this.inputs[key];
                    if (! await input?.validate()){
                        await input?.focus?.();
                        break;
                    }
                }
            } catch (error){
                console.error(error);
            }
        },

        async resolveSubmitLesson(){
            try {
                let isValid = await this.validate();
                if (!isValid){
                    this.startDynamicValidate();
                    this.focusError();
                    return;
                }
                let lesson = await this.getLesson();
                let patchRequest = new PatchRequest('Lessons/' + this.lessonId)
                    .prepareFormData();
                for (let key in lesson){
                    if (key == "Categories"){
                        for (let category of lesson[key]){
                            patchRequest.addFormData(key, category);
                        }
                        continue;
                    }
                    patchRequest.addFormData(key, lesson[key]);
                }
                let res = await patchRequest.execute();
                let body = await Request.tryGetBody(res);

                this.getToastManager().success("Chỉnh sửa bài giảng thành công");
                if (body.UserItemId != null){
                    this.router.push(`/lesson/${body.UserItemId}`);
                }
            } catch (e) {
                await Request.resolveAxiosError(e);
            }
        },

        async getLesson(){
            try {
                let title = await this.$refs.title.getValue();
                let abstract = await this.$refs.abstract.getValue();
                let thumbnail = await this.$refs.thumbnail.getValue();
                let categories = await this.$refs.category.getValue();
                let privacy = await this.$refs.privacy.getValue();
                let estimateTimeInMinutes = await this.$refs.estimate.getValue();
                let content = await this.$refs.content.getValue();
                let lesson = {
                    Title: title,
                    Abstract: abstract,
                    Thumbnail: thumbnail,
                    Categories: categories,
                    Privacy: privacy,
                    EstimateTimeInMinutes: estimateTimeInMinutes,
                    Content: content
                }
                return lesson;
            } catch (error){
                console.error(error);
                return null;
            }
        },

        async update(){
            try {
                if (this.isLessonExisted === true){
                    this.keys = ["title", "abstract", "thumbnail", "category", "privacy", "estimate", "content"];
                    for (let key of this.keys) {
                        this.inputs[key] = this.$refs[key];
                    }
                    let lesson = this.lesson;
                    this.inputs.title.setValue(lesson.Title);
                    this.inputs.abstract.setValue(lesson.Abstract);
                    this.inputs.thumbnail.setValue(lesson.Thumbnail);
                    this.inputs.category.setValue(lesson.Categories);
                    this.inputs.privacy.setValue(lesson.Privacy);
                    this.inputs.estimate.setValue(lesson.EstimateTimeInMinutes);
                    this.inputs.content.setValue(lesson.Content);
                }
            }
            catch (error){
                console.error(error);
            }
        }
    },
    props: {

    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getLoadingPanel: {}
    },
    mounted(){
        this.update();
    }
}

</script>


<style scoped>
@import url(@/css/pages/desktop/components/credit-lestion.css);

</style>


