

<template>
    
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate"
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-textfield-bound" :title="title ?? null">
            <input ref="textfield" class="p-textfield" v-model="data.value"
                    :type="data.type" :placeholder="data.placeholder" :autocomplete="autocomplete"
                    @:input="resolveOnInput" @:change="resolveOnChange" 
                    @:focus="resolveOnFocus" @:blur="resolveOnBlur" 
                    @:keyup.enter.prevent.stop="resolveOnPressEnter"
                    :readonly="inputFrame.state==='read-only'"/>
            <slot> </slot>
        </div>
    </InputFrame>
</template>

<script>
import { input } from '@/js/components/base/input';
import { myEnum } from '@/js/resources/enum';
import InputFrame from '@/components/base/inputs/MInputFrame.vue';
export default {
    name: 'MTextfield',
    data(){
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
        }
    },
    components: { InputFrame },
    mounted(){
        this.inputFrame.state = myEnum.inputState.NORMAL;
        this.components.input = this.$refs.textfield;
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
                if (this.data.isDynamicValidate){
                    await this.actionValidate();
                }
                await this.oninput(this.data.value);
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
        }
    },
    watch: {
        ...input.watch,
        type(newVal)            { this.data.type = newVal; },
        placeholder(newVal)     { this.data.placeholder = newVal; },
        isShowIcon(newVal)      { this.data.isShowIcon = newVal; },
    }
}
</script>


<style scoped>
@import url(@/css/base/input/textfield.css);
</style>
