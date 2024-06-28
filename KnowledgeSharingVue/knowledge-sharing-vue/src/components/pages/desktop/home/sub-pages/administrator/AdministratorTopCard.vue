

<template>
    <div class="p-administrator-top-card">
        <div class="card">
            <div class="p-card-top">
                <div class="p-atc-textfield-frame">
                    <MTextfieldButton 
                        placeholder="Nhập từ khóa tìm kiếm"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickSearch" 
                        :validator="null"
                        :oninput="throttleOnInputSearch"
                        state="normal" ref="search-textfield"
                    /> 
                </div>
                <div class="p-atc-textfield-frame">
                    <MTextfieldButton 
                        placeholder="Nhập ID"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickFilter" 
                        :validator="null"
                        :oninput="throttleOnInputFilter"
                        state="normal" ref="filter-textfield"
                    /> 
                </div>
            </div>
            <div class="p-devide"></div>
            <div class="p-card-bottom">
                <div class="p-pnav-left">
                    <div class="p-nav-item"
                        v-for="(item, index) in mainItems"
                        :key="item.key ?? index"
                    >
                        <router-link :to="item.link" class="p-nav-item-button router-link">
                            <div class="p-nav-item-text">
                                {{ item.label }} 
                            </div>
                        </router-link>
                    </div>
                </div>
        
                <div class="p-pnav-right">
                    <!-- More profile context button -->
                    <!-- <ProfilePanelMoreContextButton :items="moreItems"/> -->
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { useRouter } from 'vue-router';
import { useRoute } from 'vue-router';
import Debounce from '@/js/utils/debounce';
import MTextfieldButton from '@/components/base/inputs/MTextfieldButton.vue';
import { Validator } from '@/js/utils/validator';

export default {
    name: 'AdministratorTopCard',
    components: {
        MTextfieldButton,
    },
    props: {
    },
    data(){
        return {
            router: useRouter(),
            route: useRoute(),
            search: null,
            filter: null,
            mainItems: [],
            throttleOnInputSearch: Debounce.throttle(this.resolveInputSearch.bind(this), 500),
            throttleOnInputFilter: Debounce.throttle(this.resolveInputFilter.bind(this), 500),
        }
    },
    async mounted(){
        this.initItems();
        this.createItems();
    },
    methods: {
        async initItems(){
            try {
                this.mainItems = [
                    { key: 'user', label: 'Người dùng', originLink: '/administrator/user', link: '' },
                    { key: 'post', label: 'Bài viết', originLink: '/administrator/post', link: '' },
                    { key: 'lesson', label: 'Bài giảng', originLink: '/administrator/lesson', link: '' },
                    { key: 'course', label: 'Khóa học', originLink: '/administrator/course', link: '' },
                    { key: 'question', label: 'Thảo luận', originLink: '/administrator/question', link: '' },
                ];
            } catch (e){
                console.error(e);
            }
        },
        async createItems(){
            try {
                let search = this.search ?? this.route.query.search;
                let filter = this.filter ?? this.route.query.filter;
                if (Validator.isNotEmpty(filter)){
                    this.refreshNewFilter(filter);
                } else {
                    this.refreshNewSearch(search);
                }
            } catch (e){
                console.error(e);
            }
        },
        async refreshItems(){
            try {
                let search = this.search ?? this.route.query.search;
                let filter = this.filter ?? this.route.query.filter;
                if (Validator.isNotEmpty(filter)){
                    for (let item of this.mainItems){
                        item.link = item.originLink + '?filter=' + filter;
                    }
                } else if (Validator.isNotEmpty(search)){
                    for (let item of this.mainItems){
                        item.link = item.originLink + '?search=' + search;
                    }
                } else {
                    for (let item of this.mainItems){
                        item.link = item.originLink;
                    }
                }
            } catch (e){
                console.error(e);
            }
        },
        async refreshNewFilter(value){
            try {
                // await this.$refs.textfield.setValue(value);
                this.filter = value;
                this.search = '';
                await this.$refs['search-textfield'].setValue('');
                await this.$refs['filter-textfield'].setValue(value);
                await this.refreshItems();
            } catch (e){
                console.error(e);
            }
        },
        async refreshNewSearch(value){
            try {
                // await this.$refs.textfield.setValue(value);
                this.search = value;
                this.filter = '';
                await this.$refs['filter-textfield'].setValue('');
                await this.$refs['search-textfield'].setValue(value);
                await this.refreshItems();
            } catch (e){
                console.error(e);
            }
        },
        async resolveClickSearch(){
            try {
                // this.category = await this.$refs.textfield.getValue();
                // let query = { ...this.route.query };
                // query.category = this.category;
                // this.router.replace({ query });
                this.search = await this.$refs['search-textfield'].getValue();
                // await this.refreshNewSearch(this.search);
                let query = { ...this.route.query };
                query.search = this.search;
                delete query.filter;
                this.router.replace({ query });
            } catch (e){
                console.error(e);
            }
        },
        async resolveClickFilter(){
            try {
                // this.category = await this.$refs.textfield.getValue();
                // let query = { ...this.route.query };
                // query.category = this.category;
                // this.router.replace({ query });
                this.filter = await this.$refs['filter-textfield'].getValue();
                // await this.refreshNewFilter(this.filter);
                let query = { ...this.route.query };
                query.filter = this.filter;
                delete query.search;
                this.router.replace({ query });
            } catch (e){
                console.error(e);
            }
        },
        async resolveInputSearch(value){
            try {
                await this.refreshNewSearch(value);
            } catch (e){
                console.error(e);
            }
        },
        async resolveInputFilter(value){
            try {
                await this.refreshNewFilter(value);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
    },
    watch: {
        '$route.query.search'(val){
            this.refreshNewSearch(val);
        },
        '$route.query.filter'(val){
            this.refreshNewFilter(val);
        }
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-administrator-top-card{
    width: 100%;
    max-width: 100%;
}

.p-administrator-top-card .card{
    padding: 8px 16px 0px 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 8px;
}

.p-card-top{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    gap: 8px;
    padding: 8px 0px;
    align-items: center;
}

.p-card-top > *{
    width: 0;
    flex-grow: 1;
    flex-shrink: 1;
}

.p-card-bottom{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
}

.p-atc-textfield-frame{
    width: 80%;
}


.p-pnav-left{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 2px;
}

.p-pnav-right{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
    width: fit-content;
    max-width: 100%;
}

.p-nav-item{
    border-bottom: solid transparent 3px;
    padding-bottom: 1px;
}

.p-nav-item:has(.router-link-active){
    border-bottom: solid var(--primary-color-500) 3px;
}

.p-nav-item:has(.router-link-active) .p-nav-item-button{
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

