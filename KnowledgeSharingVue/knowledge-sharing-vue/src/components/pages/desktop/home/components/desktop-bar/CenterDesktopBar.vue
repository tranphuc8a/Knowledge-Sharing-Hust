
<template>
    <div class="d-centerbar">
        <div class="d-centerbar__icon">
            <div class="d-centerbar__icon-bg" :title="getLabel()?.home">
                <router-link class="p-router-link" to="home">
                    <MIcon :style="iconStyle" fa="home" />
                </router-link >
            </div>
        </div>
        <div class="d-centerbar__icon">
            <div class="d-centerbar__icon-bg" :title="getLabel()?.lesson">
                <router-link class="p-router-link" to="lessons">
                    <MIcon :style="iconStyle" fa="book-open" />
                </router-link >
            </div>
        </div>
        <div class="d-centerbar__icon">
            <div class="d-centerbar__icon-bg" :title="getLabel()?.course">
                <router-link class="p-router-link" to="courses">
                    <MIcon :style="iconStyle" fa="layer-group" />
                </router-link >
            </div>
        </div>
        <div class="d-centerbar__icon">
            <div class="d-centerbar__icon-bg" :title="getLabel()?.question">
                <router-link class="p-router-link" to="questions">
                    <MIcon :style="iconStyle" fa="comments" />
                </router-link >
            </div>
        </div>
    </div>
</template>

<script>

import { useRouter } from 'vue-router';
export default {
    name: "CenterDesktopBar",
    data() {
        return {
            iconStyle: { fontSize: '24px'},
            router: useRouter()
        }
    },
    mounted() {
        this.getLabel();
    },
    components: {
        
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

.d-centerbar__icon svg{
    color: var(--grey-color);
}
.p-router-link{
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
}
.d-centerbar__icon{
    border-top: solid var(--transparent-color) 3px;
    border-bottom: solid var(--transparent-color) 3px;
}
.d-centerbar__icon,
.d-centerbar__icon-bg{
    display: flex;
    flex-flow: row nowrap;
    flex: 1;
    height: 100%;
    border-radius: 3px;
    color: var(--grey-color);
    justify-content: center;
    align-items: center;
    cursor: pointer;
    box-sizing: border-box;
}
.d-centerbar__icon-bg{
    width: 80%;
    height: 95%;
    border-radius: 8px;
}
.d-centerbar__icon-bg:hover{
    background-color: var(--grey-color-200);
}
.d-centerbar__icon:has(.router-link-active){
    color: var(--primary-color);
    border-bottom: solid var(--primary-color) 3px;
}
.d-centerbar__icon:has(.router-link-active) svg{
    color: var(--primary-color);
}
</style>