
<template>
    <div class="d-leftbar">
        <div class="d-leftbar__logo" @:click="resolveClickLogo">
        </div>
        <MTextfield 
            :placeholder="getLabel()?.searchPlaceholder"
            :is-show-icon="true" :is-show-title="false" :is-show-error="false" :is-obligate="false"
            :onclick-icon="resolveClickSearch" 
            :validator="null" ref="textfield"
            state="normal"/>
    </div>
</template>

<script>
import MTextfield from '@/components/base/inputs/MTextfield';
import { Validator } from '@/js/utils/validator';
import { useRouter } from 'vue-router';
export default {
    name: "LeftDesktopBar",
    data() {
        return {
            textfield: null,
            router: useRouter()
        }
    },
    mounted() {
        this.getLabel();
        this.textfield = this.$refs.textfield;
    },
    components: {
        MTextfield
    },
    methods: {
        /**
         * Hàm lấy nhãn ngôn ngữ
         * @param none
         * @returns none
         * @Created PhucTV (20/2/24)
         * @Modified None
        */
        getLabel(){
            if (this.inject?.language != null){
                this.label = this.inject?.language?.pages?.homepage?.homeDbarResource;
            }
            return this.label;
        },

        /**
         * Hàm xử lý khi click vào icon search
         * @param none
         * @returns none
         * @Created PhucTV (12/04/24)
         * @Modified None
        */
        async resolveClickSearch(){
            try {
                let text = await this.textfield.getValue();
                if (Validator.isEmpty(text)){
                    // text is null then do nothing
                    return;
                }
                // navigate to search page with search text
                // this.router.push({name: 'search', query: {text: text}});
            } catch (error) {
                console.error(error);
            }
        },

        /**
         * Hàm xử lý khi click vào logo trang web
         * @param none
         * @returns none
         * @Created PhucTV (12/04/24)
         * @Modified None
        */
        async resolveClickLogo(){
            try {
                // navigate or refresh homepage
                this.router.push('/');
            } catch (error) {
                console.error(error);
            }
        }
    },
    props: {

    },
    inject: {
        inject: {},
        getToastManager: {},
        getPopupManager: {}
    }
}
</script>

<style scoped>
@import url(@/css/pages/desktop/home/components/desktop-bar.css);
</style>