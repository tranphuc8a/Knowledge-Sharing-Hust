
<template>
        <div class="p-table-frame" :state="tableState">
            
            <div class="p-table-toolbar" v-show="mode != 'view'">
                <div class="p-table-left-toolbar" :style="{ visibility: listChosen.length > 0 ? 'visible' : 'hidden'}">
                    <div class="">
                        {{ getLabel().toolbar.chosen }}
                        <span class="p-table-number-chosen"> {{ listChosen.length }}/{{ items.length }} </span>
                    </div>
                    <LinkButton :onclick="resolveUncheckAll" :label="getLabel().toolbar.unChoose" />
                    <CancelIconButton :onclick="resolveDeleteMulti" faClassname="pi-icon-smaller pi-trash-can" 
                                        color="red" :label="getLabel().toolbar.deleteChosen"/>
                </div>

                <div class="p-table-right-toolbar">
                    <slot name="rightToolbar">
                        <Textfield :placeholder="label.toolbar.find" type="text" :isShowHeader="false" :isShowTitle="false" :isShowError="false"/>
                        <ActionIcon :onclick="resolveRefresh" faClassname="pi-sprite-refresh-dark" :style="iconStyle" :iconStyle="iconStyle"/>
                    </slot>
                </div>
            </div>
            <div class="p-table-container">
                <div class="p-table-content">
                    <table class="p-table">
                        <tbody>
                            <tr v-for="(item, itemIndex) in listFilledItems" :key="itemIndex"
                                v-on:dblclick="resolveEdit(item)"
                            >
                                <td class="p-table-column p-table-column-checkbox"> 
                                    <CheckboxButton :onChecked="resolveCheck(item, itemIndex)" 
                                                    :onUnchecked="resolveUncheck(item, itemIndex)" 
                                                    :isChecked="item.isChosen" ref="itemCheckbox"/>
                                </td>
                                <td v-for="(header, headerIndex) in headers" :key="headerIndex"
                                    :class="getColumnClass(header, headerIndex)"
                                    :style="getCellStyle(item, headerIndex)">
                                    <span v-if="headerIndex < headers.length - 1">
                                        <!-- {{ item[header.field] }} -->
                                        <div v-html="item[header.field]"></div>
                                    </span>
                                    <div class="p-table-last-cell" v-if="headerIndex === headers.length - 1">
                                        <!-- <span> {{ item[header.field] }} </span> -->
                                        <span>  <div v-html="item[header.field]"></div> </span>
                                        
                                        <div class="p-table-actions" v-show="mode != 'view'">
                                            <label for="" :title="getLabel().functionality.edit">
                                                <ActionIcon faClassname="pi-pencil-solid-green pi-icon-smaller"
                                                    :onclick="resolveEdit(item)"/>
                                            </label>
                                            <label for="" :title="getLabel().functionality.option">
                                                <ContextMenu 
                                                        :position="getItemMenuContextPosition(item, itemIndex)"
                                                        :items="getItemMenuContextItems(item, itemIndex)"
                                                >
                                                    <ActionIcon faClassname="pi-icon-smaller pi-ellipsis-solid-green"/>
                                                </ContextMenu>
                                            </label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>     
                        <thead>
                            <tr class="p-table-header">
                                <th class="p-table-column p-table-column-checkbox"> 
                                    <CheckboxButton :on-checked="resolveCheckAll" :on-unchecked="resolveUncheckAll" 
                                                    :isChecked="isCheckedAll" ref="headerCheckbox" />
                                </th>
                                <th v-for="(item, index) in headers" :key="index"
                                        :class="getColumnClass(item, index)">
                                        <label :title="item.title">
                                            {{ item.label }}
                                        </label>
                                </th>
                            </tr>  
                        </thead>   
                    </table>
                </div>
                <div class="p-table-footer">
                    <div class="p-table-left-footer">
                        <div class="p-table-sumary">
                            {{ getLabel().footer.sum }} {{ items.length }}
                        </div>
                    </div>
                    <div class="p-table-right-footer">
                        <div class="p-table-numrecords">
                            <p> {{ getLabel().footer.recordPerSheet }} </p>
                            <select v-model="pageSize" name="" id="" @:change="refillTableFromStartIndex">
                                <option value="5">5</option>
                                <option value="10">10</option>
                                <option value="20">20</option>
                                <option value="50">50</option>
                            </select>
                        </div>
                        <div class="p-table-records-index">
                            {{ startIndex + 1 }}-{{ endIndex + 1 }} {{ getLabel().footer.record }}
                        </div>
                        <div class="p-table-navigate">
                            <ActionIcon :onclick="prevPage" faClassname="pi-chevron-left-solid-gray pi-icon" 
                                        :style="iconStyle" :iconStyle="iconStyle" :state="isPrev() ? 'normal' : 'disable'"/>
                            <ActionIcon :onclick="nextPage" faClassname="pi-chevron-right-solid-gray pi-icon" 
                                        :style="iconStyle" :iconStyle="iconStyle" :state="isNext() ? 'normal' : 'disable'"/>
                        </div>
                    </div>
                </div>
            </div>

            <div class="p-table-loading">
                <Spinner color="green" size="large" />
            </div>
            <div class="p-table-empty">
                {{ getLabel().empty }}
            </div>
        </div>
</template>

<script>
import LinkButton from '../buttons/MLinkButton.vue';
import CancelIconButton from '../buttons/MCancelIconButton.vue';
import Textfield from '../inputs/MTextfield.vue';
import ActionIcon from '../icons/MActionIcon.vue';
import CheckboxButton from '../inputs/MCheckboxButton.vue';
import ContextMenu from '../context-menu/MContextMenu.vue';
import { myEnum } from '@/js/resources/enum';
import Spinner from '@/components/base/icons/MSpinner.vue';

export default {
    name: "MyTable",
    data(){
        return {
            label: null,
            isCheckedAll: false,
            tableState: myEnum.tableState.NORMAL,
            pageSize: 10,
            startIndex: 0,
            endIndex: Math.min(this.pageSize - 1, this.items.length - 1),
            listChosen: [],
            dItems: [],
            listFilledItems: [],
            iconStyle: {
                width: "32px",
                height: "32px",
                'font-size': "18px"
            }
        }
    },
    mounted(){
        this.getLabel();
        this.refreshTable();
    },
    watch: {
        items(){
            this.refreshTable();
        }
    },
    methods: {
        /**
         * Lấy về label chứa các nhãn trong language resource
         * @param none
         * @return label
         * Created: PhucTV (30/1/24)
         * Modified: None
        */
        getLabel(){
            if (this.label === null || this.label === undefined){
                this.label = this.lang.components.table;
            }
            return this.label;
        },
        /*
        * Cập nhật các checkbox xem có phải checked tất cả row chưa
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async updateIsCheckedAll(){
            try {
                this.isCheckedAll = true;
                for (let i = this.startIndex; i <= this.endIndex; i++){
                    if (!this.dItems[i].isChosen){
                        this.isCheckedAll = false;
                        break;
                    }
                }
                if (this.dItems.length == 0){
                    this.isCheckedAll = false;
                }
                this.$refs.headerCheckbox.checked(this.isCheckedAll);
            } catch (error){
                console.error(error);
            }
        },
        /*
        * Bắn ra các event cho component cha xử lý
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        * Các event: table-delete-item, table-delete-multi, table-read, table-delete-item, table-clone-item
        **/
        async resolveDeleteMulti(){
            this.$emit('table-delete-multi', this.listChosen);
        },
        async resolveRefresh(){
            this.$emit('table-read');
        },
        resolveDelete(item){
            let that = this;
            return async function(){
                that.$emit('table-delete-item', item);            
            }
        },
        resolveEdit(item){
            let that = this;
            return async function(){
                that.$emit('table-edit-item', item);
            }
        },
        resolveClone(item){
            let that = this;
            return async function(){
                that.$emit('table-clone-item', item);
            }
        },
        /*
        * Chuyển đổi trạng thái table: normal, loading, empty
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async setState(state){
            this.tableState = state;
        },
        /*
        * Thực hiện check tất cả các row trong table
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveCheckAll(){
            try {
                let that = this;
                this.listFilledItems.forEach(function(item, index){
                    item.isChosen = true;
                    that.$refs.itemCheckbox[index].checked();
                });
                this.listChosen = [...this.listFilledItems];
                this.updateIsCheckedAll();
            } catch (error){
                console.error(error);
            }
        },
        /*
        * Thực hiện uncheck toàn bộ row trong table
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveUncheckAll(){
            try {
                let that = this;
                this.listFilledItems.forEach(function(item, index){
                    item.isChosen = false;
                    that.$refs.itemCheckbox[index].checked(false);
                });
                this.listChosen = [];
                this.updateIsCheckedAll();
            } catch (error) {
                console.error(error);
            }
        },
        /*
        * Hai method kiểm tra xem có thể navigate tới trang sau/trang trước hay không
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        isNext(){
            return this.endIndex < this.dItems.length - 1;
        },
        isPrev(){
            return this.startIndex > 0;
        },
        /*
        * Hai hàm Xử lý sự kiện check/uncheck một checkbox của row
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        resolveCheck(item){
            let that = this;
            return async function(){
                try {
                    that.listChosen.push(item);
                    item.isChosen = true;
                    that.updateIsCheckedAll();
                } catch (error){
                    console.error(error);
                }
            }
        },
        resolveUncheck(item){
            let that = this;
            return async function(){
                try {
                    let id = that.listChosen.indexOf(item);
                    that.listChosen.splice(id, 1);
                    item.isChosen = false;
                    that.updateIsCheckedAll();
                } catch (error){
                    console.error(error);
                }
            }
        },
        /*
        * Cập nhật lại listFilledItems từ startIndex khi pageSize thay đổi
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async refillTableFromStartIndex(){
            try {
                this.endIndex = Math.min(this.dItems.length - 1, this.startIndex + Number(this.pageSize) - 1);
                this.listFilledItems = this.dItems.slice(this.startIndex, this.endIndex + 1);
                if (this.listFilledItems.length <= 0){
                    this.setState(myEnum.tableState.EMPTY);
                } else {
                    this.setState(myEnum.tableState.NORMAL);
                }
            } catch (error){
                console.error(error);
            }
        },
        /*
        * Cập nhật lại table bắt đầu từ props items
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async refreshTable(){
            try {
                this.resolveUncheckAll();
                this.dItems = this.items.map(function(item){
                    item.isChosen = false;
                    return item;
                });
                if (this.startIndex >= this.dItems.length){
                    this.startIndex = 0;
                }
                this.endIndex = Math.min(this.startIndex + this.pageSize - 1, this.dItems.length - 1);
                this.listFilledItems = this.dItems.slice(this.startIndex, this.endIndex + 1);
                await this.refillTableFromStartIndex();
            } catch (error) {
                console.error(error);
            }
        },
        /*
        * Hai methods thực hiện navigate tới page trước và page sau
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async prevPage(){
            try {
                if (!this.isPrev()){
                    return;
                }
                this.resolveUncheckAll();
                this.startIndex = this.startIndex - Number(this.pageSize);
                if (this.startIndex < 0){
                    this.startIndex = 0;
                }
                this.refillTableFromStartIndex();
            } catch (error){
                console.error(error);
            }
        }, async nextPage(){
            try {
                if (!this.isNext()){
                    return;
                }
                this.resolveUncheckAll();
                this.startIndex = this.startIndex + Number(this.pageSize);
                if (this.startIndex > this.dItems.length - 1){
                    this.startIndex -= Number(this.pageSize);
                }
                this.refillTableFromStartIndex();
            } catch (error){
                console.error(error);
            }
        },
        /*
        * Tính toán thuộc tính class cho mỗi thẻ th/td để trích xuất css class phù hợp
        * @param item, index: item và chỉ số header của nó
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        getColumnClass(item, index){
            try {
                let firstColumn = index == 0 ? 'p-table-first-column' : '';
                let lastColumn = index == this.headers.length - 1 ? 'p-table-last-column p-table-column-function' : '';
                let columnJustify = `p-table-column-justify-${item.justify}`;
                return `p-table-column ${firstColumn} ${lastColumn} ${columnJustify}`;
            } catch (error){
                console.error(error);
                return "";
            }
        },
        /*
        * Lấy về style cho ô hiện tại nếu có
        * @param item, index: item và chỉ số header của nó
        * @Author TVPhuc (16/1/24)
        * @Edit None
        **/
        getCellStyle(item, index){
            try {
                let cssField = this.headers[index].field;
                if (item.css && item.css[cssField]){
                    return item.css[cssField];
                }
                return null;
            } catch (error){
                console.error(error);
                return null;
            }
        },
        /*
        * Lấy giá trị truyền vào props của menu context cho từng item
        * @param {*} item: customer được focus menu Context, index: chỉ số
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        getItemMenuContextPosition(item, index){
            try {
                let top = 'top';
                if (2*index < this.listFilledItems.length){
                    top = 'bottom';
                }
                return `${top} right`;
            } catch (error){
                console.error(error);
            }
        },
        getItemMenuContextItems(item){
            let that = this;
            return {
                top: [{
                    faClassname: 'pi-icon-smaller pi-pencil-solid-green',
                    label: that.label.contextMenu.edit,
                    onclick: async function(){
                        await that.resolveEdit(item)();
                    }
                }, {
                    faClassname: 'pi-icon-smaller pi-clone-solid-green',
                    label: that.label.contextMenu.clone,
                    onclick: async function(){
                        await that.resolveClone(item)();
                    }
                }],
                bottom: [{
                    faClassname: 'pi-icon-smaller pi-trash-can',
                    color: 'red',
                    label: that.label.contextMenu.delete,
                    onclick: async function(){
                        await that.resolveDelete(item)();
                    }
                }]
            }
        }
    },
    components: {
        LinkButton, CancelIconButton, Textfield, ActionIcon, CheckboxButton, ContextMenu, Spinner
    },  
    props: {
        headers: {
            type: Array,
            default:()=>[{
                    type: String,
                    label: "Mã nhân viên",
                    field: "userid",
                    justify: "left"
                },{
                    type: String,
                    label: "Họ tên",
                    field: "username",
                    justify: "left"
                },{
                    type: Number,
                    label: "Tiền nợ",
                    field: "debt",
                    justify: "right"
                }
            ]
        },
        items: {
            type: Array,
            default: ()=>[]
        },
        mode: {
            type: String,
            default: 'normal'
        }
    }
};
</script>

<style>
@import url(@/css/base/table.css);
</style>
