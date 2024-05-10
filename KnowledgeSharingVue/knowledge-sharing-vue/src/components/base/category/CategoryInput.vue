<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate" :is-full="inputFrame.isFull" 
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-category-textfield" @:click="resolveClickExpand">
            <div class="p-category-textfield-left">
                <CategoryItem 
                    v-for="(category, index) in data.listedCategories"
                    :key="category + index"
                    :category="category"
                    :is-editing="true"
                    :on-delete-category="resolveDeleteCategory(category)"
                />
                <input type="text" class="p-category-input" v-model="data.text" 
                        :placeholder="data.placeholder" 
                        @:focus="resolveOnFocus" 
                        @:blur="resolveOnBlur"
                        @:input="resolveOnInput"
                        v-on:keyup.enter.prevent.stop="resolveOnPressEnter"
                        ref="input">
            </div>
            <div class="p-category-textfield-right">
                <div class="p-icon-container p-icon-down" @:click="resolveClickExpand">
                    <MIcon fa="chevron-down" />
                </div>
                <div class="p-icon-container p-icon-up" @:click="resolveClickCollapse">
                    <MIcon fa="chevron-up" />
                </div>
            </div>
        </div>
        <!-- STATE OF FRAME: normal, loading, empty -->
        <div class="p-category-dropdown-frame" :state="data.dropdownState" ref="dropdownFrame">
            <div class="p-category-dropdown-content" ref="dropdownContent">
                <div class="p-category-dropdown-loading">
                    <MSpinner :style="{ fontSize: '24px' }" />
                </div>
                <div class="p-category-dropdown-empty">
                    {{ getLabel()?.noOptions }}
                </div>
                <div class="p-category-dropdown-items">
                    <!-- ITEMS STATE: normal, default, chosen, focus -->
                    <div v-for="(item, index) in data.filteredItems" :key="index"   
                        class="p-category-item" :state="item.state"
                        @:click="(clickItemFunction(item))()">
                        <div class="p-category-item-label" :title="item.label">
                            {{ item.label }}
                        </div>  
                        <div class="p-category-item-checked">
                            <MIcon fa="check" />
                        </div>
                    </div>                            
                </div>
            </div>
        </div>
    </InputFrame>
</template>


<script>
import CategoryItem from './CategoryItem.vue';
import { input } from '@/js/components/base/input';
import InputFrame from '../inputs/MInputFrame.vue';
import { myEnum } from '@/js/resources/enum';
import { Validator } from '@/js/utils/validator';
import { Unicode } from '@/js/utils/unicode';
import { GetRequest, Request } from '@/js/services/request';

export default {
    name: 'CategoryInput',
    components: {
        InputFrame, CategoryItem
    },
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
                listedCategories: [],
                text: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,

                placeholder: this.placeholder,
                dropdownState: myEnum.dropdownState.NORMAL,
                items: [],
                filteredItems: [],
                callFilter: 0,
                isFiltering: false,

                label: null
            },
            components: {
                dropdownContent: null,
                dropdownFrame: null,
                input: null
            },
            iconStyle: {
                fontSize: '16px',
                color: 'var(--primary-color)',
                cursor: 'pointer'
            },
            isLoadingCategories: false
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
        // listItemLoader: {
        //     type: Function,
        //     default: async function(){
        //         await new Promise(e => setTimeout(e, 1000));
        //         return Array.from([{
        //                 label: 'Hưng Yên',
        //                 value: 'Hưng Yên'
        //             }, {
        //                 label: 'Hà Nội',
        //                 value: 'Hà Nội'
        //             }, {
        //                 label: 'Nam Định',
        //                 value: 'Nam Định'
        //             }, ]);
        //     }
        // },
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
    methods:{
        ...input.methods,

        async listItemLoader(){
            if (this.isLoadingCategories) return;
            try {
                let res = await new GetRequest('Categories')
                    .execute();
                let body = await Request.tryGetBody(res);
                let listCategories = body.map(function(cat){
                    return {
                        label: cat.CategoryName,
                        value: cat.CategoryName
                    }
                });
                return listCategories;
            } catch (error){
                console.error(error);
                return [];
            } finally {
                this.isLoadingCategories = true;
            }
        },

        getLabel(){
            if (this.data?.label == null){
                this.data.label = this.lang?.components?.input?.combobox;
            }
            return this.data.label;
        },

        resolveDeleteCategory(category){
            let that = this;
            return async function(){
                try {
                    let index = that.data.listedCategories.indexOf(category);
                    if (index >= 0){
                        that.data.listedCategories.splice(index, 1);
                    }
                } catch (error){
                    console.error(error);
                }
            }
        },


        /**
        * Override validate function
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async validate(){
            try {
                if (this.validator != null){
                    let value = await this.getValue();
                    let res = await this.validator.validate(value);
                    this.inputFrame.errorMessage = res.msg;
                    return res.isValid;
                }
            } catch (error){
                console.error(error);
                return false;
            }
        },
        /**
        * Thực hiện xử lý logic sau khi thay đổi giá trị của input live
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnInput(){
            try {
                this.data.isFiltering = true;
                await this.resolveFilterItem();
                await this.oninput?.();
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
                await this.onfocus?.();
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
                let that = this;
                this.data.isFiltering = false;
                // do combobox logic when change to new value 
                await new Promise(e => setTimeout(e, 250));
                await that.setState(myEnum.inputState.NORMAL);
                await that.resolveOnChange?.();
                await that.onblur?.();
                // this.$nextTick(async function(){
                //     await new Promise(e => setTimeout(e, 250));
                //     await that.setState(myEnum.inputState.NORMAL);
                //     await that.resolveOnChange?.();
                //     await that.onblur?.();
                // });               
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
                this.data.text = "";
                if (value?.length > 0){
                    this.data.listedCategories = value;
                } else {
                    this.data.listedCategories = [];
                }
            } catch (error){
                console.error(error);
            }
        },

        /* Override
        * Hàm thực hiện lay danh sach category da nhap
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async getValue(){
            try {
                return this.data.listedCategories;
            } catch (error){
                console.error(error);
                return null;
            }
        },


        // OTHER METHODS:
        /* *
        * Xử lý logic khi click chuột vào một item của combobox
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        clickItemFunction(item){
            return async function(){
                try {
                    this.data.text = "";
                    this.data.listedCategories.push(item.label);

                    console.log("clicked");
                    let that = this;
                    
                    await that.resolveOnBlur();
                    await that.setState(myEnum.inputState.NORMAL);
                    await that.resolveOnChange?.();
                    // this.$nextTick(async function(){
                    //     try {
                    //         await new Promise(e => setTimeout(e, 550));
                    //         await that.resolveOnBlur();
                    //         await that.setState(myEnum.inputState.NORMAL);
                    //         await that.resolveOnChange?.();
                    //     } catch (e) {
                    //         console.error(e);
                    //     }
                    // });
                } catch (error){
                    console.error(error);
                }
            }.bind(this);
        },  
        /* *
        * Hai hàm xử lý sự kiện click vào nút expand và collapse
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveClickExpand(){
            await this.$refs.input.focus();
            // await this.resolveOnFocus();
        },
        async resolveClickCollapse(){
            await this.$refs.input.blur();
            // await this.resolveOnBlur();
        },
        /**
        * Cập nhật chiều cao của dropdownFrame dựa trên dropdownContent
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async updateDropdownHeight(){
            try {
                // let clH = 
                this.components.dropdownFrame.style.height = `${this.components.dropdownContent.clientHeight}px`;
            } catch (e) {
                console.error(e);
            }
        },
        /* *
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

                let text = this.data.text;
                if (Validator.isNotEmpty(text)) {
                    if (!this.data.filteredItems.some(function(item){
                        return item.value == text;
                    })) {
                        this.data.filteredItems.unshift({
                            label: text,
                            value: text
                        });
                    }
                }

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
        /* *
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
                    await (this.clickItemFunction(item))();
                    await this.onPressEnter(item.value);
                } else {
                    this.blur();
                }
            } catch (error){
                console.log(error);
            }
        }
    },
    mounted(){
        this.components.dropdownContent = this.$refs.dropdownContent;
        this.components.dropdownFrame = this.$refs.dropdownFrame;
        this.components.input = this.$refs.input;
    }
}

</script>


<style scoped>
@import url(@/css/base/input/category-input.css);
</style>


