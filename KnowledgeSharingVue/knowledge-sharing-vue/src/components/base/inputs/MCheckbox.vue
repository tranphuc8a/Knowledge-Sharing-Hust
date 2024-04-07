<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">     
        <div :class="['p-checkbox-items', `p-${data.direction}-direction`]">
            <label  v-for="(item, index) in data.items" 
                    class="p-checkbox-item" 
                    :key="index" >

                <div class="p-checkbox">
                    <input type="checkbox" :value="item.value" v-model="data.value"
                            @:change="resolveOnChange"  
                            :name="data.group"
                            />
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

let checkbox = {
    name: "Checkbox",
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
        if (! (this.data.value instanceof Array)){
            this.data.value = [];
        }
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
        
        /* Override
        * Hàm thực hiện đặt các giá trị vào checkbox
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        setValue(listValue){
            try {
                if (Validator.isEmpty(listValue) 
                    || !(listValue instanceof Array)) {
                    return;
                }
                this.data.value = listValue;
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
                validate(items, listChosen){
                    if (Validator.isEmpty(listChosen) || listChosen == []) {
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
                    if (listChosen.all(function(value){
                        return items.some(function(item){
                            item.value === value;
                        });
                    })){
                        return {
                            isValid: true
                        }
                    }
                    return {
                        isValid: false,
                        msg: "Các giá trị lựa chọn không hợp lệ"
                    }
                }
            }
        }
    },
    watch: {
        ...input.watch,
        value(newVal) {
            if (! (newVal instanceof Array)){
                newVal = [];
            } 
            this.setValue(newVal);
        },
        group(newVal)   { this.data.group = newVal; },
        direction(newVal) { this.data.direction = newVal; },
        items(newVal)       { this.data.items = newVal; }
    }
};
export default checkbox;
</script>

<style>
    @import url(@/css/base/checkbox.css);
</style>


