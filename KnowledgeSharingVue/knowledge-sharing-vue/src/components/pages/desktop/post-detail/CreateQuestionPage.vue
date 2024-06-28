<template>
    <DesktopHomeFrame>
        <div class="d-content p-credit-lestion">
            <div class="d-empty-panel" v-show="isError === true">
                <not-found-panel :text="errorMessage" />
            </div>

            <div class="p-form" v-show="isError === false">
                <div class="p-row p-title">
                    Tạo mới bài thảo luận 
                    {{ course != null ? ` trong khóa học ${course?.Title}` : '' }}
                </div>
                <div class="p-row">
                    <MTextfield 
                        label="Tiêu đề" :title="null" placeholder="Tiêu đề của bài thảo luận"
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
                        label="Mô tả" :title="null" placeholder="Mô tả của bài thảo luận"
                        :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        :validator="null"
                        :rows="1" max-height="150px"
                        state="normal" ref="abstract"/>
                    <div></div>
                </div>

                <div class="p-row">
                    <div class="p-input-category">
                        <CategoryInput :label="'Chọn danh mục'" 
                            ref="category" :validator="validators.category"
                            :is-show-title="true" :is-show-error="true" :is-obligate="false"
                        />
                    </div>
                    <div></div>
                </div>
                
                <div class="p-row p-editor">
                    <MarkdownEditor ref="content"/>
                </div>

                <div class="p-row p-submit-button">
                    <MButton label="Tạo bài thảo luận" :onclick="resolveSubmitQuestion" />
                </div>
            </div>

        </div>
    </DesktopHomeFrame>

</template>

<!-- title, abstract, thumbnail, categories, content -->

<script> 
import MButton from '@/components/base/buttons/MButton';
import MImageInput from '@/components/base/inputs/MImageInput.vue';
import CategoryInput from '@/components/base/category/CategoryInput.vue';
import MTextArea from '@/components/base/inputs/MTextArea'
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import MarkdownEditor from './components/MarkdownEditor.vue';
import MTextfield from '@/components/base/inputs/MTextfield'
import { NotEmptyValidator, PositiveNumberValidator, LimitItemNumberValidator } from '@/js/utils/validator';
import { myEnum } from '@/js/resources/enum';
import { PostRequest, Request, GetRequest } from '@/js/services/request';
import { useRoute, useRouter } from 'vue-router';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CurrentUser from '@/js/models/entities/current-user';


export default {
    name: 'CreateQuestionPage',
    components: {
        MTextfield, MTextArea, MImageInput,
        MButton, NotFoundPanel,
        DesktopHomeFrame, MarkdownEditor, CategoryInput
    },
    data(){
        return {
            validators: {
                title: new NotEmptyValidator("Tiêu đề bài thảo luận không được trống"),
                estimateTime: new PositiveNumberValidator("Giá trị không hợp lệ")
                    .setIsAcceptEmpty(false, "Giá trị không được trống"),
                category: new LimitItemNumberValidator("Số lượng category phải từ 2-5 loại khác nhau")
                    .setBoundary(2, 5),
            },
            privacyEnum: myEnum.EPrivacy,
            inputs: [],
            keys: [],
            route: useRoute(),
            router: useRouter(),
            courseId: null,
            course: null,

            isError: false,
            errorMessage: "",
            currentUser: null,
            defaultErrorMessage: "Khóa học không tồn tại hoặc đã bị xóa",
        }
    },
    async created(){
        try {
            this.courseId = this.route.query.courseId;
            if (this.courseId != null){
                this.getLoadingPanel().show();
                let res = await new GetRequest('Courses/' + this.courseId).execute();
                let body = await Request.tryGetBody(res);
                if (body != null){
                    this.course = new ResponseCourseModel();
                    this.course.copy(body);

                    // if (this.currentUser != null){
                    //     if (this.course?.UserId != this.currentUser?.UserId){
                    //         this.isError = true;
                    //         this.errorMessage = "Bạn không có quyền tạo bài thảo luận trong khóa học này";
                    //         return;
                    //     }
                    // }
                    this.isError = false;
                }
            } else {
                this.courseId = null;
            }
        } catch (error) {
            try {
                Request.resolveAxiosError(error);
                let userMsg = await Request.tryGetUserMessage(error);
                this.errorMessage = userMsg ?? this.defaultErrorMessage;
                this.isError = true;
            } catch (e){
                console.error(e);
            }
        } finally {
            this.getLoadingPanel().hide();
        }
    },
    methods: {

        async validate(){
            try {
                let isValid = true;
                console.log(this.inputs);
                for (let key of this.keys) {
                    let input = this.inputs[key];
                    console.log(input);
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

        async resolveSubmitQuestion(){
            try {
                let isValid = await this.validate();
                if (!isValid){
                    this.startDynamicValidate();
                    this.focusError();
                    return;
                }
                let Question = await this.getQuestion();
                let postRequest = new PostRequest('Questions')
                    .prepareFormData();
                for (let key in Question){
                    if (key == "Categories"){
                        for (let category of Question[key]){
                            postRequest.addFormData(key, category);
                        }
                        continue;
                    }
                    postRequest.addFormData(key, Question[key]);
                }
                
                let res = await postRequest.execute();
                let body = await Request.tryGetBody(res);
                
                this.getToastManager().success("Tạo mới bài thảo luận thành công");
                if (body.UserItemId != null){
                    this.router.push('/question/' + body.UserItemId);
                }
            } catch (e) {
                await Request.resolveAxiosError(e);
            }
        },

        async getQuestion(){
            try {
                let title = await this.$refs.title.getValue();
                let abstract = await this.$refs.abstract.getValue();
                let thumbnail = await this.$refs.thumbnail.getValue();
                let categories = await this.$refs.category.getValue();
                let content = await this.$refs.content.getValue();
                let question = {
                    Title: title,
                    Abstract: abstract,
                    Thumbnail: thumbnail,
                    Categories: categories,
                    Content: content
                }
                question.CourseId = this.courseId ?? null;
                return question;
            } catch (error){
                console.error(error);
                return null;
            }
        },

        async checkLoggedIn(){
            try {
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser == null){
                    this.getPopupManager().requiredLogin();
                }
            } catch (error){
                console.error(error);
            }
        }
    },
    props: {

    },
    inject: {
        getLoadingPanel: {},
        getToastManager: {},
        getPopupManager: {}
    },
    mounted(){
        try {
            this.checkLoggedIn();
            this.keys = ["title", "abstract", "thumbnail", "category", "content"];
            for (let key of this.keys) {
                this.inputs[key] = this.$refs[key];
            }
            // console.log(this.inputs);
        }
        catch (error){
            console.error(error);
        }
    }
}

</script>


<style scoped>
@import url(@/css/pages/desktop/components/credit-lestion.css);

</style>


