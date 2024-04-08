<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-date-picker-bound">
            <!-- <input ref="date-picker" class="p-date-picker" 
                    v-model="data.value" :placeholder="data.placeholder"  
                    @:change="resolveOnChange" @:focus="resolveOnFocus" @:blur="resolveOnBlur" 
                    :readonly="inputFrame.state==='read-only'"
                    type="date"/> -->
            <VueDatePicker ref="date-picker" class="p-date-picker" 
                    :readonly="inputFrame.state==='read-only'"
                    @internal-model-change="resolveOnChange" @:focus="resolveOnFocus" @:blur="resolveOnBlur"
                    v-model="data.value" :format="formatDate" />
        </div>
    </InputFrame>
</template>

<script>
import InputFrame from './MInputFrame.vue';
import { input } from '@/js/components/base/input';
import { MyDate } from '@/js/utils/mydate';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

export default{
    name: 'DatePicker',
    components: { InputFrame, VueDatePicker },
    data(){
        return {
            inputFrame: {
                state: this.state,
                label: this.label,
                title: this.title,
                errorMessage: this.errorMessage,
                isObligate: this.isObligate,
                isShowTitle: this.isShowTitle,
                isShowError: this.isShowError
            },
            data: {
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,
                placeholder: this.placeholder,
                isShowIcon: this.isShowIcon
            },
            components: {
                input: null,
            }
        }
    },
    mounted(){
        this.components.input = this.$refs.input;
    },
    props: {
        ...input.props,
        placeholder: { default: "Placeholder" },
        isShowIcon: { default: true },
        oninput: {
            type: Function,
            default: async function(){}
        },
        format: { default: 'dd-MM-yyyy' }
    },
    methods: {
        ...input.methods,
        formatDate(date) {
            return new MyDate(date).toFormat(this.format);
        }
    },
    watch: {
        ...input.watch,
        placeholder(newVal)     { this.data.placeholder = newVal; },
        isShowIcon(newVal)      { this.data.isShowIcon = newVal; },
    }
}
</script>

<style>
@import url(@/css/base/input/datepicker.css);
</style>
