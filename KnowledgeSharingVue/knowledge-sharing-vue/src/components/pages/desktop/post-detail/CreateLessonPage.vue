<template>
    <DesktopHomeFrame>
        <div class="d-content">
            <div class="p-form">
                <div class="p-row p-title">
                    Tạo mới bài giảng
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
                        <CategoryInput :label="'Nhập category'" 
                            ref="category"
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
                    <MButton label="Tạo bài giảng" :onclick="resolveSubmitLesson" />
                </div>
            </div>

        </div>
    </DesktopHomeFrame>

</template>

<!-- title, abstract, thumbnail, categories, Privacy, estimateTimeInMinutes, content -->

<script> 
import MButton from '@/components/base/buttons/MButton';
import MRadio from '@/components/base/inputs/MRadio.vue';
import MImageInput from '@/components/base/inputs/MImageInput.vue';
import CategoryInput from '@/components/base/category/CategoryInput.vue';
import MTextArea from '@/components/base/inputs/MTextArea'
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import MarkdownEditor from './components/MarkdownEditor.vue';
import MTextfield from '@/components/base/inputs/MTextfield'
import { NotEmptyValidator, PositiveNumberValidator } from '@/js/utils/validator';
import { myEnum } from '@/js/resources/enum';
import { PostRequest, Request } from '@/js/services/request';
import { useRouter } from 'vue-router';

export default {
    name: 'CreateLessonPage',
    components: {
        MTextfield, MTextArea, MImageInput,
        MRadio, MButton,
        DesktopHomeFrame, MarkdownEditor, CategoryInput
    },
    data(){
        return {
            validators: {
                title: new NotEmptyValidator("Tiêu đề bài giảng không được trống"),
                estimateTime: new PositiveNumberValidator("Giá trị không hợp lệ")
                    .setIsAcceptEmpty(false, "Giá trị không được trống")
            },
            privacyEnum: myEnum.EPrivacy,
            inputs: [],
            keys: [],
            router: useRouter()
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

        async resolveSubmitLesson(){
            try {
                let isValid = await this.validate();
                if (!isValid){
                    this.startDynamicValidate();
                    this.focusError();
                    return;
                }
                let lesson = await this.getLesson();
                let postRequest = new PostRequest('Lessons')
                    .prepareFormData();
                for (let key in lesson){
                    postRequest.addFormData(key, lesson[key]);
                }
                let res = await postRequest.execute();
                let body = await Request.tryGetBody(res);

                this.getToastManager().success("Tạo mới bài giảng thành công");
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
        }
    },
    props: {

    },
    inject: {
        getToastManager: {}
    },
    mounted(){
        this.keys = ["title", "abstract", "thumbnail", "category", "privacy", "estimate", "content"];
        for (let key of this.keys) {
            this.inputs[key] = this.$refs[key];
        }
        console.log(this.inputs)
    }
}

</script>


<style scoped>

.d-content{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    padding-left: 24px;
    padding-right: 24px;
    padding-bottom: 48px;
}

.p-form{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    background-color: #fff;
    box-sizing: border-box;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1),
                0 2px 4px 0 rgba(0, 0, 0, 0.06);

}

.p-row{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 64px;
}

.p-row > :last-child{
    min-width: 500px;
}

.p-title, .p-submit-button{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 24px;
    font-weight: 500;
    margin-top: 20px;
    margin-bottom: 20px;
    font-family: 'ks-font-semibold';
    
}

.p-input-category{
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 20px;
}

.p-input-image{
    width: fit-content;
}

.p-input-estimate{
    width: 500px;
}

.p-editor{
    background-color: var(--primary-color-200);
    border: solid 1px var(--primary-color-200);
    border-radius: 4px;
    height: 700px;
    overflow: hidden;
}

</style>


