<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate"
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-textarea-bound" :title="title ?? null">
            <textarea ref="textarea" class="p-textarea" v-model="data.value"
                    :placeholder="data.placeholder"  
                    @:input="resolveOnInput" @:change="resolveOnChange" 
                    @:focus="resolveOnFocus" @:blur="resolveOnBlur" 
                    :readonly="inputFrame.state==='read-only'"/>
        </div>
    </InputFrame>
</template>

<script>
import Icon from '@/components/base/icons/MIcon.vue';
import ActionIcon from '@/components/base/icons/MActionIcon.vue';
import Spinner from '@/components/base/icons/MSpinner.vue';
import InputFrame from './MInputFrame.vue';
import { input } from '@/js/components/base/input';
// import { myEnum } from '@/js/resources/enum';

let textarea = {
    name: "TextArea",
    data() {
        return {
            inputFrame: {
                state: this.state,
                label: this.label,
                title: this.title,
                errorMessage: this.errorMessage,
                isObligate: this.isObligate,
                // isFull: this.isFull
                isShowTitle: this.isShowTitle,
                isShowError: this.isShowError
            },
            data: {
                greenColor: { color: 'var(--light-green-color)'},
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,
                placeholder: this.placeholder,
                type: this.type,
                isShowIcon: this.isShowIcon
            },
            components: {
                input: null,
            }
        };
    },
    mounted() {
        this.components.input = this.$refs.textarea;
    },
    components: {
        Icon, InputFrame, ActionIcon, Spinner
    },
    methods: {
        ...input.methods,
        
        /**
        * Ghi đè sự kiện resolveOnChange
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnChange(){
            try {
                await this.onchange(this.data.value);
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Xử lý sự kiện resolveOnInput
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnInput(){
            try {
                this.adjustHeight();
                if (this.data.isDynamicValidate){
                    await this.actionValidate();
                }
                await this.oninput(this.data.value);
            } catch (error){
                console.error(error);
            }
        },

        /**
        * Xử lý sự kiện căn chỉnh chiều cao của textarea
        * @param none
        * @returns none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        */
        async adjustHeight() {
            try {
                this.components.input.style.height = `auto`;
                this.components.input.style.height = `${this.components.input.scrollHeight}px`;
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện click vào icon
         * @param none
         * @Created PhucTV (25/1/24)
         * @Modified None
        */
        resolveOnclickIcon(){
            let that = this;
            return async function(){
                await that.onclickIcon(that.data.value);
            }
        }
    },
    props: {
        ...input.props,
        type: { default: "text" },
        placeholder: { default: "Placeholder" },
        autocomplete: { default: "email" },
        isShowIcon: { default: true },
        oninput: {
            type: Function,
            default: async function(){}
        },
        onclickIcon: {
            type: Function,
            default: null
        }
    },
    watch: {
        ...input.watch,
        type(newVal)            { this.data.type = newVal; },
        placeholder(newVal)     { this.data.placeholder = newVal; },
        isShowIcon(newVal)      { this.data.isShowIcon = newVal; },
    }
};
export default textarea;
</script>

<style>
@import url(@/css/base/input/textarea.css);
</style>
