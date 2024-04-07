<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">     
            <div class="p-checkbox">
                <input type="checkbox" @:change="resolveOnChange" v-model="data.value" ref="input"
                        :name="data.group" :id="data.id" />
            </div>
    </InputFrame>
</template>

<script>

import { input } from '@/js/components/base/input';
import InputFrame from './MInputFrame.vue';

let checkbox = {
    components: { InputFrame },
    data() {
        return {
            inputFrame: {
                state: this.state,
                label: this.label,
                title: this.title,
                errorMessage: this.errorMessage,
                isObligate: this.isObligate,
                // isFull: this.isFull
                isShowTitle: false,
                isShowError: false
            },
            data: {
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,

                group: this.group,
                index: this.index,
                id: this.id
            },
            components: {
                input: null
            }
        };
    },
    mounted() {
        this.components.input = this.$refs.input;
        if (this.data.value instanceof Array){
            this.data.value = false;
        }
    },
    methods: {
        ...input.methods,
        /* Override
        * Thực hiện thay đổi giá trị, xử lý và gọi sự kiện cha
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnChange(){
            try {
                if (this.data.isDynamicValidate){
                    await this.actionValidate();
                }
                if (this.data.value){
                    await this.onChecked(this.data.value, this.data.index, this.data.group);
                } else {
                    await this.onUnchecked(this.data.value, this.data.index, this.data.group);
                }
                await this.onchange(this.data.value, this.data.index, this.data.group);
            } catch (error) {
                console.error(error);
            }
        },
        /*
        * Tương tự setValue nhưng tên ý nghĩa hơn
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async checked(isChecked = true){
            this.setValue(isChecked);
        },
    },
    props: {
        ...input.props,
        onChecked: {
            type: Function,
            default: async function(){}
        },
        onUnchecked: {
            type: Function,
            default: async function(){}
        },
        isFull: { default: false},
        group: { default: "checkbox-group"},        // checkbox group 
        id: { default: "checkbox-id"},              // use for label focus to
    },
    watch: {
        ...input.watch,
        // event onChecked, onUnchecked not neccessary to watch
        value(newVal)       { newVal = Boolean(newVal); this.data.value = newVal; },
        group(newVal)       { this.data.group = newVal; },
        index(newVal)       { this.data.index = newVal; },
        id(newVal)          { this.data.id = newVal; },
    }
};
export default checkbox;
</script>

<style>
    @import url(@/css/base/checkbox.css);
</style>


