<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title ?? ''" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">     
        <div :class="['p-radio-items', `p-${data.direction}-direction`]">
            <label  v-for="(item, index) in data.items" 
                    class="p-radio-item" 
                    :key="index" >

                <div class="p-radio-button">
                    <label>
                        <div class="p-radio-button-mask"> <div class="p-radio-button-dot"> </div> </div>
                        <input :name="data.group" :value="item.value" 
                            @:change="resolveOnChange"
                            v-model="data.value" type="radio" class="p-radio-button-org" />
                    </label>
                </div>
                <span class="p-radio-label"> {{ item.label }} </span>
            </label>
        </div>
    </InputFrame>
</template>

<script>
import { Validator } from '@/js/utils/validator';
import { input } from '@/js/components/base/input';
import InputFrame from './MInputFrame.vue';
let radio = {
    name: "Radio",
    components: { InputFrame },
    data() {
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

                group: this.group,
                direction: this.direction,
                items: this.items
            },
            components: {
                input: null
            }
        };
    },
    mounted() {
        
    },
    methods: {
        ...input.methods,
        /* Override
        * Validate dữ liệu của input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async validate(){
            try {
                let rs = await this.validator.validate(this.data.items, this.data.value);
                if (rs.isValid) {
                    return true;
                }
                this.inputFrame.errorMessage = rs.msg;
                return false;
            } catch (error){
                console.error(error);
            }
        },

        // override lại hàm setValue kế thừa từ input.js
        setValue(value){
            try {
                this.data.value = value;
                if (this.data.isDynamicValidate){
                    this.actionValidate();
                }
            } catch (error){
                console.error(error);
            }
        },
       
    },
    props: {
        ...input.props,
        direction: {
            default: "column",
            validator: function(direction){
                return direction === "column" || direction === "row";
            }
        },
        items: {
            type: Array,
            default: [{
                label: "Nam",
                value: 1
            }, {
                label: "Nữ",
                value: 2
            }, {
                label: "Khác",
                value: 0
            }]
        },
        group: {
            type: String,
            default: "Gender"
        },
        validator: {
            type: Object,
            default: {
                validate(items, value){
                    if (Validator.isEmpty(value)) {
                        return {
                            isValid: true
                        }
                    }
                    if (Validator.isEmpty(items)) {
                        return {
                            isValid: false,
                            msg: "Không tồn tại item"
                        }
                    }
                    if (items.some(function(item){
                        return item.value === value;
                    })){
                        return {
                            isValid: true
                        }
                    }
                    return {
                        isValid: false,
                        msg: "Giá trị không tồn tại"
                    }
                }
            }
        }
    },
    watch: {
        ...input.watch,
        value(newVal) {
            this.setValue(newVal);
        },
        group(newVal)   { this.data.group = newVal; },
        direction(newVal) { this.data.direction = newVal; },
        items(newVal)       { this.data.items = newVal; }
    }
};
export default radio;
</script>

<style>
    @import url(@/css/base/input/radiobutton.css);
</style>


