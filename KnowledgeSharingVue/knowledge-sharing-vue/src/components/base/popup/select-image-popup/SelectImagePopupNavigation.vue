

<template>
    <div class="p-select-image-popup-navigation">
        <div class="p-nav-left">
            <div class="p-nav-item"
                v-for="(item, index) in mainItems"
                :key="item.key ?? index"
            >
                <div :class="['p-nav-item-button', {'p-nav-item-active': item.key == tabIndex}]"
                    @:click="resolveOnTabChange(item.key)">
                    <div class="p-nav-item-text">
                        {{ item.label }} 
                    </div>
                </div>
            </div>
        </div>

        <div class="p-nav-right">
        </div>
    </div>
</template>



<script>
// import { myEnum } from '@/js/resources/enum';
// import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'SelectImagePopupNavigation',
    components: {
    },
    props: {
        onChangeTab: {
            type: Function,
            default: () => {}
        },
    },
    data(){
        return {
            listItems: [],
            mainItems: [],
            moreItems: [],
            tabEnum: {
                Gallery: 0,
                EnterUrl: 1,
                Upload: 2,
            },
            tabIndex: 0,
        }
    },
    async created(){
        await this.initItems();
        await this.refresh();
    },
    async mounted(){
    },
    methods: {
        async initItems(){
            try {
                this.listItems = { 
                    [this.tabEnum.Gallery]: { 
                        key: this.tabEnum.Gallery, 
                        label: 'Thư viện ảnh', 
                    },
                    [this.tabEnum.EnterUrl]: { 
                        key: this.tabEnum.EnterUrl, 
                        label: 'Nhập URL', 
                    },
                    [this.tabEnum.Upload]: { 
                        key: this.tabEnum.Upload, 
                        label: 'Tải lên', 
                    },
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                let listMainItemType = [
                    this.tabEnum.Gallery,
                    this.tabEnum.EnterUrl,
                    this.tabEnum.Upload,
                ]
                for (let type of listMainItemType){
                    this.mainItems.push(this.listItems[type]);
                }
                this.tabIndex = 0;
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnTabChange(tabIndex){
            try {
                this.tabIndex = tabIndex;
                this.onChangeTab?.(tabIndex);
            } catch (e){
                console.error(e);
            }
        },
    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-select-image-popup-navigation{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.p-nav-left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 2px;
}

.p-nav-item{
    border-bottom: solid transparent 3px;
    padding-bottom: 1px;
}

.p-nav-item:has(.p-nav-item-active){
    border-bottom: solid var(--primary-color-500) 3px;
}

.p-nav-item:has(.p-nav-item-active) .p-nav-item-button{
    color: var(--primary-color);
}

.p-nav-item-button{
    text-decoration: none;
    font-family: 'ks-font-semibold';
    color: var(--grey-color-600);
    cursor: pointer;
}

.p-nav-item-text{
    padding: 16px;
    border-radius: 4px;
    height: 52px;
    display: flex;
    flex-flow: row nowrap;
    justify-self: center;
    align-items: center;
}

.p-nav-item-text:hover{
    background-color: var(--grey-color-200);
}

</style>

