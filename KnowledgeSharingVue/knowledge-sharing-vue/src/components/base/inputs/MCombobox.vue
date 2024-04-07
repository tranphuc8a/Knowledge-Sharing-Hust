<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-combobox-textfield">
            <div class="p-combobox-textfield-left">
                <input type="text" class="p-combobox-input" v-model="data.text" 
                        :placeholder="data.placeholder" 
                        @:focus="resolveOnFocus" @:blur="resolveOnBlur" 
                        @:input="resolveOnInput"
                        v-on:keyup.enter.prevent.stop="resolveOnPressEnter"
                        ref="input">
            </div>
            <div class="p-combobox-textfield-right">
                <div class="p-icon-container p-icon-down" @:click="resolveClickExpand">
                    <i class="fa pi-sprite-chevron-down p-icon"></i>
                </div>
                <div class="p-icon-container p-icon-up" @:click="resolveClickCollapse">
                    <i class="fa pi-sprite-chevron-up p-icon"></i>
                </div>
            </div>
        </div>
        <!-- STATE OF FRAME: normal, loading, empty -->
        <div class="p-combobox-dropdown-frame" :state="data.dropdownState" ref="dropdownFrame">
            <div class="p-combobox-dropdown-content" ref="dropdownContent">
                <div class="p-combobox-dropdown-loading">
                    <Spinner color="green" size="normal" />
                </div>
                <div class="p-combobox-dropdown-empty">
                    {{ label.noOptions }}
                </div>
                <div class="p-combobox-dropdown-items">
                    <!-- ITEMS STATE: normal, default, chosen, focus -->
                    <div v-for="(item, index) in data.filteredItems" :key="index"   
                        class="p-combobox-item" :state="item.state"
                        @:click="clickItemFunction(item)">
                        <div class="p-combobox-item-label" :title="item.label">
                            {{ item.label }}
                        </div>
                        <div class="p-combobox-item-checked">
                            <i class="pi-icon pi-check-solid-green p-validating-icon"></i>
                        </div>
                    </div>                            
                </div>
            </div>
        </div>
    </InputFrame>
</template>

<script>
import { Unicode } from '@/js/utils/unicode';
import { Validator } from '@/js/utils/validator';
import { input } from '@/js/components/base/input';
import InputFrame from './MInputFrame.vue';
import { myEnum } from '@/js/resources/enum';
import Spinner from '@/components/base/icons/MSpinner.vue';

let combobox = {
    name: "Combobox",
    components: { InputFrame, Spinner },
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
                text: this.value,
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,

                placeholder: this.placeholder,
                dropdownState: myEnum.dropdownState.NORMAL,
                chosen: null,
                items: [],
                filteredItems: [],
                callFilter: 0,
                isFiltering: false
            },
            label: this.lang.components.input.combobox,
            components: {
                dropdownContent: null,
                dropdownFrame: null,
                input: null
            }
        };
    },
    mounted() {
        // this.refreshListItems();
        this.components.dropdownContent = this.$refs.dropdownContent;
        this.components.dropdownFrame = this.$refs.dropdownFrame;
        this.components.input = this.$refs.input;
    },
    watch: {
        ...input.watch,
        placeholder(newVal)     { this.data.placeholder = newVal; },
        listItemLoader()        { this.refreshListItems(); },
        listItemFilter()        { this.refreshListItems(); },
    },
    methods: {
        ...input.methods,
        /*
        * Override validate function
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async validate(){
            try {
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
        /*
        * Thực hiện xử lý logic sau khi thay đổi giá trị của input live
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnInput(){
            try {
                this.data.isFiltering = true;
                await this.resolveFilterItem();
                await this.oninput();
            } catch (error){
                console.error(error);
            }
        },
        /* Override
        * Thực hiện xử lý logic sau khi focus vào input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnFocus(){
            try {
                this.setState(myEnum.inputState.EXPAND);
                await this.resolveFilterItem();
                await this.onfocus();
            } catch (error){
                console.error(error);
            }
        },
        /* Override
        * Thực hiện xử lý logic khi unfocus input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnBlur(){
            try {
                this.data.isFiltering = false;
                await new Promise(e => setTimeout(e, 250));
                // if (this.data.isClickItem === false){
                // }
                // this.data.isClickItem = false;
                // update chosen item when unfocus combobox:
                let that = this;
                if (this.data.chosen){
                    this.data.chosen.state = myEnum.comboboxItemState.NORMAL;
                }
                this.data.chosen = this.data.items.filter(function(item){
                    return item.label === that.data.text;
                });
                this.data.chosen = this.data.chosen.length > 0 ? this.data.chosen[0] : null;
                if (this.data.chosen) {
                    this.data.chosen.state = myEnum.comboboxItemState.CHOSEN;
                } 
                // do combobox logic when change to new value                
                await this.setState(myEnum.inputState.NORMAL);
                await this.resolveOnChange();
                await this.onblur();
            } catch (error){
                console.error(error);
            }
        },
        /* Override
        * Hai hàm thực hiện focus và blur vào thẻ input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async focus(){
            try {
                this.$refs.input.focus();
            } catch (error){
                console.error(error);
            }
        },
        async blur(){
            try {
                this.$refs.input.blur();
            } catch (error){
                console.error(error);
            }
        },

        /* Override
        * Hàm thực hiện đặt giá trị vào combobox
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async setValue(value){
            try {
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
        /* 
        * Xử lý logic khi click chuột vào một item của combobox
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async clickItemFunction(item){
            try {
                item.state = myEnum.comboboxItemState.CHOSEN;
                this.chosen = item;
                this.data.text = item.label;
                this.data.value = item.value;
                // this.data.isClickItem = true;
                await this.blur();
            } catch (error) {
                console.error(error);
            }
        },  
        /* 
        * Hai hàm xử lý sự kiện click vào nút expand và collapse
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveClickExpand(){
            await this.$refs.input.focus();
        },
        async resolveClickCollapse(){
            await this.$refs.input.blur();
        },
        /*
        * Cập nhật chiều cao của dropdownFrame dựa trên dropdownContent
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async updateDropdownHeight(){
            // let clH = 
            this.components.dropdownFrame.style.height = `${this.components.dropdownContent.clientHeight}px`;
        },
        /* 
        * Xử lý sự kiện yêu cầu lọc item theo text
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveFilterItem(){
            try {
                this.data.dropdownState = myEnum.dropdownState.LOADING;
                if (this.data.items.length <= 0){
                    await this.refreshListItems();
                }
                setTimeout(this.updateDropdownHeight.bind(this), 50);


                this.data.callFilter += 1;
                if (this.data.isFiltering){
                    this.data.filteredItems = await this.listItemFilter(this.data.items, String(this.data.text));
                } else {
                    this.data.filteredItems = this.data.items;
                }
                this.data.callFilter -= 1;

                // if (this.data.callFilter > 0) {
                //     // còn nhiều lời gọi call filter đằng sau nữa.
                //     return;
                // }
                if (this.data.filteredItems.length <= 0){
                    this.data.dropdownState = myEnum.dropdownState.EMPTY;
                } else {
                    this.data.dropdownState = myEnum.dropdownState.NORMAL;
                }
                setTimeout(this.updateDropdownHeight.bind(this), 50);
            } catch (error) {
                console.error(error);
            }
        },
        /* 
        * Xử lý yêu cầu làm mới danh sách items của combobox
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async refreshListItems(){
            try {
                let response = await this.listItemLoader();
                response ??= [];
                if (response != null && response.length > 0){
                    this.data.items = response.map(function(item){
                        if (Validator.isEmpty(item.state)){
                            item.state = myEnum.comboboxItemState.NORMAL;
                        }
                        return item;
                    });
                } else {
                    this.data.items = [];
                }
                // await this.resolveFilterItem();
                // console.log(this.data.filteredItems);
            } catch (error){
                console.error(error);
            }
        },

        // override resolveOnPressEnter
        async resolveOnPressEnter(){
            try {
                let text = this.data.text;
                let item = this.data.items.find(function(it){
                    return String(it.label) == String(text);
                });
                if (item != null){
                    await this.clickItemFunction(item);
                    await this.onPressEnter(item.value);
                } else {
                    this.blur();
                }
            } catch (error){
                console.log(error);
            }
        }
    },
    props: {
        ...input.props,
        oninput: {
            type: Function,
            default: async function(){}
        },
        placeholder: {
            type: String,
            default: "- Chọn giá trị -"
        },
        autocomplete: { default: "email" },
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
        listItemFilter: {
            type: Function,
            default: async function(items, text){
                await new Promise(e => setTimeout(e, 500));
                if (text === null || text === undefined){
                    text = "";
                }
                if (Validator.isEmpty(items)) return [];
                return items.filter(function(item){
                    return Unicode.unicodeToAscii(item.label).includes(Unicode.unicodeToAscii(text));
                });
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
export default combobox;
</script>

<style>
    @import url(@/css/base/combobox.css);
</style>


