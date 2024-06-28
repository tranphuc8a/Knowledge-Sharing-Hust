<template>
    <div class="p-context-menu" tabindex="1" @blur="resolveOnBlur">
        <div class="p-context-menu-mask" @:click="toggleShowContextMenu" ref="mask">
            <slot />
        </div>

        <div class="p-context-menu-container" ref="menu" v-show="isShowMenu">
            <!-- Items on context menu top -->
            <div class="p-context-menu-top" v-show="dItems.top.length > 0">
                <div class="p-context-menu-item" 
                    v-for="(item, index) in dItems.top" :key="index"
                    @:click="resolveClickItemFunction(item.onclick)">
                    <div class="p-context-menu-item-icon">
                        <MIcon :fa="item.fa" :style="item.style" />
                    </div>
                    <div class="p-context-menu-item-title">
                        {{ item.label }}
                    </div>
                </div>
            </div>
            <div class="p-context-menu-devide" v-show="dItems.top.length > 0 && dItems.bottom.length > 0">
                <div> </div>
            </div>
            <!-- Items on context menu bottom -->
            <div class="p-context-menu-bottom" v-show="dItems.bottom.length > 0">
                <div class="p-context-menu-item" 
                    v-for="(item, index) in dItems.bottom" :key="index"
                    @:click="resolveClickItemFunction(item.onclick)">
                    <div class="p-context-menu-item-icon">
                        <MIcon :fa="item.fa" :style="item.style" />
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
        /**
        * Hiển thị hoặc ẩn menu context
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async toggleShowContextMenu(){
            try {
                this.isShowMenu = !this.isShowMenu;
            } catch (error){
                console.error(error);
            }
        },
        
        /**
        * Xử lý khi unfocus menu (ẩn menu)
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async resolveOnBlur(){
            try {
                this.isShowMenu = false;
            } catch (error){
                console.error(error);
            }
        },
        
        /**
        * Xử lý khi click item trong menu dropdown
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async resolveClickItemFunction(itemCallback){
            try {
                await itemCallback();
                await this.toggleShowContextMenu();
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Lấy classname cho item icon
        * @param none
        * @return className của icon cần lấy
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        getItemIconClassname(item){
            return `fa ${item.faClassname} p-icon p-${item.color}-icon`;
        },
        /**
        * Định dạng lại danh sách các items
        * @param none
        * @Author TVPhuc (20/12/23)
        * @Edit None
        **/
        async formatItems(){
            try {
                // Đặt các giá trị mặc định cho item nếu props truyền thiếu dữ liệu
                let temp = this.items;
                let formatItem = function(item){
                    let keys = ['fa', 'style', 'label', 'onclick'];
                    let defaults = ['book', null, 'Đọc sách', async function(){}];
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
        /**
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
        position: { default: "bottom left" },
        items: {
            default: { // example about items form
                top: [{
                    fa: 'user-plus',
                    style: null,
                    label: 'Thêm người dùng',
                    onclick: async function(){}
                }, {
                    fa: 'book',
                    label: 'Đọc sách',
                    onclick: async function(){}
                }],
                bottom: [{
                    fa: 'trash-can',
                    style: { color: 'var(--red-color)'},
                    label: 'Xóa',
                    onclick: async function(){ }
                }]
            }
        }
    }

}

</script>


<style>
@import url(@/css/base/others/context-menu.css);
</style>
