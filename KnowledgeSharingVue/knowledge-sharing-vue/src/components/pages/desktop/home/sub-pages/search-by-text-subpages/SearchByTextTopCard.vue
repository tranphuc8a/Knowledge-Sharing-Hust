

<template>
    <div class="p-search-by-text-top-card">
        <div class="card">
            <div class="p-card-top">
                <div class="p-sbttc-textfield-frame">
                    <MTextfieldButton 
                        placeholder="Nhập từ khóa tìm kiếm"
                        :is-show-icon="true" 
                        :is-show-title="false" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickSearch" 
                        :validator="null"
                        :oninput="throttleOnInput"
                        state="normal" ref="textfield"
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
    name: 'SearchByTextTopCard',
    components: {
        MTextfieldButton,
    },
    props: {
    },
    data(){
        return {
            router: useRouter(),
            route: useRoute(),
            searchText: '',
            mainItems: [],
            throttleOnInput: Debounce.throttle(this.resolveInputSearch.bind(this), 500),
        }
    },
    async created(){
        this.initItems();
        this.refreshItems();
    },
    async mounted(){
        try {
            let search = this.route.query.search;
            this.refreshNewSearchText(search);
        } catch (e){
            console.error(e);
        }
    },
    methods: {
        async initItems(){
            try {
                this.mainItems = [
                    { key: 'user', label: 'Người dùng', originLink: '/search-text/user', link: '' },
                    { key: 'post', label: 'Bài viết', originLink: '/search-text/post', link: '' },
                    { key: 'lesson', label: 'Bài giảng', originLink: '/search-text/lesson', link: '' },
                    { key: 'course', label: 'Khóa học', originLink: '/search-text/course', link: '' },
                    { key: 'question', label: 'Thảo luận', originLink: '/search-text/question', link: '' },
                ];
            } catch (e){
                console.error(e);
            }
        },
        async refreshItems(){
            try {
                let searchText = this.searchText;
                if (Validator.isEmpty(searchText)){
                    let search = this.route.query.search;
                    searchText = search ?? '';
                }
                for (let item of this.mainItems){
                    item.link = item.originLink + '?search=' + searchText;
                }
            } catch (e){
                console.error(e);
            }
        },
        async refreshNewSearchText(value){
            try {
                await this.$refs.textfield.setValue(value);
            } catch (e){
                console.error(e);
            }
        },
        async resolveClickSearch(){
            try {
                this.searchText = this.$refs.textfield.getValue();
                let query = { ...this.$route.query }; // Tạo một bản sao của query hiện tại
                query.search = this.searchText; // Cập nhật query mới
                this.$router.replace({ query }); // Thay thế URL với query mới
            } catch (e){
                console.error(e);
            }
        },
        async resolveInputSearch(value){
            try {
                this.searchText = value;
                this.refreshItems();
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
    },
    watch: {
        '$route.query.search'(val){
            this.refreshNewSearchText(val);
        }
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-search-by-text-top-card{
    width: 100%;
    max-width: 100%;
}

.p-search-by-text-top-card .card{
    padding: 8px 16px 0px 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 8px;
}

.p-card-top{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    padding: 8px 0px;
    align-items: center;
}

.p-card-bottom{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
}

.p-sbttc-textfield-frame{
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

