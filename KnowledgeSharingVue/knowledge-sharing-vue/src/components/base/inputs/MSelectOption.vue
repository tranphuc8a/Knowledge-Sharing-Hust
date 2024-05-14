<template>

    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-select-option-frame" tabindex="1" @:blur="resolveOnBlur">
            <div class="p-select-option-box">
                <div class="p-select-option-box-left" @:click="toggleSelectOption">
                    <div class="p-select-option-value" v-show="data.value != null">
                        {{ data.text }}
                    </div>
                    <div class="p-select-option-placeholder" v-show="data.value == null">
                        {{ data.placeholder }}
                    </div>
                </div>
                <div class="p-select-option-box-right">
                    <div class="p-icon-container p-icon-down" @:click="resolveClickExpand">
                        <MIcon fa="chevron-down" />
                    </div>
                    <div class="p-icon-container p-icon-up" @:click="resolveClickCollapse">
                        <MIcon fa="chevron-up" />
                    </div>
                </div>
            </div>
            <!-- STATE OF DROPDOWN FRAME: normal, loading, empty -->
            <div class="p-select-option-dropdown-frame" :state="data.dropdownState" ref="dropdownFrame">
                <div class="p-select-option-dropdown-content" ref="dropdownContent">
                    <div class="p-select-option-dropdown-loading">
                        <MSpinner :style="{ fontSize: '24px' }" />
                    </div>
                    <div class="p-select-option-dropdown-empty">
                        {{ getLabel()?.noOptions }}
                    </div>
                    <div class="p-select-option-dropdown-items">
                        <!-- ITEMS STATE: normal, default, chosen, focus -->
                        <div v-for="(item, index) in data.items" :key="index"   
                            class="p-select-option-item" :state="item.state"
                            @:click="clickItemFunction(item)">
                            <div class="p-select-option-item-label" 
                                :title="item.label"
                                v-show="item.value != null">
                                {{ item.label }}
                            </div>
                            <div class="p-select-option-item-placeholder" 
                                v-show="item.value == null">
                                {{ item.label }}
                            </div>
                            <div class="p-select-option-item-checked">
                                <MIcon fa="check" />
                            </div>
                        </div>                            
                    </div>
                </div>
            </div>
        </div>
    </InputFrame>
</template>

<script>
import { Validator } from '@/js/utils/validator';
import { input } from '@/js/components/base/input';
import InputFrame from './MInputFrame.vue';
import { myEnum } from '@/js/resources/enum';

let selectOption = {
    name: "SelectOption",
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
                text: this.placeholder,
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,

                placeholder: this.placeholder,
                dropdownState: myEnum.dropdownState.NORMAL,
                chosen: null,
                items: [],
            },
            components: {
                dropdownContent: null,
                dropdownFrame: null
            }
        };
    },
    mounted() {
        this.components.dropdownContent = this.$refs.dropdownContent;
        this.components.dropdownFrame = this.$refs.dropdownFrame;
        this.setValue(this.value);
    },
    watch: {
        ...input.watch,
        placeholder(newVal)     { this.data.placeholder = newVal; },
        listItemLoader()        { this.refreshListItems(); },
    },
    methods: {
        ...input.methods,

        async getLabel(){
            return this.lang?.components?.input?.combobox;
        },
        
        /**
        * Override validate function
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async validate(){
            try {
                if (this.validator?.validate == null) return;
                let rsVal = await this.validator.validate(this.data.items, this.data.text);
                if (!rsVal.isValid){
                    this.inputFrame.errorMessage = rsVal.msg;
                    return false;
                }
                return true;
            } catch (error) {
                console.error(error);
            }
        },
        
        /* Override
        * Thực hiện xử lý logic khi expand
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnExpand(){
            try {
                this.setState(myEnum.inputState.EXPAND);
                await this.resolveRefreshItem();
            } catch (error){
                console.error(error);
            }
        },
        /* Override
        * Thực hiện xử lý logic khi blur component
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnBlur(){
            try {
                // await new Promise(e => setTimeout(e, 250));
                
                // update chosen item when unfocus selectOption:
                let text = this.data.text;
                if (this.data.chosen){
                    this.data.chosen.state = myEnum.comboboxItemState.NORMAL;
                }
                this.data.chosen = this.data.items.filter(function(item){
                    return item.label === text;
                });
                this.data.chosen = this.data.chosen.length > 0 ? this.data.chosen[0] : null;
                if (this.data.chosen?.value != null) {
                    this.data.chosen.state = myEnum.comboboxItemState.CHOSEN;
                } 
                // do selectOption logic when change to new value                
                await this.setState(myEnum.inputState.NORMAL);
                await this.resolveOnChange?.();
            } catch (error){
                console.error(error);
            }
        },
        

        /* Override
        * Hàm thực hiện đặt giá trị vào select option
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async setValue(value){
            try {
                if (Validator.isEmpty(value)){
                    this.data.text = this.placeholder;
                    this.data.value = null;
                    return;
                }
                if (this.data.items.length <= 0){
                    await this.refreshListItems();
                }
                let item = this.data.items.find(function(it){
                    return String(it.value) === String(value);
                });
                // this.data.text = value ?? "";
                if (item) {
                    item.state = myEnum.comboboxItemState.CHOSEN;
                    this.data.value = item.value;
                    this.data.text = item.label;
                } else {
                    this.data.text = "";
                }
                if (this.data.chosen){
                    this.data.chosen.state = myEnum.comboboxItemState.NORMAL;
                }
                this.data.chosen = item;
                if (this.isDynamicValidate){
                    this.actionValidate();
                }
            } catch (error){
                console.error(error);
            }
        },


        // OTHER METHODS:
        /**
        * Xử lý logic khi click chuột vào một item của selectOption
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async clickItemFunction(item){
            try {
                // item.state = myEnum.comboboxItemState.CHOSEN;
                this.chosen = item;
                this.data.text = item.label;
                this.data.value = item.value;
                // this.data.isClickItem = true;
                await this.resolveOnBlur();
            } catch (error) {
                console.error(error);
            }
        },  
        /**
        * Hai hàm xử lý sự kiện click vào nút expand và collapse
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveClickExpand(){
            await this.resolveOnExpand();
        },
        async resolveClickCollapse(){
            await this.resolveOnBlur();
        },
        /**
        * Cập nhật chiều cao của dropdownFrame dựa trên dropdownContent
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async updateDropdownHeight(){
            // let clH = 
            this.components.dropdownFrame.style.height = `${this.components.dropdownContent.clientHeight}px`;
        },
        /**
        * Xử lý sự kiện lam moi danh sach items cua select a
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveRefreshItem(){
            try {
                if (this.data.items.length > 1) return;

                this.data.dropdownState = myEnum.dropdownState.LOADING;
                this.$nextTick(this.updateDropdownHeight.bind(this));

                let tempItems = [{
                    value: null,
                    label: this.data.placeholder
                }];
                
                let response = await this.listItemLoader();
                response ??= [];
                if (response?.length >= 0){
                    tempItems = tempItems.concat(response.map(function(item){
                        if (Validator.isEmpty(item.state)){
                            item.state = myEnum.comboboxItemState.NORMAL;
                        }
                        return item;
                    }));
                }
                if (tempItems.length <= 1){
                    this.data.items = [];
                    this.data.text = "";
                    this.data.value = null;
                    this.data.chosen = null;
                    this.data.dropdownState = myEnum.dropdownState.EMPTY;
                } else {
                    this.data.items = tempItems;
                    this.data.dropdownState = myEnum.dropdownState.NORMAL;
                }
                this.$nextTick(this.updateDropdownHeight.bind(this));
            } catch (error) {
                console.error(error);
            }
        },



        // override resolveOnPressEnter
        async resolveOnPressEnter(){
            try {
                if (this.state == myEnum.inputState.EXPAND){
                    let text = this.data.text;
                    let item = this.data.items.find(function(it){
                        return String(it.label) == String(text);
                    });
                    if (item != null){
                        await this.clickItemFunction(item);
                    }
                    let value = this.getValue();
                    await this.onPressEnter?.(value);
                } else {
                    await this.resolveOnExpand();
                }
            } catch (error){
                console.log(error);
            }
        },

        async focus(){
            await this.resolveOnExpand();
        },

        async toggleSelectOption(){
            if (this.inputFrame.state == myEnum.inputState.EXPAND){
                await this.resolveOnBlur();
            } else {
                await this.resolveOnExpand();
            }
        }
    },
    props: {
        ...input.props,
        placeholder: {
            type: String,
            default: "- Chọn giá trị -"
        },
        listItemLoader: {
            type: Function,
            default: async function(){
                await new Promise(e => setTimeout(e, 1000));
                return Array.from([{
                        label: 'Hưng Yên',
                        value: 1
                    }, {
                        label: 'Hà Nội',
                        value: 2
                    }, {
                        label: 'Nam Định',
                        value: 3
                    }, ]);
            }
        },
        isDynamicValidate: { default: false },
        validator: {
            type: Object,
            default: { async validate(items, text){
                try {
                    if (Validator.isEmpty(text)){
                        return { isValid: true };
                    }
                    if (Validator.isEmpty(items)){
                        return {
                            isValid: false,
                            msg: "Dữ liệu không hợp lệ"
                        }
                    }
                    if (items.some(function(item){
                        return item.label === text;
                    })) {
                        return { isValid : true };
                    }
                    return {
                        isValid: false,
                        msg: "Giá trị không tồn tại"
                    }
                } catch (error){
                    console.error(error);
                    return {
                        isValid: false,
                        msg: error
                    }
                }
            }}
        }
    },
};
export default selectOption;
</script>

<style>
    @import url(@/css/base/input/select-option.css);
</style>


