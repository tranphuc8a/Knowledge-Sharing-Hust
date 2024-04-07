<template>
    <div class="p-context-menu" tabindex="1" @focus="resolveOnFocus" @blur="resolveOnBlur">
        <div class="p-context-menu-mask" @:click="showContextMenu"
            ref="mask">
            <slot />
        </div>

        <!-- <input type="text" ref="input"
            /> -->
        <div class="p-context-menu-container" ref="menu" v-show="isShowMenu">
            <!-- Items on context menu top -->
            <div class="p-context-menu-top" v-show="dItems.top.length > 0">
                <div class="p-context-menu-item" v-for="(item, index) in dItems.top" :key="index"
                    @:click="resolveClickItemFunction(item.onclick)">
                    <div class="p-context-menu-item-icon">
                        <i :class="getItemIconClassname(item)"></i>
                    </div>
                    <div class="p-context-menu-item-title">
                        {{ item.label }}
                    </div>
                </div>
            </div>
            <div class="p-context-menu-devide" v-show="dItems.bottom.length > 0">
                <div></div>
            </div>
            <!-- Items on context menu bottom -->
            <div class="p-context-menu-bottom" v-show="dItems.bottom.length > 0">
                <div class="p-context-menu-item" v-for="(item, index) in dItems.bottom" :key="index"
                    @:click="resolveClickItemFunction(item.onclick)">
                    <div class="p-context-menu-item-icon">
                        <i :class="getItemIconClassname(item)"></i>
                    </div>
                    <div class="p-context-menu-item-title">
                        {{ item.label }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
import { Validator } from '@/js/utils/validator';


export default {
    data(){
        return {
            isShowMenu: false,
            dItems: {
                top: [],
                bottom: []
            },
            menu: null,
            input: null,
            mask: null
        }
    },
    mounted(){
        this.menu = this.$refs.menu;
        // this.input = this.$refs.input;
        this.mask = this.$refs.mask;
        this.formatItems();
        this.updateMenuContextPosition();
    },
    methods: {
        /* 
        * Hiển thị context menu
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async showContextMenu(){
            try {
                // await this.input.focus();
            } catch (error){
                console.error(error);
            }
        },
        /* 
        * Xử lý khi focus menu
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async resolveOnFocus(){
            try {
                this.isShowMenu = true;
            } catch (error){
                console.error(error);
            }
        },
        /* 
        * Xử lý khi unfocus menu
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async resolveOnBlur(){
            try {
                // await new Promise(e => setTimeout(e, 200));
                this.isShowMenu = false;
            } catch (error){
                console.error(error);
            }
        },
        /* 
        * Xử lý khi click item
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async resolveClickItemFunction(itemCallback){
            try {
                // console.log("do call back");
                await itemCallback();
                // console.log("do call back done");
                // await this.input.blur();
            } catch (error){
                console.error(error);
            }
        },
        /* 
        * Lấy classname cho item icon
        * @param none
        * @return className của icon cần lấy
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        getItemIconClassname(item){
            return `fa ${item.faClassname} p-icon p-${item.color}-icon`;
        },
        /* 
        * Định dạng lại danh sách các items
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async formatItems(){
            try {
                // Đặt các giá trị mặc định cho item nếu props truyền thiếu
                let temp = this.items;
                let formatItem = function(item){
                    let keys = ['faClassname', 'color', 'label', 'onclick'];
                    let defaults = ['pi-icon pi-book', 'green', 'Đọc sách', async function(){}];
                    for (let i = 0; i < 4; i++){
                        if (Validator.isEmpty(item[keys[i]])){
                            item[keys[i]] = defaults[i];
                        }
                    }
                };
                temp.top.forEach(function(item){
                    formatItem(item);
                });
                temp.bottom.forEach(function(item){
                    formatItem(item);
                })
                this.dItems = temp;
            } catch (error){
                console.error(error);
            }
        },
        /* 
        * Cập nhật vị trí menu context
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async updateMenuContextPosition(){
            try {
                this.menu.style.position = 'absolute';
                let position = this.position;
                // tách position ra làm 2 vị trí : first, second
                let arrWords = await position.split(' ').filter(word => word !== '');
                let first = arrWords[0];
                let second = arrWords[1];

                let mWidth = this.mask.clientWidth;
                let mHeight = this.mask.clientHeight;
                if (['top', 'bottom'].includes(first)){
                    first = (first === 'top') ? 'bottom' : 'top';
                    this.menu.style[first] = `${mHeight}px`;
                } else if (['left', 'right'].includes(first)){
                    first = (first === 'left') ? 'right' : 'left';
                    this.menu.style[first] = `${mWidth}px`;
                }
                this.menu.style[second] = '0px';
            } catch (error){
                console.error(error);
            }
        },
    },
    watch: {
        items() { this.formatItems(); },
        position() { this.updateMenuContextPosition(); }
    },
    props: {
        position: { default: "bottom right" },
        items: {
            default: { // example about items form
                top: [{
                    faClassname: 'pi-icon pi-book',
                    color: 'green',
                    label: 'Đọc sách',
                    onclick: async function(){}
                }, {
                    faClassname: 'pi-icon pi-book',
                    color: 'green',
                    label: 'Đọc sách',
                    onclick: async function(){}
                }],
                bottom: [{
                    faClassname: 'pi-icon pi-book',
                    color: 'red',
                    label: 'Đọc sách',
                    onclick: async function(){ console.log("xóa"); }
                }]
            }
        }
    }

}

</script>


<style>
@import url(@/css/base/context-menu.css);
</style>
